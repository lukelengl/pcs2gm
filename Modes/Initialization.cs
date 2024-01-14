using pcs2gm.Models;
using Spectre.Console;

namespace pcs2gm.Modes;

public class Initialization
{
    public static async Task<Config> ValidateAndGetConfigAsync()
    {
        if (!Config.Exists())
        {
            Setup.Run();
        }
        var config = Config.Load();

        bool validConfig = true;
        await AnsiConsole.Status().StartAsync("Validating Planning Center configuration...", async ctx =>
        {
            try
            {
                await config.ValidatePlanningCenterAsync();
            }
            catch (Exception)
            {
                validConfig = false;
                AnsiConsole.MarkupLine("[red]Planning Center Configuration is invalid![/]");
            }
        });

        await AnsiConsole.Status().StartAsync("Validating GroupMe configuration...", async ctx =>
        {
            try
            {
                await config.ValidateGroupMeAsync();
            }
            catch (Exception)
            {
                validConfig = false;
                AnsiConsole.MarkupLine("[red]GroupMe Configuration is invalid![/]");
            }
        });

        if (!validConfig)
        {
            Setup.Run();
            config = Config.Load();
        }

        return config;
    }
}