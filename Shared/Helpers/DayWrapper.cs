using System.Data;
using System.Diagnostics;
using System.Reflection;
using Shared.Interfacing;

namespace Shared;

public class DayWrapper
{
    public Day Day;
    private readonly IInputService _inputService;
    private readonly IAnswerService _answerService;

    public DayWrapper(IInputService inputService, IAnswerService answerService)
    {
        _inputService = inputService;
        _answerService = answerService;
    }
    
    public async Task HandleSelect()
    {
        Console.Clear();
        Console.WriteLine(Day.Info);
        Console.WriteLine();
        Console.WriteLine("Do you want to solve Part 1 or 2?");
        Day.SetInputString(await _inputService.GetInputOfDayAsync(Day.Year, Day.DayNumber));
        string result = "";
        switch (Console.ReadLine())
        {
            case "1":
                Day.Gather_input();
                await CheckAnswer(Day.HandlePart1(), Day.Year, Day.DayNumber, 1);
                break;
            case "2":
                Day.Gather_input();
                await CheckAnswer(Day.HandlePart1(), Day.Year, Day.DayNumber, 2);
                break;
            case "1t":
                // Test input;
                break;
            case "2t":
                // Test input;
                break;
            case "1p":
                Performance_logging(Day.Gather_input, Day.Part1);
                break;
            case "2p":
                Performance_logging(Day.Gather_input, Day.Part2);
                break;
            case "3p":
                Performance_logging(Day.Gather_input, Day.Part1, Day.Part2);
                break;
            default:
                Console.WriteLine($"Not implemented");
                await HandleSelect();
                break;
        }
    }
    
    private async Task CheckAnswer(string answer, int year, int day, int partNumber)
    {
        Console.WriteLine(await _answerService.PostAnswerAsync(year, day, partNumber, answer) switch
        {
            AnswerStatus.Correct => $"Answer: {answer} | CORRECT",
            AnswerStatus.Wrong => $"Answer: {answer} | INCORRECT",
            AnswerStatus.TooSoon => $"Spamming too much. HOLD YOUR HORSES",
            AnswerStatus.DayAlreadyCompleted => ":/",
            AnswerStatus.Unknown => "DEES KLOPT NIETE",
            _ => throw new ArgumentOutOfRangeException()
        });
    }
    private static void Performance_logging(params Action[] actions)
    {
        Stopwatch stopwatch = new();
        Dictionary<string, double> timings_per_action = new();
        Console.WriteLine("Results:");
        foreach (var action in actions)
        {
            var action_name = action.GetMethodInfo().Name;
            if (action_name != "Gather_input")
                Console.WriteLine(action_name);
            stopwatch.Reset();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            timings_per_action.Add(action.GetMethodInfo().Name, (double)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond);
            if (action_name != "Gather_input")
                Console.WriteLine();
        }
        stopwatch.Reset();
        Console.WriteLine("Performance metrics");
        foreach (var timing in timings_per_action)
        {
            Console.WriteLine($"{timing.Key}: {timing.Value} ms");
        }
    }
    
    public void Deselect()
    {
        Console.WriteLine("Press Key to go back to main menu");
        Console.ReadKey();
        Console.Clear();
    }
}