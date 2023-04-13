using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction
{
    public interface IIpLookupApi
    {
        [Get("/ip_to_location/{ipAddress}")]
        Task<ApiResponse<IpData>> GetDataForIp(string ipAddress);
    }
}