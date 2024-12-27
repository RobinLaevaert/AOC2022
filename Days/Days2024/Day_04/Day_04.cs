using System.IO.Compression;

namespace Days2024;

public class Day_04 : Day
{
    public Day_04()
    {
        Title = "Ceres Search";
        DayNumber = 4;
        Year = 2024;
    }
    public List<LetterCoordinate> LetterCoordinates;
    public override void Gather_input()
    {
        LetterCoordinates =
            Input.SelectMany((line, y) => line.Select((letter, x) => new LetterCoordinate(letter, x, y))).ToList();
    }

    public override string HandlePart1()
    {
        LetterCoordinates = LetterCoordinates.Select(y =>
        {
            y.Neighbours = LetterCoordinates.Where(z =>
                (z.X - 1 == y.X || y.X == z.X + 1 || y.X == z.X) &&
                (z.Y - 1 == y.Y || y.Y == z.Y + 1 || y.Y == z.Y) &&
                (z.Y != y.Y || z.X != y.X)).ToList();
            return y;
        }).ToList();
        var xmasCount = LetterCoordinates.Where(x => x.Letter == 'X')
            .Select(start => start.Neighbours.SelectMany(x =>
            {
                var xDiff = x.X - start.X;
                var yDiff = x.Y - start.Y;
                return x.Neighbours.Where(y => y.X - x.X == xDiff && y.Y - x.Y == yDiff)
                    .SelectMany(z =>
                    {
                        return z.Neighbours.Where(a => a.X - z.X == xDiff && a.Y - z.Y == yDiff)
                            .Select(a => new string([start.Letter, x.Letter, z.Letter, a.Letter]));
                    })
                    .ToList();
            }))
            .Select(possibleStrings => possibleStrings.Count(x => x == "XMAS"))
            .Sum();
        return xmasCount.ToString();
    }

    public override string HandlePart2()
    {
        LetterCoordinates = LetterCoordinates.Select(y =>
        {
            y.Neighbours = LetterCoordinates.Where(z =>
                (z.X - 1 == y.X || y.X == z.X + 1 || y.X == z.X) &&
                (z.Y - 1 == y.Y || y.Y == z.Y + 1 || y.Y == z.Y) &&
                (z.Y != y.Y || z.X != y.X)).ToList();
            return y;
        }).ToList();
        var MASs = new List<Temp>();
        foreach (var start in LetterCoordinates.Where(x => x.Letter == 'M'))
        {
            var possibleStrings = start.Neighbours.SelectMany(x =>
            {
                var xDiff = x.X- start.X;
                var yDiff = x.Y- start.Y;
                return x.Neighbours
                    .Where(y => xDiff != 0 && yDiff != 0)
                    .Where(y => y.X - x.X == xDiff && y.Y - x.Y == yDiff)
                    .Where(y => new string([start.Letter, x.Letter, y.Letter]) == "MAS")
                    .Select(z => new Temp
                    {
                        AX = x.X,
                        AY = x.Y,
                        xDiff = xDiff,
                        yDiff = yDiff,
                    }).ToList();
            });
            MASs.AddRange(possibleStrings);
        }
        return MASs.GroupBy(x => new {x.AX, x.AY}).Count(x => x.Count() > 1).ToString();
    }
}

public class LetterCoordinate(char letter, int x, int y)
{
    public char Letter = letter;
    public int X = x;
    public int Y = y;
    public List<LetterCoordinate> Neighbours = new List<LetterCoordinate>();
}

public class Temp
{
    public int xDiff { get; set; }
    public int yDiff { get; set; }
    public int AX { get; set; }
    public int AY { get; set; }
}