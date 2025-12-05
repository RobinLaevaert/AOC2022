using System.Text.Json;

namespace Days2025;

public class Day_01 : Day
{
    public List<Instruction> Instructions = [];
    public Day_01()
    {
        Title = "Secret Entrance";
        DayNumber = 1;
        Year = 2025;
    }

    public override void Gather_input()
    {
        Instructions = Input.Where(x => !string.IsNullOrEmpty(x))
            .Select(x => new Instruction(Enum.Parse<Direction>(x[..1]), int.Parse(x[1..]))).ToList();
    }

    public override string HandlePart1()
    {
        var dials = Instructions.Aggregate(new List<int>() { 50 },
            (acc, instruction) =>
            [
                ..acc, acc.Last() + ((instruction.Direction == Direction.L ? -1 : 1) * instruction.Value)
            ]);

        var timesZero = dials.Count(x => x%100 == 0);
        return timesZero.ToString();
    }

    public override string HandlePart2()
    {
        var dials = Instructions.Aggregate((50, 0),
            (acc, instruction) =>
            {
                var previous = acc.Item1;
                var newResult = (previous + ((instruction.Direction == Direction.L ? -1 : 1) * instruction.Value));
                var timesPassedZero = Enumerable.Range(0, instruction.Value)
                    .Select((x, index) => previous + (index * (instruction.Direction == Direction.L ? -1 : 1)))
                    .Count(x => x % 100 == 0);
                return (newResult, acc.Item2 + timesPassedZero);
            }
        );

        var timesZero = dials.Item2;
        return timesZero.ToString();
    }
}

public record struct Instruction(Direction Direction, int Value);

public enum Direction
{
    L,
    R
}