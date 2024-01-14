using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace psc2gm.Api.PlanningCenter.Models;

public class GetPlanPersonsResponse
{
    [JsonPropertyName("data")]
    public List<PlanPerson> Data { get; set; }
}

public class PlanPerson
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("attributes")]
    public PlanPersonAttributes Attributes { get; set; }

    [JsonPropertyName("relationships")]
    public Relationships Relationships { get; set; }
}

public class PlanPersonAttributes
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}

public static class Status
{
    public const string Declined = "D";
}