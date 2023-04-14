using System.Text.Json.Serialization;

namespace Client.Abstractions.Models
{
    public record IpData
    {
        [JsonPropertyName("city")]
        public string City { get; set; } = null!;

        [JsonPropertyName("connection")]
        public Connection Connection { get; set; } = null!;

        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; } = null!;

        [JsonPropertyName("continent_name")]
        public string ContinentName { get; set; } = null!;

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = null!;

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; } = null!;

        [JsonPropertyName("currencies")]
        public List<Currency> Currencies { get; set; } = null!;

        [JsonPropertyName("ip")]
        public string Ip { get; set; } = null!;

        [JsonPropertyName("is_eu")]
        public bool? IsEu { get; set; } = null!;

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; } = null!;

        [JsonPropertyName("location")]
        public Location Location { get; set; } = null!;

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; } = null!;

        [JsonPropertyName("region_name")]
        public string RegionName { get; set; } = null!;

        [JsonPropertyName("timezones")]
        public List<string> Timezones { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }
}

