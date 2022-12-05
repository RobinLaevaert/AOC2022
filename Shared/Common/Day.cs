namespace Shared;

public abstract class Day
{
    public string Title = string.Empty;
    public int DayNumber;
    public int Year;
    public string Info => $"{DayNumber}. {Title}";
    

    private IEnumerable<string> _challengeInput;
    private IEnumerable<string> TestInput => File.ReadAllLines($"{Environment.CurrentDirectory}/{this.GetType().Name}/TestInput.txt");

    public IEnumerable<string> Input;

    protected Day() { }

    public abstract void Gather_input();
    public void Part1() => Console.WriteLine(HandlePart1());
    public void Part2() => Console.WriteLine(HandlePart2());

    public abstract string HandlePart1();

    public abstract string HandlePart2();

    public void SetInputString(string inputString)
    {
        _challengeInput = inputString.Split('\n');
    }

    public void UseTestInput() => Input = TestInput;
    public void UseChallengeInput() => Input = _challengeInput;


}