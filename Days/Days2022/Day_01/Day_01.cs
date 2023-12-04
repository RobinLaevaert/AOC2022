namespace Days2022;

public class Day_01 : Day
{
    public List<Elf> input = new ();
    public Day_01()
    {
        Title = "Calorie Counting";
        DayNumber = 1;
        Year = 2022;
    }
    public override void Gather_input()
    {
        var elfCount = 1;
        var currentElf = new Elf(elfCount);
        foreach (var line in Input)
        {
            if (string.IsNullOrEmpty(line))
            {
                input.Add(currentElf);
                elfCount++;
                currentElf = new(elfCount);
            }
            else
            {
                currentElf.Calories.Add(int.Parse(line));
            }
        }
    }

    public override string HandlePart1()
    {
        var maxElf = input.OrderByDescending(x => x.Calories.Sum()).First();
        return $"{maxElf.Calories.Sum()}";
    }

    public override string HandlePart2()
    {
        var maxElves = input.OrderByDescending(x => x.Calories.Sum()).Take(3);
        return $"{maxElves.Sum(x => x.Calories.Sum())}";
    }
}

public class Elf
{
    public Elf(int number)
    {
        Number = number;
        Calories = new();
    }
    public int Number { get; set; }
    public List<int> Calories { get; set; }
}