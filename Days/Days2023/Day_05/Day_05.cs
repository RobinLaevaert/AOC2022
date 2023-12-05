using System.Collections.Concurrent;

namespace Days2023;

public class Day_05 : Day
{
	private List<long> seeds;
	private List<Map> Maps { get; set; }
	public Day_05()
	{
		Title = "You Give A Seed A Fertilizer";
		DayNumber = 5;
		Year = 2023;
	}

	public override void Gather_input()
	{
		seeds = Input.Single(x => x.StartsWith("seeds:")).Split("seeds:").Last().Trim().Split(" ")
			.Select(x => long.Parse(x.Trim())).ToList();
		Maps = new List<Map>();
		Map map = null;
		foreach (var line in Input.Where(x =>!x.StartsWith("seeds:")))
		{
			if(string.IsNullOrWhiteSpace(line))
			{
				if(map != null) Maps.Add(map);
				continue;
			}

			if (line.Contains("map"))
			{
				map = new Map()
				{
					Name = line.Split(" map:").First(),
					Ranges = new List<Range>()
				};
				continue;
			}

			var numbers = line.Split(" ").Select(x => long.Parse(x.Trim())).ToList();
			map.Ranges.Add(new Range()
			{
				DestinationStart = numbers[0],
				SourceStart = numbers[1],
				Length = numbers[2]
			});
		}
		if(Maps.Count != 7) Maps.Add(map);
	}

	public override string HandlePart1()
	{
		var CurrentNumbers = seeds.ToList();
		foreach (var map in Maps)
		{
			for (int i = 0; i < CurrentNumbers.Count(); i++)
			{
				var correspondingRange =
					map.Ranges.SingleOrDefault(x => x.SourceStart <= CurrentNumbers[i] && CurrentNumbers[i] <= x.SourceEnd);
				if (correspondingRange == null) continue;
				CurrentNumbers[i] = correspondingRange.DestinationStart + (CurrentNumbers[i] - correspondingRange.SourceStart);
			}
		}

		return CurrentNumbers.Min().ToString();
	}

	public override string HandlePart2()
	{
		var lowNumbs = new ConcurrentBag<long>();
		Parallel.ForEach(seeds.Chunk(2), numbers =>
		{
			long testing = 0;
			long tempTesing = long.MaxValue;
			for (var i = 0; i < numbers[1]; i++)
			{
				testing = numbers[0] + i;
				foreach (var map in Maps)
				{
					var correspondingRange =
						map.Ranges.SingleOrDefault(x => x.SourceStart <= testing && testing <= x.SourceEnd);
					if (correspondingRange == null) continue;
					testing = correspondingRange.DestinationStart + (testing - correspondingRange.SourceStart);
				}

				if (testing < tempTesing) tempTesing = testing;
			}

			lowNumbs.Add(tempTesing);
		});
		return lowNumbs.Min().ToString();
	}
}

public class Map
{
	public string Name { get; set; }
	public List<Range> Ranges { get; set; }
}

public class Range
{
	public long DestinationStart { get; set; }
	public long SourceStart { get; set; }
	public long Length { get; set; }
	public long DestinationEnd => DestinationStart + Length - 1;
	public long SourceEnd => SourceStart + Length - 1;
}