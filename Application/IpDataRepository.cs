using Application.Interfaces;
using Client.Abstractions.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application
{
    public class IpDataRepository : IIpDataRepository
    {
        private readonly IpContext _context;

        public IpDataRepository(IpContext context)
        {
            _context = context;
        }

        public async Task<IpData?> GetIpData(string ipAddress)
        {
            _context.Database.EnsureCreated();

            var entity = await _context.IpAddresses.FirstOrDefaultAsync(x => x.Ip == ipAddress);

            return entity?.Data != null ? JsonSerializer.Deserialize<IpData>(entity.Data) : null;
        }

        public async Task AddIpData(IpData data)
        {
            var entity = new IpEntity { Ip = data.Ip, Data = JsonSerializer.Serialize(data) };

            await _context.IpAddresses.AddAsync(entity);

            await _context.SaveChangesAsync();

        }
    }
}
