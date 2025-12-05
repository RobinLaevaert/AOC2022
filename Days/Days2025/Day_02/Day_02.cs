using System.Text.RegularExpressions;

namespace Days2025;

public class Day_02 : Day
{
    public Day_02()
    {
        Title = "Gift Shop";
        DayNumber = 2;
        Year = 2025;
    }
    List<Range> ranges = [];
    public override void Gather_input()
    {
        ranges = Input.First(x => !string.IsNullOrEmpty(x)).Split(',').Select(x =>
        {
            var edges = x.Split('-');
            return new Range(long.Parse(edges[0]), long.Parse(edges[1]));
        }).ToList();
    }

    public override string HandlePart1()
    {
        var numbers = new List<long>();
        foreach (var range in ranges)
        {
            for (var i = range.Start; i < range.End; i++)
            {
                var str = i.ToString();
                if(str.Length %2 != 0) continue;
                var m = str.Length / 2;
                if (str[..m] == str[m..])
                    numbers.Add(i);
            }
        }

        return numbers.Sum().ToString();
    }
    
    public override string HandlePart2()
    {
        var numbers = new List<long>();
        foreach (var range in ranges)
        {
            for (var i = range.Start; i <= range.End; i++)
            {
                var str = i.ToString();
                var maxL = str.Length / 2;
                var found = false;
                for (var j = 1; j <= maxL; j++)
                {
                    if (str.Length % j != 0) continue;
                    var pattern = str[..j];
                    var occurences = Regex.Matches(str, pattern).Count;
                    if (occurences == str.Length / pattern.Length)
                    {
                        found = true;
                        break;
                    }
                }

                if (found) numbers.Add(i);
            }
        }
        return numbers.Sum().ToString();
    }
}

public record struct Range(long Start, long End);