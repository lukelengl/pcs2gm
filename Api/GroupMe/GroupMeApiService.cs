using System.Text;
using System.Text.Json;
using pcs2gm.Api.GroupMe.Models;
using pcs2gm.Models;
using psc2gm.Api.GroupMe.Models;

namespace pcs2gm.Api.GroupMe;

public class GroupMeApiService
{
    private const string BaseUrl = "https://api.groupme.com/v3/";

    private HttpClient _httpClient;
    private string _token;

    public GroupMeApiService(Config config)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BaseUrl);

        _token = config.GroupMeAccessToken;
    }

    public async Task TestAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"users/me?token={_token}");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<string> CreateGroupAsync(string name)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"groups?token={_token}");
        request.Content = new StringContent(JsonSerializer.Serialize(new CreateGroup { Name = name }), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var createGroupResponse = JsonSerializer.Deserialize<CreateGroupResponse>(responseString);
        return createGroupResponse!.Response.Id;
    }

    public async Task AddMembersToGroupAsync(string groupId, List<AddGroupMember> members) {
        var request = new HttpRequestMessage(HttpMethod.Post, $"groups/{groupId}/members/add?token={_token}");
        request.Content = new StringContent(JsonSerializer.Serialize(new AddGroupMembersPayload { Members = members }), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}