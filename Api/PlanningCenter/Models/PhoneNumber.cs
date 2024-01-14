using System.Text.Json.Serialization;

namespace psc2gm.Api.PlanningCenter.Models;

public class GetPhoneNumbersResponse
{
    [JsonPropertyName("data")]
    public List<PhoneNumber> Data { get; set; }
}

public class PhoneNumber
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("attributes")]
    public PhoneNumberAttributes Attributes { get; set; }
}

public class PhoneNumberAttributes
{
    [JsonPropertyName("e164")]
    public string E164 { get; set; }
}