
using System.Text.Json.Serialization;

namespace psc2gm.Api.PlanningCenter.Models;

public class GetPlansResponse
{
    [JsonPropertyName("data")]
    public List<Plan> Data { get; set; }
}

public class Plan
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("attributes")]
    public PlanAttributes Attributes { get; set; }
}

public class PlanAttributes
{
    [JsonPropertyName("sort_date")]
    public DateTime SortDate { get; set; }
}