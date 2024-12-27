namespace Days2024;

public class Day_03 : Day
{
    private List<string> input;
    public Day_03()
    {
        Title = "Mull It Over";
        DayNumber = 3;
        Year = 2024;
    }
    public override void Gather_input()
    {
        input = Input.ToList();
    } 

    public override string HandlePart1()
    {
        var totalCount = 0;
        foreach (var line in input)
        {
            var count = 0;
            for (var i = 0; i < line.Length; i++)
            {
                var left = i;
                var right = i + 12;
                if(right > line.Length) right = line.Length;
                var t = line[left .. right];
                if (t.StartsWith("mul("))
                {
                    var closingIndex = t.IndexOf(')');
                    if (closingIndex != -1)
                    {
                        var actualMultiplierString = t[4..closingIndex];
                        var multiplierFactors = actualMultiplierString.Split(',');
                        var parsable = multiplierFactors.Select(x => int.TryParse(x, out var result));
                        if (multiplierFactors.Length == 2 && parsable.All(x => x))
                        {
                            count += multiplierFactors.Aggregate(1, (x, y) => x * int.Parse(y));
                        }
                    }
                }
            }
            totalCount+=count;
        }

        return totalCount.ToString();
    }

    public override string HandlePart2()
    {
        var totalCount = 0;
        var currentStart = true;
        foreach (var line in input)
        {
            var count = 0;
            var multiplierResults = new List<Tuple<int, int>>(); // Result, startIndex
            var conditions = new List<Tuple<bool, int>>() { Tuple.Create(currentStart, 0) };
            for (var i = 0; i < line.Length; i++)
            {
                var left = i;
                var right = i + 12;
                if(right > line.Length) right = line.Length;
                var t = line[left .. right];
                if (t.StartsWith("mul("))
                {
                    var closingIndex = t.IndexOf(')');
                    if (closingIndex != -1)
                    {
                        var actualMultiplierString = t[4..closingIndex];
                        var multiplierFactors = actualMultiplierString.Split(',');
                        var parsable = multiplierFactors.Select(x => int.TryParse(x, out var result));
                        if (multiplierFactors.Length == 2 && parsable.All(x => x))
                        {
                            multiplierResults.Add(Tuple.Create(multiplierFactors.Aggregate(1, (x, y) => x * int.Parse(y)), left));
                        }
                    }
                }

                var right2 = i + 7;
                if (right2 > line.Length) right2 = line.Length;
                var t2 = line[left .. right2];
                if (t2.StartsWith("do()"))
                {
                    conditions.Add(Tuple.Create(true, left));
                }
                if (t2.StartsWith("don't()"))
                {
                    conditions.Add(Tuple.Create(false, left));
                }
            }

            var finalLineCount = multiplierResults.Where(x => conditions.Last(y => y.Item2 < x.Item2).Item1).Sum(x => x.Item1);
            totalCount+=finalLineCount;
            currentStart = conditions.Last().Item1;
        }

        return totalCount.ToString();
    }
}