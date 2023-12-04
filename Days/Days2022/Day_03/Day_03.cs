namespace Days2022;

public class Day_03 : Day
{
    List<RuckSack> ruckSacks;
    public Day_03()
    {
        DayNumber = 3;
        Year = 2022;
        Title = "Rucksack Reorganization";
    }
    public override void Gather_input()
    {
        ruckSacks = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new RuckSack(x)).ToList();
    }

    public override string HandlePart1()
    {
        return ruckSacks.Sum(x => x.Duplicates.Sum(GetPrio)).ToString();
    }

    public override string HandlePart2()
    {
        return ruckSacks.Chunk(3).Sum(x => x[0].Complete.Intersect(x[1].Complete).Intersect(x[2].Complete).Sum(GetPrio)).ToString();
    }

    private static int GetPrio(char c) => c - 96 < 0 ? c - 38 : c - 96;
}

public class RuckSack
{
    public RuckSack(string input)
    {
        Complete = input.ToCharArray();
    }
    public IEnumerable<Char> Complete { get; set; }
    private IEnumerable<Char[]> Comparted => Complete.Chunk(Complete.ToList().Count/2).ToList();
    public IEnumerable<Char> Compartment1 => Comparted.First().ToList();
    public IEnumerable<Char> Compartment2 => Comparted.Last().ToList();
    public IEnumerable<Char> Duplicates => Compartment1.Intersect(Compartment2);
}