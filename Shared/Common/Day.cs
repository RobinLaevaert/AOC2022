namespace Shared;

public abstract class Day
{
    public string Title = string.Empty;
    public int DayNumber;
    public int Year;
    public string Info => $"{DayNumber}. {Title}";

    private string _inputString = string.Empty;

    public IEnumerable<string> Input => _inputString.Split("\n");

    protected Day() { }

    public abstract void Gather_input();
    public void Part1() => Console.WriteLine(HandlePart1());
    public void Part2() => Console.WriteLine(HandlePart2());

    public abstract string HandlePart1();

    public abstract string HandlePart2();

    public void SetInputString(string inputString)
    {
        _inputString = inputString;
    }

   
}