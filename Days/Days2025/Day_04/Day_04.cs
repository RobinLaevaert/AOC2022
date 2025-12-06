namespace Days2025;

public class Day_04 : Day
{
    List<Location> _locations;
    public Day_04()
    {
        Title = "Printing Department";
        DayNumber = 4;
        Year = 2025;
    }

    public override void Gather_input()
    {
        _locations = Input.Where(x => !string.IsNullOrEmpty(x)).SelectMany((line, y) => line.Select((reading, x) =>
                new Location()
                {
                    Character = reading,
                    Coordinate = new Coordinate(x, y)
                }))
            .ToList();
    }

    public override string HandlePart1()
    {
        var neighbours = _locations.Select(loc =>
        {
            return new
            {
                location = loc,
                neighbours = _locations.Where(loc2 =>
                    (loc2.Coordinate.X >= loc.Coordinate.X - 1 &&
                     loc2.Coordinate.X <= loc.Coordinate.X + 1)
                    &&
                    (loc2.Coordinate.Y >= loc.Coordinate.Y - 1 &&
                     loc2.Coordinate.Y <= loc.Coordinate.Y + 1)
                    &&
                    (!(loc.Coordinate.X == loc2.Coordinate.X && loc.Coordinate.Y == loc2.Coordinate.Y))
                )
            };
        }).Where(x => x.location.IsRoll && x.neighbours.Count(y => y.IsRoll) < 4);
        
        return neighbours.Count().ToString();
    }

    public override string HandlePart2()
    {
        var canChange = true;
        var locations = _locations.ToList();
        var removedRolls = 0;
        while (canChange)
        {
            var removableLocations = GetLocationsToRemove(locations);
            if (removableLocations.Count == 0)
            {
                canChange = false;
                break;
            }

            locations = locations.Select(x =>
            {
                if (x.IsRoll && removableLocations.Any(y => y.Coordinate.X == x.Coordinate.X && y.Coordinate.Y == x.Coordinate.Y))
                {
                    x.Character = '.';
                    removedRolls++;
                }

                return x;
            }).ToList();
        }
        return removedRolls.ToString();
    }

    public static List<Location> GetLocationsToRemove(List<Location> input) =>
        input.Where(loc =>

            loc.IsRoll &&
            input.Where(loc2 =>
                (loc2.Coordinate.X >= loc.Coordinate.X - 1 &&
                 loc2.Coordinate.X <= loc.Coordinate.X + 1)
                &&
                (loc2.Coordinate.Y >= loc.Coordinate.Y - 1 &&
                 loc2.Coordinate.Y <= loc.Coordinate.Y + 1)
                &&
                (!(loc.Coordinate.X == loc2.Coordinate.X && loc.Coordinate.Y == loc2.Coordinate.Y))
            ).Count(loc2 => loc2.IsRoll) < 4).ToList();
}

public record struct Coordinate(int X, int Y);

public class Location
{
    public Coordinate Coordinate { get; set; }
    public char Character { get; set; }
    public bool IsRoll => Character == '@';
}