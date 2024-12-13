namespace Days2024;

public class Day_01 : Day
{
    public List<List<int>> locationIdLists = new();
    public Day_01()
    {
        Title = "Historian Hysteria";
        DayNumber = 1;
        Year = 2024;
    }
    
    public override void Gather_input()
    {
        var parsedInput = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x =>
            x.Split(' ').Where(y => !string.IsNullOrWhiteSpace(y)).Select(int.Parse).ToList()).ToList();

        locationIdLists = Enumerable.Range(0, parsedInput.First().Count()).Select(x => parsedInput.Select(y => y[x]).ToList())
            .ToList();
    }

    public override string HandlePart1()
    {
        var sortedLists = locationIdLists.Select(x => x.Order().ToList());
        var distances = Enumerable.Range(0, locationIdLists.First().Count).Select(x => Math.Abs(sortedLists.Select(y => y[x]).ToList()[1..].Append(-sortedLists.First()[x]).Sum()));
        var sumOfDistances = distances.Sum();
        return sumOfDistances.ToString();
    }

    public override string HandlePart2()
    {
        return locationIdLists.First().Select(x => x * locationIdLists.Last().Count(y => y == x)).Sum().ToString();
    }
}