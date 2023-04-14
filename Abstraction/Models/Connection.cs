using System.Text.Json.Serialization;

namespace Client.Abstractions.Models
{
    public record Connection
    {
        [JsonPropertyName("asn")]
        public int? Asn { get; set; } = null!;

        [JsonPropertyName("isp")]
        public string Isp { get; set; } = null!;
    }
}
