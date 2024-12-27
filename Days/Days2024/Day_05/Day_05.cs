namespace Days2024;

public class Day_05 : Day
{
    public Day_05()
    {
        Title = "Print Queue";
        DayNumber = 5;
        Year = 2024;
    }

    public List<Rule> Rules;
    public List<List<int>> Updates;
    public override void Gather_input()
    {
        Rules = Input.Where(x => x.Contains('|'))
            .Select(x => new Rule(int.Parse(x.Split('|')[0]), int.Parse(x.Split('|')[1]))).ToList();
        Updates = Input.Where(x => x.Contains(','))
            .Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();
    }

    public override string HandlePart1()
    {
        var MiddlePoints = new List<int>();
        foreach (var update in Updates)
        {
            var isValid = true;
            var middlePoint = update[(update.Count - 1) / 2];
            var passedPoints = new List<int>();
            foreach (var point in update)
            {
                var relevantRules = Rules.Where(x => x.After == point).ToList();
                if (relevantRules.Count != 0)
                {
                    if (relevantRules.Any(y => update.Contains(y.Before) && !passedPoints.Contains(y.Before)))
                    {
                        isValid = false;
                        break;
                    }
                }
                passedPoints.Add(point);
            }
            
            if(isValid)
                MiddlePoints.Add(middlePoint);
        }
        return MiddlePoints.Sum().ToString();
    }

    public override string HandlePart2()
    {
        var invalidLines = new List<List<int>>();
        foreach (var update in Updates)
        {
            var passedPoints = new List<int>();
            foreach (var point in update)
            {
                var relevantRules = Rules.Where(x => x.After == point).ToList();
                if (relevantRules.Count != 0)
                {
                    if (relevantRules.Any(y => update.Contains(y.Before) && !passedPoints.Contains(y.Before)))
                    {
                        invalidLines.Add(update);
                        break;
                    }
                }
                passedPoints.Add(point);
            }
        }
        
        var MiddlePoints = new List<int>();
        foreach (var update in invalidLines)
        {
            var updateHolder = update.ToList();
            var passedPoints = new List<int>();
            var FirstPoint = updateHolder.Single(x => Rules.Where(y => y.After == x).All(y => !update.Contains(y.Before)));
            passedPoints.Add(FirstPoint);
            updateHolder.Remove(FirstPoint);
            for (int i = 0; i < update.Count-1; i++)
            {
                var temp = updateHolder
                    .Where(x => Rules.Where(y => y.After == x).All(y => passedPoints.Contains(y.Before) || !update.Contains(y.Before))).ToList();
                if (temp.Count == 1)
                {
                    passedPoints.Add(temp[0]);
                    updateHolder.Remove(temp[0]);
                }
                else
                {
                    Console.WriteLine("IDK, hope this doesn't happen");
                }
                    
            }
            MiddlePoints.Add(passedPoints[(passedPoints.Count-1)/2]);
        }

        return MiddlePoints.Sum().ToString();
    }
}

public record struct Rule(int Before, int After);