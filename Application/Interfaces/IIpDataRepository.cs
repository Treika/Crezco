using Client.Abstractions.Models;

namespace Application.Interfaces
{
    public interface IIpDataRepository
    {
        Task<IpData?> GetIpData(string ipAddress);
        Task AddIpData(IpData data);
    }
}
