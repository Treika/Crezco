using System.Text.Json.Serialization;

public class Connection
{
    [JsonPropertyName("asn")]
    public int? Asn { get; set; }

    [JsonPropertyName("isp")]
    public string Isp { get; set; }
}

public class Currency
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
}

public class Location
{
    [JsonPropertyName("calling_codes")]
    public List<string> CallingCodes { get; set; }

    [JsonPropertyName("capital")]
    public string Capital { get; set; }

    [JsonPropertyName("flag")]
    public string Flag { get; set; }

    [JsonPropertyName("native_name")]
    public string NativeName { get; set; }

    [JsonPropertyName("top_level_domains")]
    public List<string> TopLevelDomains { get; set; }
}

public class IpData
{
    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("connection")]
    public Connection Connection { get; set; }

    [JsonPropertyName("continent_code")]
    public string ContinentCode { get; set; }

    [JsonPropertyName("continent_name")]
    public string ContinentName { get; set; }

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; }

    [JsonPropertyName("country_name")]
    public string CountryName { get; set; }

    [JsonPropertyName("currencies")]
    public List<Currency> Currencies { get; set; }

    [JsonPropertyName("ip")]
    public string Ip { get; set; }

    [JsonPropertyName("is_eu")]
    public bool? IsEu { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    [JsonPropertyName("region_name")]
    public string RegionName { get; set; }

    [JsonPropertyName("timezones")]
    public List<string> Timezones { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

