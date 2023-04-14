using Application.Interfaces;
using Application.Queries;
using Client.Abstraction;
using Client.Abstractions.Models;
using Cqrs.Handlers;
using Cqrs.Model;
using Microsoft.Extensions.Caching.Memory;
using Refit;

namespace Application.Handlers
{
    internal class GetIpQueryHandler : RequestHandler<GetIpQuery, IpData>
    {
        private readonly IIpLookupApi _ipLookupApi;
        // simple local cache for inexpensive data requests. More complex data would have this replaced with a distributed cache (something like Redis)
        private readonly IMemoryCache _cache;
        private readonly IIpDataRepository _repository;

        public GetIpQueryHandler(IIpLookupApi ipLookupApi, IMemoryCache cache, IIpDataRepository repository)
        {
            _ipLookupApi = ipLookupApi;
            _cache = cache;
            _repository = repository;
        }

        protected async override Task<HandlerResponse<IpData>> Request(GetIpQuery request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue<IpData>(request.IpAddress, out var ipData))
            {
                var getDataResponse = await GetData(request);
                if (getDataResponse.Status != HandlerStatus.Success)
                {
                    return getDataResponse;
                }

                ipData = getDataResponse.Result;

                UpdateCache(ipData);
            }

            return Success(ipData!);
        }

        private async Task<HandlerResponse<IpData>> GetData(GetIpQuery request)
        {
            var ipData = await _repository.GetIpData(request.IpAddress);

            if (ipData == null)
            {
                var apiResponse = await GetIpDataFromApi(request);
                if (!apiResponse.IsSuccessStatusCode)
                {
                    string? content = apiResponse.Error.Content;
                    return Failure(HandlerStatus.Error, null!, content ?? string.Empty);
                }
                if (apiResponse.Content.Latitude == 0 && apiResponse.Content.Longitude == 0 && apiResponse.Content.IsEu == null)
                {
                    return Failure(HandlerStatus.NotFound, null!, Constants.NotFound(request.IpAddress));
                }
                ipData = apiResponse.Content;
                await _repository.AddIpData(ipData);
            }

            return Success(ipData);
        }

        private async Task<ApiResponse<IpData>> GetIpDataFromApi(GetIpQuery request)
        {
            return await _ipLookupApi.GetDataForIp(request.IpAddress);
        }

        private void UpdateCache(IpData data)
        {
            _cache.Set(data.Ip, data);
        }
    }
}
