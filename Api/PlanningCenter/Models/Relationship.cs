using System.Text.Json.Serialization;

namespace psc2gm.Api.PlanningCenter.Models;

public class Relationships
{
    [JsonPropertyName("team")]
    public Relationship Team { get; set; }

    [JsonPropertyName("person")]
    public Relationship Person { get; set; }
}

public class Relationship
{
    [JsonPropertyName("data")]
    public RelationshipData Data { get; set; }
}

public class RelationshipData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}