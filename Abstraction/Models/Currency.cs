using System.Text.Json.Serialization;

namespace Client.Abstractions.Models
{
    public record Currency
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = null!;
    }
}
