using System.Text.Json.Serialization;

namespace psc2gm.Api.GroupMe.Models
{
    public class CreateGroup
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class CreateGroupResponse
    {
        [JsonPropertyName("response")]
        public CreateGroupResponseData Response { get; set; }
    }

    public class CreateGroupResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}