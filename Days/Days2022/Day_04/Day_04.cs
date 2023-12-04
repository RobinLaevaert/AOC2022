namespace Days2022;

public class Day_04 : Day
{
    public List<CleaningPair> CleaningPairs;
    public Day_04()
    {
        Title = "Camp Cleanup";
        DayNumber = 4;
        Year = 2022;
    }
    public override void Gather_input()
    {
        CleaningPairs = Input.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new CleaningPair(x)).ToList();
    }

    public override string HandlePart1()
    {
        var completeOverlaps = CleaningPairs.Where(x =>
            (x.Elf2.Item1 <= x.Elf1.Item1 && x.Elf1.Item2 <= x.Elf2.Item2) ||
            (x.Elf1.Item1 <= x.Elf2.Item1 && x.Elf2.Item2 <= x.Elf1.Item2));
        return completeOverlaps.Count().ToString();
    }

    public override string HandlePart2()
    {
        var overlaps = CleaningPairs.Where(x =>
            (x.Elf2.Item1 <= x.Elf1.Item1 && x.Elf1.Item1 <= x.Elf2.Item2) ||
            (x.Elf2.Item1 <= x.Elf1.Item2 && x.Elf1.Item2 <= x.Elf2.Item2) ||
            (x.Elf1.Item1 <= x.Elf2.Item1 && x.Elf2.Item1 <= x.Elf1.Item2) ||
            (x.Elf1.Item1 <= x.Elf2.Item2 && x.Elf2.Item2 <= x.Elf1.Item2));

        return overlaps.Count().ToString();
    }
}

public class CleaningPair
{
    public CleaningPair(string input)
    {
        var splitInput = input.Split(",");
        var firstElfSplit = splitInput.First().Split("-");
        var secondElfSplit = splitInput.Last().Split("-");
        Elf1 = Tuple.Create(int.Parse(firstElfSplit.First()), int.Parse(firstElfSplit.Last()));
        Elf2 = Tuple.Create(int.Parse(secondElfSplit.First()), int.Parse(secondElfSplit.Last()));
    }
    public Tuple<int,int> Elf1 { get; set; }
    public Tuple<int,int> Elf2 { get; set; }
}
