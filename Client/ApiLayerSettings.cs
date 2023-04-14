using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Client
{
    public record ApiLayerSettings
    {
        [Required]
        [Url]
        public string BaseAddress { get; set; } = null!;
        [Required]
        public string ApiKey { get; set; } = null!;

        public ApiLayerSettings(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(this);
        }
    }
}
