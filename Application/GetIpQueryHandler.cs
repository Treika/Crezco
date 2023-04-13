using Abstraction;
using Cqrs.Handlers;
using Cqrs.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Application
{
    internal class GetIpQueryHandler : RequestHandler<GetIpQuery, IpData>
    {
        private readonly IIpLookupApi _ipLookupApi;
        private readonly IMemoryCache _cache;

        public GetIpQueryHandler(IIpLookupApi ipLookupApi, IMemoryCache cache)
        {
            _ipLookupApi = ipLookupApi;
            _cache = cache;
        }

        protected async override Task<HandlerResponse<IpData>> Request(GetIpQuery request, CancellationToken cancellationToken)
        {
            if(!_cache.TryGetValue<IpData>(request.ipAddress, out var ipData))
            {
                return await GetIpData(request);
            }

            return Success(ipData);


        }

        private async Task<HandlerResponse<IpData>> GetIpData(GetIpQuery request)
        {
            var response = await _ipLookupApi.GetDataForIp(request.ipAddress);
            if (!response.IsSuccessStatusCode)
            {
                return Failure(HandlerStatus.Error, null!, response?.Error?.Content!);
            }

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));
            _cache.Set(request.ipAddress, response.Content, memoryCacheEntryOptions);

            return Success(response.Content);
        }
    }
}
