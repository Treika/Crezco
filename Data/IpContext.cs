using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class IpContext : DbContext
    {
        public IpContext(DbContextOptions<IpContext> options) : base(options) { }
        public DbSet<IpEntity> IpAddresses { get; set; }
    }
}
