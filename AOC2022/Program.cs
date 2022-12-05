// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Shared.Interfacing;

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json")
    .Build();

var client = new AoCClient(configuration["AoCClientConfig:BaseUrl"], configuration["AoCClientConfig:Session"]);
var inputService = new InputService(client);
var answerService = new AnswerService(client);

while (true)
{
    // Console.WriteLine("Which year do you want?");
    // var years = State.years.Where(x => x.Days.Any()).Select(x => x.YearNumber.ToString());
    // WriteHelper.PrintInfos(years.ToList());
    // var yearInput = Console.ReadLine();
    Console.WriteLine("Which Day do you want ?");
    var infos = State.years.Single().Days.Where(x => x.Title != null).Select(x => x.Info);
    WriteHelper.PrintInfos(infos.ToList());
    var input = Console.ReadLine();
    var dayWrapper = new DayWrapper(inputService, answerService);
    if (int.TryParse(input, out var chosenDay) && State.years.Single().Days.SingleOrDefault(x => x.DayNumber == chosenDay) != null)
    {
        var day = State.years.Single().Days.Single(x => x.DayNumber == chosenDay);
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
                new Day_01(),
            }
        }
    };
}

