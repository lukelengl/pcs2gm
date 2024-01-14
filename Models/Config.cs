using pcs2gm.Api.GroupMe;
using pcs2gm.Api.PlanningCenter;
using Tomlyn;

namespace pcs2gm.Models;

public class Config 
{
    public const string FileName = "config.toml";
    public string PlanningCenterApplicationId { get; set; } = string.Empty;
    public string PlanningCenterSecret { get; set; } = string.Empty;
    public string GroupMeAccessToken { get; set; } = string.Empty;

    public async Task ValidatePlanningCenterAsync()
    {
        var planningCenterApiService = new PlanningCenterApiService(this);
        await planningCenterApiService.GetServiceTypesAsync();
    }

    public async Task ValidateGroupMeAsync()
    {
        var groupMeApiService = new GroupMeApiService(this);
        await groupMeApiService.TestAsync();
    }

    public static bool Exists()
    {
        return File.Exists(FileName);
    }

    public static Config Load()
    {
        var fileContents = File.ReadAllText(FileName);
        return Toml.ToModel<Config>(fileContents);
    }

    public static void Save(Config config)
    {
        File.WriteAllText(FileName, Toml.FromModel(config));
    }
}