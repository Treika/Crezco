using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class IpEntity
    {
        [Key]
        public string Ip { get; set; } = null!;
        public string Data { get; set; } = null!;
    }
}