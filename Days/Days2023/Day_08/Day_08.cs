namespace Days2023;

public class Day_08 : Day
{
	private List<int> instructions;
	private Dictionary<string, Tuple<string, string>> maps;
	public Day_08()
	{
		Title = "Haunted Wasteland";
		DayNumber = 8;
		Year = 2023;
	}

	public override void Gather_input()
	{
		instructions = Input.First().Select(x => x == 'L' ? 0 : 1).ToList();
		maps = Input.Skip(2).Where(x => !string.IsNullOrWhiteSpace(x)).ToDictionary(x => x.Split(" = ").First(), x =>
		{
			var cleaned = x.Split("=").Last().Trim().Trim('(').Trim(')').Split(", ");
			return Tuple.Create(cleaned[0], cleaned[1]);
		});
	}

	public override string HandlePart1()
	{
		var start = "AAA";
		var end = "ZZZ";
		var current = start;
		var index = 0;
		while (current != end)
		{
			var actualIndex = index % instructions.Count;

			var map = maps[current];
			current = instructions[actualIndex] == 0 ? map.Item1 : map.Item2;

			index++;
		}

		return index.ToString();
	}

	public override string HandlePart2()
	{
		long tempLcm = 1;
		foreach (var map in maps.Where(x => x.Key.EndsWith("A")).ToList())
		{
			var currentMap = map.Key;
			var index = 0;
			while (!currentMap.EndsWith("Z"))
			{
				var actualIndex = index % instructions.Count;

				var mapt = maps[currentMap];
				currentMap = instructions[actualIndex] == 0 ? mapt.Item1 : mapt.Item2;

				index++;
			}

			tempLcm = lcm(tempLcm, index);
		}

		return tempLcm.ToString();
	}

	static long gcf(long a, long b)
	{
		while (b != 0)
		{
			long temp = b;
			b = a % b;
			a = temp;
		}
		return a;
	}
	
	static long lcm(long a, long b)
	{
		return (a / gcf(a, b)) * b;
	}
}