namespace Days2025;

public class Day_03 : Day
{
    List<List<int>> banks = [];
    public Day_03()
    {
        Title = "Lobby";
        DayNumber = 3;
        Year = 2025;
    }
    public override void Gather_input()
    {
        banks = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Select(y => y - '0').ToList()).ToList();
    }

    public override string HandlePart1()
    {
        var joltages = banks.Select(bank =>
        {
            var rightDigit = 0;
            var leftDigit = 0;
            for (var i = 0; i < bank.Count; i++)
            {
                var num = bank[i];
                if (num > leftDigit && i != bank.Count - 1)
                {
                    leftDigit = num;
                    rightDigit = 0;
                }
                else if (num > rightDigit)
                {
                    rightDigit = num;
                }
            }

            return int.Parse($"{leftDigit}{rightDigit}");
        });
        return joltages.Sum().ToString();
    }

    public override string HandlePart2()
    {
        var joltages = banks.Select(bank =>
        {
            List<long> numbers = [];
            var lastIndex = 0;
            var lastNumber = 0;
            for (var i = 1; i <= 12; i++)
            {
                var leftRange = 0;
                if (i != 1)
                {
                    leftRange = bank[lastIndex..].FindIndex(y => y == lastNumber) + lastIndex + 1;
                }
                var number = bank[leftRange..^(12-i)].Max();
                lastNumber = number;
                numbers.Add(number);
                lastIndex = (bank[leftRange..].FindIndex(y => y == lastNumber) + leftRange);
            }

            var resultNumber = numbers.Select((y, index) => y * Math.Pow(10, (numbers.Count - (index+1)))).Sum();
            return resultNumber;
        });
        return joltages.Sum().ToString();
    }
}