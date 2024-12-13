namespace Days2024;

public class Day_02 : Day
{
    private List<Report> reports = new();
    public Day_02()
    {
        Title = "Red-Nosed Reports";
        DayNumber = 2;
        Year = 2024;
    }
    public override void Gather_input()
    {
        reports = Input.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new Report(x.Split(' ').Where(y => !string.IsNullOrWhiteSpace(y)).Select(int.Parse))).ToList();
    }

    public override string HandlePart1()
    {
        var safeReadings = reports.Where(x => x.IsSafe()).ToList();
        return safeReadings.Count().ToString();
    }

    public override string HandlePart2()
    {
        var safeReadings = reports.Where(x => x.IsP2Safe()).ToList();
        return safeReadings.Count().ToString();
    }
}

public class Report
{
    public Report(IEnumerable<int> input)
    {
        Readings = input.ToList();
        DampenedReadings = Enumerable.Range(0, input.Count() + 1)
            .Select((x, index) => input.Where((y, yindex) => yindex != index).ToList()).ToList();
    }
    public List<int> Readings { get; set; }
    public List<List<int>> DampenedReadings { get; set; }
    private List<(int, int)> zippedReadings => Readings.Zip(Readings.Skip(1), (a, b) => (a, b)).ToList();

    public bool IsSafe() =>
        (zippedReadings.All(p => p.Item1 < p.Item2) || zippedReadings.All(p => p.Item1 > p.Item2)) &&
        zippedReadings.All(p => Math.Abs(p.Item1 - p.Item2) >= 1 && Math.Abs(p.Item1 - p.Item2) <= 3);

    public bool IsP2Safe()
    {
        return DampenedReadings.Any(x =>
        {
            var zipped = x.Zip(x.Skip(1), (a, b) => (a, b)).ToList();
            return (zipped.All(p => p.Item1 < p.Item2) || zipped.All(p => p.Item1 > p.Item2)) &&
                   zipped.All(p => Math.Abs(p.Item1 - p.Item2) >= 1 && Math.Abs(p.Item1 - p.Item2) <= 3);
        });
    }
}