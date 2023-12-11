namespace Days2023;

public class Day_11 : Day
{
	private CosmicImage Original;

	public Day_11()
	{
		Title = "Cosmic Expansion";
		DayNumber = 11;
		Year = 2023;
	}

	public override void Gather_input()
	{
		Original = new CosmicImage()
		{
			Galaxies = Input.SelectMany((row, y) => row.Select((galaxy, x) => new Galaxy()
			{
				Location = new Coord(x, y),
				IsEmpty = galaxy == '.'
			})).ToList()
		};
	}

	public override string HandlePart1() => GetSumOfDistancesAfterExpansion(2).ToString();

	public override string HandlePart2() => GetSumOfDistancesAfterExpansion(1000000).ToString();

	private long GetSumOfDistancesAfterExpansion(int expansionRate)
	{
		var emptyColumnsIndexes = Original.Galaxies.GroupBy(x => x.Location.X).Where(x => x.All(y => y.IsEmpty))
			.Select(x => x.Key).ToList();
		var emptyRowsIndexes = Original.Galaxies.GroupBy(x => x.Location.Y).Where(x => x.All(y => y.IsEmpty))
			.Select(x => x.Key).ToList();

		var factor = expansionRate - 1;
		var nonEmptyGalaxies = Original.Galaxies.Where(x => !x.IsEmpty);
		var nonEmptyGalaxiesAfterExpansion = nonEmptyGalaxies.Select(x => new Galaxy()
		{
			IsEmpty = x.IsEmpty,
			Location = new Coord(x.Location.X + (emptyColumnsIndexes.Count(z => z < x.Location.X) * factor),
				x.Location.Y + (emptyRowsIndexes.Count(z => z < x.Location.Y) * factor))
		});

		var galaxyPairs = (from item1 in nonEmptyGalaxiesAfterExpansion
			from item2 in nonEmptyGalaxiesAfterExpansion
			where item1 < item2
			select Tuple.Create(item1, item2)).ToList();

		var distances = galaxyPairs.Select(x => Tuple.Create(x,
			(long)Math.Abs(x.Item2.Location.Y - x.Item1.Location.Y) + Math.Abs(x.Item2.Location.X - x.Item1.Location.X))
		).ToList();

		var totalDistance = distances.Sum(x => x.Item2);
		return totalDistance;
	}
}

public class CosmicImage
{
	public List<Galaxy> Galaxies { get; set; }
}

public class Galaxy : IComparable<Galaxy>
{
	public Coord Location { get; set; }
	public bool IsEmpty { get; set; }

	public int CompareTo(Galaxy? other)
	{
		if (ReferenceEquals(this, other)) return 0;
		if (ReferenceEquals(null, other)) return 1;
		return Location.CompareTo(other.Location);
	}

	public static bool operator <(Galaxy operand1, Galaxy operand2)
	{
		return operand1.CompareTo(operand2) < 0;
	}

	public static bool operator >(Galaxy operand1, Galaxy operand2)
	{
		return operand1.CompareTo(operand2) < 0;
	}
}


