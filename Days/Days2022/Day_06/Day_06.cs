namespace Days2022;

public class Day_06 : Day
{
    private string input;
    public Day_06()
    {
        Title = "Tuning Trouble";
        DayNumber = 6;
        Year = 2022;
    }

    public override void Gather_input()
    {
        input = Input.First();
    }

    public override string HandlePart1()
    {
        var nr = 0;
        for (int i = 0; i < input.Length-3; i++)
        {
            var temp = input[i..(i + 4)];
            if (!temp.GroupBy(x => x).Any(g => g.Count() > 1))
            {
                nr = i + 4;
                break;
            }
        }

        return nr.ToString();
    }

    public override string HandlePart2()
    {
        var nr = 0;
        for (int i = 0; i < input.Length-13; i++)
        {
            var temp = input[i..(i + 14)];
            if (!temp.GroupBy(x => x).Any(g => g.Count() > 1))
            {
                nr = i + 14;
                break;
            }
        }

        return nr.ToString();
    }
}