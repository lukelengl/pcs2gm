using pcs2gm.Api.PlanningCenter;
using pcs2gm.Api.GroupMe;
using pcs2gm.Models;
using psc2gm.Api.PlanningCenter.Models;
using Spectre.Console;
using pcs2gm.Api.GroupMe.Models;

namespace pcs2gm.Modes;

public class CreateANewGroup {

    public class GroupPerson
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
    
    public static async Task RunAsync()
    {
        var planningCenterApiService = new PlanningCenterApiService(Config.Load());

        Dictionary<string, string> serviceTypesDictionary = new Dictionary<string, string>();
        await AnsiConsole.Status().StartAsync("Loading service types...", async ctx =>
        {
            var serviceTypes = await planningCenterApiService.GetServiceTypesAsync();
            ctx.Status = "Service types loaded!";

            serviceTypesDictionary = serviceTypes.ToDictionary(x => x.Attributes.Name, x => x.Id);
        });

        var serviceTypeSelection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Which type of service?")
            .PageSize(10)
            .AddChoices(serviceTypesDictionary.Keys)
        );

        var serviceTypeId = serviceTypesDictionary[serviceTypeSelection];

        Dictionary<string, string> planDictionary = new Dictionary<string, string>();
        await AnsiConsole.Status().StartAsync("Loading service plans...", async ctx =>
        {
            var plans = await planningCenterApiService.GetPlansAsync(serviceTypeId);
            ctx.Status = "Service plans loaded!";
            planDictionary = plans.ToDictionary(x => x.Attributes.SortDate.ToString("D"), x => x.Id);
        });

        var planSelection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Which service plan?")
            .PageSize(10)
            .AddChoices(planDictionary.Keys)
        );

        var planId = planDictionary[planSelection];

        var teamsDictionary = new Dictionary<string, string>();
        await AnsiConsole.Status().StartAsync("Loading teams...", async ctx =>
        {
            var planPersons = await planningCenterApiService.GetTeamsAsync(serviceTypeId);
            ctx.Status = "Teams loaded!";
            teamsDictionary = planPersons.ToDictionary(x => x.Attributes.Name, x => x.Id);
        });

        var teamsSelection = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Which teams?")
            .AddChoices(teamsDictionary.Keys));

        var teamIds = teamsSelection.Select(x => teamsDictionary[x]).ToList();

        List<PlanPerson> planPersons = new List<PlanPerson>();
        await AnsiConsole.Status().StartAsync("Loading team members...", async ctx =>
        {
            var persons = await planningCenterApiService.GetPlanPersonsAsync(serviceTypeId, planId);
            ctx.Status = "Team members loaded!";
            planPersons.AddRange(persons.Where(x => teamIds.Contains(x.Relationships.Team.Data.Id)));
        });

        var declinedPlanPersons = planPersons.Where(x => x.Attributes.Status == psc2gm.Api.PlanningCenter.Models.Status.Declined).ToList();

        planPersons = planPersons.Except(declinedPlanPersons).DistinctBy(x => x.Relationships.Person.Data.Id).ToList();

        var personsWithoutPhoneNumbers = new List<PlanPerson>();

        var groupPersons = new List<GroupPerson>();
        foreach(var planPerson in planPersons)
        {
            var phoneNumbers = (await planningCenterApiService.GetPhoneNumbersAsync(planPerson.Relationships.Person.Data.Id)).Select(x => x.Attributes.E164);
            foreach(var phoneNumber in phoneNumbers)
            {
                groupPersons.Add(new GroupPerson { Name = planPerson.Attributes.Name, PhoneNumber = phoneNumber });
            }
            if (!phoneNumbers.Any()) {
                personsWithoutPhoneNumbers.Add(planPerson);
            }
        }

        var groupName = AnsiConsole.Prompt(new TextPrompt<string>("What do you want to name the [blue]GroupMe[/] group?"));

        AnsiConsole.MarkupLine("The group will be created with the following members:");
        foreach(var groupPerson in groupPersons)
        {
            AnsiConsole.MarkupLine($"[blue]{groupPerson.Name}[/] - [green]{groupPerson.PhoneNumber}[/]");
        }
        AnsiConsole.WriteLine(string.Empty);

        AnsiConsole.MarkupLine("The following people have declined the plan and will be excluded:");
        foreach(var declinedPlanPerson in declinedPlanPersons)
        {
            AnsiConsole.MarkupLine($"[red]{declinedPlanPerson.Attributes.Name}[/]");
        }
        AnsiConsole.WriteLine(string.Empty);

        AnsiConsole.MarkupLine("The following people do not have a phone number and will be excluded:");
        foreach(var personWithoutPhoneNumber in personsWithoutPhoneNumbers)
        {
            AnsiConsole.MarkupLine($"[red]{personWithoutPhoneNumber.Attributes.Name}[/]");
        }

        if (!AnsiConsole.Confirm("Do you want to proceed?", defaultValue: false))
        {
            AnsiConsole.MarkupLine("Cancelled!");
            AnsiConsole.MarkupLine("Press any key to start over...");
            Console.ReadKey();
            return;
        }

        var groupMeApiService = new GroupMeApiService(Config.Load());
        var groupId = await groupMeApiService.CreateGroupAsync(groupName);
        var addGroupMembers = groupPersons.Select(x => new AddGroupMember { Nickname = x.Name, PhoneNumber = x.PhoneNumber }).ToList();
        await groupMeApiService.AddMembersToGroupAsync(groupId, addGroupMembers);

        AnsiConsole.MarkupLine($"[green]GroupMe group \"{groupName}\" created![/]");
        AnsiConsole.MarkupLine("Press any key to exit...");
        Console.ReadKey();
    }   
}