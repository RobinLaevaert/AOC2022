// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Shared.Interfacing;

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .Build();

var client = new AoCClient(configuration["AoCClientConfig:BaseUrl"], configuration["AoCClientConfig:Session"]);
var inputService = new InputService(client);
var answerService = new AnswerService(client);

while (true)
{
    Console.WriteLine("Which year do you want?");
    var years = State.years.Where(x => x.Days.Any()).Select(x => x.YearNumber.ToString());
    WriteHelper.PrintInfos(years.ToList());
    var yearInput = Console.ReadLine();
    Console.WriteLine("Which Day do you want ?");
    var year = State.years.Single(x => x.YearNumber.ToString() == yearInput);
    var infos = year.Days.Where(x => !string.IsNullOrEmpty(x.Title)).Select(x => x.Info);
    WriteHelper.PrintInfos(infos.ToList());
    var input = Console.ReadLine();
    var dayWrapper = new DayWrapper(inputService, answerService);
    if (int.TryParse(input, out var chosenDay) && year.Days.SingleOrDefault(x => x.DayNumber == chosenDay) != null)
    {
        var day = year.Days.Single(x => x.DayNumber == chosenDay);
        dayWrapper.Day = day;
        await dayWrapper.HandleSelect();
        dayWrapper.Deselect();
    }
    else
    {
        Console.WriteLine("Day not found, Press Key to go back to main menu");
        Console.ReadKey();
        Console.Clear();
    }
}

public static class State
{
    public static readonly List<Year> years = new()
    {
        new Year()
        {
            YearNumber = 2022,
            Days = new()
            {
                new Days2022.Day_01(),
                new Days2022.Day_02(),
                new Days2022.Day_03(),
                new Days2022.Day_04(),
                new Days2022.Day_05(),
                new Days2022.Day_06(),
                new Days2022.Day_07(),
                new Days2022.Day_08(),
                new Days2022.Day_09(),
                new Days2022.Day_10(),
                new Days2022.Day_11(),
                new Days2022.Day_12(),
                new Days2022.Day_13(),
                new Days2022.Day_14(),
                new Days2022.Day_15(),
                new Days2022.Day_16(),
                new Days2022.Day_17(),
                new Days2022.Day_18(),
                new Days2022.Day_19(),
                new Days2022.Day_20(),
                new Days2022.Day_21(),
                new Days2022.Day_22(),
                new Days2022.Day_23(),
                new Days2022.Day_24(),
                new Days2022.Day_25(),
            }
        },
        new Year()
        {
            YearNumber = 2023,
            Days = new()
            {
                new Days2023.Day_01(),
                new Days2023.Day_02(),
                new Days2023.Day_03(),
                new Days2023.Day_04(),
                new Days2023.Day_05(),
                new Days2023.Day_06(),
                new Days2023.Day_07(),
                new Days2023.Day_08(),
                new Days2023.Day_09(),
                new Days2023.Day_10(),
                new Days2023.Day_11(),
                new Days2023.Day_12(),
                new Days2023.Day_13(),
                new Days2023.Day_14(),
                new Days2023.Day_15(),
                new Days2023.Day_16(),
                new Days2023.Day_17(),
                new Days2023.Day_18(),
                new Days2023.Day_19(),
                new Days2023.Day_20(),
                new Days2023.Day_21(),
                new Days2023.Day_22(),
                new Days2023.Day_23(),
                new Days2023.Day_24(),
                new Days2023.Day_25(),
            }
        },
        new Year()
        {
            YearNumber = 2024,
            Days = new()
            {
                new Days2024.Day_01(),
                new Days2024.Day_02(),
                new Days2024.Day_03(),
                new Days2024.Day_04(),
                new Days2024.Day_05(),
                new Days2024.Day_06(),
            }
        }
    };
}

