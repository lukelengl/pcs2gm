using System.Text.Json.Serialization;

namespace psc2gm.Api.PlanningCenter.Models;

public class GetTeamsResponse
{
    [JsonPropertyName("data")]
    public List<Team> Data { get; set; }
}

public class Team
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("attributes")]
    public TeamAttributes Attributes { get; set; }
}

public class TeamAttributes
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}