using System.Security.AccessControl;

namespace Days2023;

public class Day_06 : Day
{
    public List<Race> Races;
    public Day_06()
    {
        Title = "Wait For It";
        DayNumber = 6;
        Year = 2023;
    }
    public override void Gather_input()
    {
        var times = Input.Single(x => x.StartsWith("Time:")).Split("Time:").Last().Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => int.Parse(x.Trim()));
        var distances = Input.Single(x => x.StartsWith("Distance:")).Split("Distance:").Last().Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => int.Parse(x.Trim())).ToList();

        Races = times.Select((x, index) => new Race()
        {
            Time = x,
            Distance = distances[index]
        }).ToList();
    }

    public override string HandlePart1()
    {
        return Races.Aggregate(1, (a, x) => a * (int)x.PossibleTimes).ToString();
    }

    public override string HandlePart2()
    {
        var actualRaceDistance = string.Join("",Races.Select(x => x.Distance.ToString()));
        var actualRaceTime = string.Join("",Races.Select(x => x.Time.ToString()));
        var actualRace = new Race()
        {
            Distance = long.Parse(actualRaceDistance),
            Time = int.Parse(actualRaceTime)
        };
        
        return actualRace.PossibleTimes.ToString();
    }
}

public class Race
{
    public int Time { get; set; }
    public long Distance { get; set; }
    private long CutOff1 => (long)Math.Ceiling((Time - Math.Sqrt(Math.Pow(Time, 2) - 4 * Distance)) / 2);
    private long CutOff2 => (long)Math.Ceiling((Math.Sqrt(Math.Pow(Time, 2) - 4 * Distance) + Time ) / 2);
    public long PossibleTimes => Math.Min(CutOff2, Time) - Math.Max(CutOff1, 0);
}