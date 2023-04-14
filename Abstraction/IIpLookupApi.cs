using Client.Abstractions.Models;
using Refit;

namespace Client.Abstraction
{
    public interface IIpLookupApi
    {
        [Get("/ip_to_location/{ipAddress}")]
        Task<ApiResponse<IpData>> GetDataForIp(string ipAddress);
    }
}