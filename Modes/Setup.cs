using pcs2gm.Models;
using Spectre.Console;

namespace pcs2gm.Modes;

public class Setup
{
    public static void Run()
    {
        var config = new Config();
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("pcs2gm").Color(Color.Green));
        AnsiConsole.MarkupLine("Let's setup your access to [green]Planning Center[/] and [blue]GroupMe[/].");

        GetAndTestPlanningCenterCredentials(config);
        GetAndTestGroupMeCredentials(config);
        Config.Save(config);
    }

    private static void GetAndTestPlanningCenterCredentials(Config config)
    {
        config.PlanningCenterApplicationId =
            AnsiConsole.Prompt<string>(new TextPrompt<string>("What's your [green]Planning Center[/] Application ID?"));

        config.PlanningCenterSecret =
            AnsiConsole.Prompt<string>(new TextPrompt<string>("What's your [green]Planning Center[/] Secret?"));
    }

    private static void GetAndTestGroupMeCredentials(Config config)
    {
        config.GroupMeAccessToken =
            AnsiConsole.Prompt<string>(new TextPrompt<string>("What's your [blue]GroupMe[/] Access Token?"));
    }
}