using pcs2gm.Models;
using Spectre.Console;

namespace pcs2gm.Modes;

public class MainMenu 
{
    const string CreateANewGroup = "Create a new group";
    const string Setup = "Setup";
    const string Quit = "Quit";
    
    public static async Task RunAsync()
    {
        var mode = string.Empty;

        while (mode != Quit)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("pcs2gm").Color(Color.Green));

            mode = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .PageSize(10)
                    .AddChoices(
                        CreateANewGroup,
                        Setup,
                        Quit)
            );

            switch (mode)
            {
                case CreateANewGroup:
                    await Modes.CreateANewGroup.RunAsync();
                    break;
                case Setup:
                    Modes.Setup.Run();
                    break;
            }
        }
        AnsiConsole.Clear();
    }
}