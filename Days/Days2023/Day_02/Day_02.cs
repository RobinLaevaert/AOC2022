namespace Days2023;

public class Day_02 : Day
{
    public List<Game> Games;
    public Day_02()
    {
        Title = "Cube Conundrum";
        DayNumber = 2;
        Year = 2023;
    }
    
    public override void Gather_input()
    {
        Games = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Game()
        {
            Id = Convert.ToInt16(x.Split(":")[0].Split("Game ").Last()),
            SubSets = x.Split(":").Last().Split(";").Select(y => new SubSet(y)).ToList()
        }).ToList();
    }

    public override string HandlePart1()
    {
        var maxRed = 12;
        var maxGreen = 13;
        var maxBlue = 14;

        var possibleGames =
            Games.Where(x => x.SubSets.All(y => y.Blue <= maxBlue && y.Green <= maxGreen && y.Red <= maxRed));

        var sum = possibleGames.Sum(x => x.Id);
        return sum.ToString();
    }

    public override string HandlePart2()
    {
        var powers =
            Games.Select(x => x.SubSets.Max(y => y.Blue) * x.SubSets.Max(y => y.Red) * x.SubSets.Max(y => y.Green));
        var sum = powers.Sum();
        return sum.ToString();
    }
}

public class Game
{
    public int Id { get; set; }
    public List<SubSet> SubSets { get; set; }
}

public class SubSet
{
    public SubSet(string subsetString)
    {
        Blue = subsetString.Split(',').Any(x => x.Contains(nameof(Blue).ToLower()))
            ? int.Parse(subsetString.Split(',').First(x => x.Contains(nameof(Blue).ToLower())).Split(" ")
                .First(x => int.TryParse(x, out var y)))
            : 0;
        Red = subsetString.Split(',').Any(x => x.Contains(nameof(Red).ToLower()))
            ? int.Parse(subsetString.Split(',').First(x => x.Contains(nameof(Red).ToLower())).Split(" ")
                .First(x => int.TryParse(x, out var y)))
            : 0;
        Green = subsetString.Split(',').Any(x => x.Contains(nameof(Green).ToLower()))
            ? int.Parse(subsetString.Split(',').First(x => x.Contains(nameof(Green).ToLower())).Split(" ")
                .First(x => int.TryParse(x, out var y)))
            : 0;
    }
    public int Blue { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
}