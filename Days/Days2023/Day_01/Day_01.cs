using Shared;

namespace Days2023;

public class Day_01 : Day
{
    public List<string> input = new ();

    public Day_01()
    {
        Title = "Trebuchet?!";
        DayNumber = 1;
        Year = 2023;
    }
    public override void Gather_input()
    {
        input = Input.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
    }

    public override string HandlePart1()
    {
        var sum = input.Select(x => x.Where(Char.IsDigit).ToArray())
            .Select(x => new string(new char[] { x.FirstOrDefault(), x.LastOrDefault() }))
            .Sum(Convert.ToInt32);
        return sum.ToString();
    }

    public override string HandlePart2()
    {
        var sum = input.Select(x =>
            x.Replace("one", "one1one")
                .Replace("two", "two2two")
                .Replace("three", "three3three")
                .Replace("four", "four4four")
                .Replace("five", "five5five")
                .Replace("six", "six6six")
                .Replace("seven", "seven7seven7")
                .Replace("eight", "eight8eight")
                .Replace("nine", "nine9nine"))
            .Select(x => x.Where(char.IsDigit).ToArray())
            .Select(x => new string(new[] { x.FirstOrDefault(), x.LastOrDefault() }))
            .Sum(Convert.ToInt32);
        return sum.ToString();
    }
}