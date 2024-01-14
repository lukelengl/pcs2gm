using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using pcs2gm.Models;
using psc2gm.Api.PlanningCenter.Models;

namespace pcs2gm.Api.PlanningCenter;

public class PlanningCenterApiService
{
    private const string BaseUrl = "https://api.planningcenteronline.com";
    private const string ServicesApiVersion = "2018-11-01";
    private const string PeopleApiVersion = "2022-07-14";

    private HttpClient _httpClient;

    public PlanningCenterApiService(Config config)
    {
        _httpClient = new HttpClient();

        Encoding encoding = Encoding.UTF8;
        string credential = String.Format("{0}:{1}", config.PlanningCenterApplicationId, config.PlanningCenterSecret);
        var credentialsEncoded = Convert.ToBase64String(encoding.GetBytes(credential));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentialsEncoded);
    }

    public async Task<List<ServiceType>> GetServiceTypesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/services/v2/service_types");
        request.Headers.Add("X-PCO-API-Version", ServicesApiVersion);
        var responseString = await GetResponseStringAsync(request);
        var getServiceTypesResponse = JsonSerializer.Deserialize<GetServiceTypesResponse>(responseString);
        return getServiceTypesResponse!.Data;
    }

    public async Task<List<Plan>> GetPlansAsync(string serviceTypeId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/services/v2/service_types/{serviceTypeId}/plans?per_page=100&filter=after&after={DateTime.Now.ToString("yyyy-MM-dd")}");
        request.Headers.Add("X-PCO-API-Version", ServicesApiVersion);
        var responseString = await GetResponseStringAsync(request);
        var getPlansResponse = JsonSerializer.Deserialize<GetPlansResponse>(responseString);
        return getPlansResponse!.Data;
    }

    public async Task<List<PlanPerson>> GetPlanPersonsAsync(string serviceTypeId, string planId)
    {
        var offset = 0;
        var persons = await GetPlanPersonsPageAsync(serviceTypeId, planId, offset++);
        while (persons.Count == 100)
        {
            persons.AddRange(await GetPlanPersonsPageAsync(serviceTypeId, planId, offset++));
        }
        return persons;
    }

    private async Task<List<PlanPerson>> GetPlanPersonsPageAsync(string serviceTypeId, string planId, int offset)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/services/v2/service_types/{serviceTypeId}/plans/{planId}/team_members?per_page=100&offset={offset}");
        request.Headers.Add("X-PCO-API-Version", ServicesApiVersion);
        var responseString = await GetResponseStringAsync(request);
        var getPlanPersonsResponse = JsonSerializer.Deserialize<GetPlanPersonsResponse>(responseString);
        return getPlanPersonsResponse!.Data;
    }

    public async Task<List<Team>> GetTeamsAsync(string serviceTypeId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/services/v2/service_types/{serviceTypeId}/teams?per_page=100");
        request.Headers.Add("X-PCO-API-Version", ServicesApiVersion);
        var responseString = await GetResponseStringAsync(request);
        var getTeamsResponse = JsonSerializer.Deserialize<GetTeamsResponse>(responseString);
        return getTeamsResponse!.Data;
    }

    public async Task<List<PhoneNumber>> GetPhoneNumbersAsync(string personId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/people/v2/people/{personId}/phone_numbers");
        request.Headers.Add("X-PCO-API-Version", PeopleApiVersion);
        var responseString = await GetResponseStringAsync(request);
        var getPhoneNumbersResponse = JsonSerializer.Deserialize<GetPhoneNumbersResponse>(responseString);
        return getPhoneNumbersResponse!.Data.DistinctBy(x => x.Attributes.E164).ToList();
    }

    private async Task<string> GetResponseStringAsync(HttpRequestMessage request)
    {
        var response = await _httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) {
            var retryAfter = response.Headers.RetryAfter;
            if (retryAfter != null) {
                var delay = retryAfter.Delta.Value;
                await Task.Delay(delay);
                return await GetResponseStringAsync(request);
            }
        }
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

}