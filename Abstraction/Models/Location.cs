using System.Text.Json.Serialization;

namespace Client.Abstractions.Models
{
    public record Location
    {
        [JsonPropertyName("calling_codes")]
        public List<string> CallingCodes { get; set; } = null!;

        [JsonPropertyName("capital")]
        public string Capital { get; set; } = null!;

        [JsonPropertyName("flag")]
        public string Flag { get; set; } = null!;

        [JsonPropertyName("native_name")]
        public string NativeName { get; set; } = null!;

        [JsonPropertyName("top_level_domains")]
        public List<string> TopLevelDomains { get; set; } = null!;
    }
}
