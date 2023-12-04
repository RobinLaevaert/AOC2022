using System.Text.RegularExpressions;
using Shared;

namespace Days2023;

public partial class Day_03 : Day
{
    public List<Coordinate> Coordinates;
    public Day_03()
    {
        Title = "Gear Ratios";
        DayNumber = 3;
        Year = 2023;
    }
    public override void Gather_input()
    {
        Coordinates = new List<Coordinate>();
        var yIndex = 0;
        foreach (var s in Input)
        {
            Coordinates.AddRange(s.Select((x,xIndex) => new Coordinate()
            {
                Y = yIndex,
                X = xIndex,
                Value = x.ToString()
            }));
            yIndex++;
        }
    }

    public override string HandlePart1()
    {
        var CoordNumbers = new List<CoordinateNumber>();
        var actualNumbers = Coordinates.Where(x => x.IsNumber);
        foreach (var y in actualNumbers.Select(x => x.Y).Distinct())
        {
            var coordinatesInCurrentRow = Coordinates.Where(x => x.Y == y);
            var str = string.Concat(coordinatesInCurrentRow.Select(x => x.Value));

            var coords = new List<Coordinate>();
            for (var i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                {
                    coords.Add(coordinatesInCurrentRow.Single(z => z.X == i));
                }
                else
                {
                    if (coords.Any())
                    {
                        CoordNumbers.Add(new CoordinateNumber()
                        {
                            Coordinates = coords
                        });
                        coords = new List<Coordinate>();
                    }
                }

                if (i == str.Length - 1)
                {
                    if (coords.Any())
                    {
                        CoordNumbers.Add(new CoordinateNumber()
                        {
                            Coordinates = coords
                        });
                        coords = new List<Coordinate>();
                    }
                }
            }
        }

        var symbols = Coordinates.Where(x => x.IsSymbol);
        var numbersAdjacentToSymbols = CoordNumbers.Where(x => x.Coordinates.Any(y =>
            symbols.Any(z =>
                (z.X - 1 == y.X || y.X == z.X + 1 || y.X == z.X) &&
                (z.Y - 1 == y.Y || y.Y == z.Y + 1 || y.Y == z.Y)
            ))).ToList();
        return numbersAdjacentToSymbols.Sum(x => x.Number).ToString();
    }

    public override string HandlePart2()
    {
        var CoordNumbers = new List<CoordinateNumber>();
        var actualNumbers = Coordinates.Where(x => x.IsNumber);
        foreach (var y in actualNumbers.Select(x => x.Y).Distinct())
        {
            var coordinatesInCurrentRow = Coordinates.Where(x => x.Y == y);
            var str = string.Concat(coordinatesInCurrentRow.Select(x => x.Value));

            var coords = new List<Coordinate>();
            for (var i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                {
                    coords.Add(coordinatesInCurrentRow.Single(z => z.X == i));
                }
                else
                {
                    if (coords.Any())
                    {
                        CoordNumbers.Add(new CoordinateNumber()
                        {
                            Coordinates = coords
                        });
                        coords = new List<Coordinate>();
                    }
                }

                if (i == str.Length - 1)
                {
                    if (coords.Any())
                    {
                        CoordNumbers.Add(new CoordinateNumber()
                        {
                            Coordinates = coords
                        });
                        coords = new List<Coordinate>();
                    }
                }
            }
        }
        
        var result = Coordinates.Where(x => x.IsGear).Select(x => CoordNumbers.Where(y => y.Coordinates.Any(y =>
                (x.X - 1 == y.X || y.X == x.X + 1 || y.X == x.X) &&
                (x.Y - 1 == y.Y || y.Y == x.Y + 1 || y.Y == x.Y)))).Where(x => x.Count() == 2)
            .Select(x => x.First().Number * x.Last().Number).Sum();

        return result.ToString();

        return "";
    }
    
}

public class PartNumber
{
    public int Number { get; set; }

}

public class Coordinate
{
    public string Value { get; set; }
    public int? Number => IsNumber ? int.Parse(Value) : null;
    public bool IsSymbol => !IsNumber && Value != "." && !string.IsNullOrWhiteSpace(Value);
    public bool IsNumber => int.TryParse(Value, out var Number);
    public bool IsGear => Value == "*";
    public int X { get; set; }
    public int Y { get; set; }
}

public class CoordinateNumber
{
    public int Number => int.Parse(string.Concat(Coordinates.OrderBy(x => x.Y).ThenBy(x => x.X).Select(x => x.Value)));
    public List<Coordinate> Coordinates { get; set; }
}