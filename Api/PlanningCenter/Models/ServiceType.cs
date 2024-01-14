using System.Text.Json.Serialization;

namespace psc2gm.Api.PlanningCenter.Models;

public class GetServiceTypesResponse
{
    [JsonPropertyName("data")]
    public List<ServiceType> Data { get; set; }

}

public class ServiceType
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("attributes")]
    public ServiceTypeAttributes Attributes { get; set; }
}

public class ServiceTypeAttributes
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}