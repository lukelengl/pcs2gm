using System.Text.Json.Serialization;

namespace pcs2gm.Api.GroupMe.Models;

public class AddGroupMembersPayload
{
    [JsonPropertyName("members")]
    public List<AddGroupMember> Members { get; set; }

}

public class AddGroupMember 
{
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }
}