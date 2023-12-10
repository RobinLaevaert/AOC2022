namespace Days2023;

public class Day_10 : Day
{
	private Dictionary<Coord, Tile> Tiles;
	private Coord StartCoord;
	public Day_10()
	{
		Title = "Pipe Maze";
		DayNumber = 10;
		Year = 2023;
	}

	public override void Gather_input()
	{
		Tiles = Input.Where(x => !string.IsNullOrWhiteSpace(x)).SelectMany((row, y) => row.Select((tile, x) =>

			new Tile()
			{
				Location = new Coord(x, y),
				Description = tile
			}
		)).ToDictionary(x => x.Location, x => x);

		StartCoord = Tiles.Where(x => x.Value.Description == 'S').Select(x => x.Value.Location).Single();
	}

	public override string HandlePart1()
	{
		var currentCoord = StartCoord;
		var connectedCoords = Tiles.GetValueOrDefault(currentCoord).ConnectedLocationCoords
			.Where(x => Tiles.GetValueOrDefault(x)?.ConnectedLocationCoords.Contains(currentCoord) == true).ToList();
		var coordsInLoop = new List<Coord>() { currentCoord };
		currentCoord = connectedCoords.First();
		coordsInLoop.Add(currentCoord);
		while (!currentCoord.Equals(StartCoord))
		{
			var currentTile = Tiles.GetValueOrDefault(currentCoord);
			var newCoord = currentTile.GetNextLocationCoord(coordsInLoop[coordsInLoop.IndexOf(currentCoord)-1]);
			coordsInLoop.Add(newCoord);
			currentCoord = newCoord;
		}

		return ((coordsInLoop.Count - 1) / 2).ToString();
	}

	public override string HandlePart2()
	{
		var currentCoord = StartCoord;
		var connectedCoords = Tiles.GetValueOrDefault(currentCoord).ConnectedLocationCoords
			.Where(x => Tiles.GetValueOrDefault(x)?.ConnectedLocationCoords.Contains(currentCoord) == true).ToList();
		var coordsInLoop = new List<Coord>() { currentCoord };
		currentCoord = connectedCoords.First();
		coordsInLoop.Add(currentCoord);
		while (!currentCoord.Equals(StartCoord))
		{
			var currentTile = Tiles.GetValueOrDefault(currentCoord);
			var newCoord = currentTile.GetNextLocationCoord(coordsInLoop[coordsInLoop.IndexOf(currentCoord)-1]);
			coordsInLoop.Add(newCoord);
			currentCoord = newCoord;
		}

		foreach (var tile in Tiles.Where(x => !coordsInLoop.Contains(x.Key)))
		{
			tile.Value.Description = '.';
		}

		var startTile = Tiles.GetValueOrDefault(StartCoord);
		var tCoordConnected = startTile.ConnectedLocationCoords.Where(x => Tiles.GetValueOrDefault(x)?.ConnectedLocationCoords.Contains(currentCoord) == true).ToList();
		if (tCoordConnected.All(z => z.X == StartCoord.X)) startTile.Description = '|';
		if (tCoordConnected.All(z => z.Y == StartCoord.Y)) startTile.Description = '-';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y - 1) && tCoordConnected.Any(z => z.X == StartCoord.X - 1 && z.Y == StartCoord.Y)) startTile.Description = 'J';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y - 1) && tCoordConnected.Any(z => z.X == StartCoord.X + 1 && z.Y == StartCoord.Y)) startTile.Description = 'L';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y + 1) && tCoordConnected.Any(z => z.X == StartCoord.X + 1 && z.Y == StartCoord.Y)) startTile.Description = 'F';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y + 1) && tCoordConnected.Any(z => z.X == StartCoord.X - 1 && z.Y == StartCoord.Y)) startTile.Description = '7';


		var insideCount = 0;
		for (var y = 0; y <= Tiles.Select(x => x.Key).Max(x => x.Y); y++)
		{
			var row = Tiles.Where(x => x.Key.Y == y).ToList();

			for (var x = 0; x <= Tiles.Select(z => z.Key).Max(z => z.X); x++)
			{
				var tCoord = new Coord(x, y);
				var tile = Tiles.GetValueOrDefault(tCoord);
				if (tile.Description != '.') continue;
				
				var left = row.Where(z => z.Key.X < tCoord.X).ToList();
				var leftLines = left.Where(z => z.Value.Description == '|').ToList();
				var leftCorners = left.Where(z => z.Value.Description is 'J' or '7' or 'L' or 'F').ToList();
				var leftCount = 0;
				var leftHold = '.';
				foreach (var value in left.Select(t => t.Value))
				{
					switch (value.Description)
					{
						case '|':
							leftCount++;
							break;
						case 'L':
							if (leftHold == '.') leftHold = value.Description;
							break;
						case 'J':
							if (leftHold == '.') leftHold = value.Description;
							if (leftHold == 'L') leftHold = '.';
							else
							{
								leftHold = '.';
								leftCount++;
							}
							break;
						case 'F':
							if (leftHold == '.') leftHold = value.Description;
							break;
						case '7':
							if (leftHold == '.') leftHold = value.Description;
							if (leftHold == 'F') leftHold = '.';
							else
							{
								leftHold = '.';
								leftCount++;
							}
							break;
							
					}
				}
					
				// F7 + 0
				// LJ + 0
				if (
					(leftLines.Any() || leftCorners.Any()) && leftCount % 2 != 0
				)
					insideCount++;
			}
		}

		return insideCount.ToString();
	}
}

public class Tile
{
	public Coord Location { get; set; }
	public char Description { get; set; }

	public List<Coord> ConnectedLocationCoords => Description switch
	{
		'|' => new List<Coord>() { new(Location.X, Location.Y + 1), new(Location.X, Location.Y + -1) },
		'-' => new List<Coord>() { new(Location.X - 1, Location.Y), new(Location.X + 1, Location.Y) },
		'L' => new List<Coord>() { new(Location.X, Location.Y - 1), new(Location.X + 1, Location.Y) },
		'J' => new List<Coord>() { new(Location.X, Location.Y - 1), new(Location.X - 1, Location.Y) },
		'7' => new List<Coord>() { new(Location.X, Location.Y + 1), new(Location.X - 1, Location.Y) },
		'F' => new List<Coord>() { new(Location.X, Location.Y + 1), new(Location.X + 1, Location.Y) },
		'.' => new List<Coord>(),
		'S' => new List<Coord>()
		{
			new(this.Location.X, Location.Y + 1), new(this.Location.X, Location.Y - 1),
			new(this.Location.X + 1, Location.Y), new(this.Location.X - 1, Location.Y)
		},
	};

	public Coord GetNextLocationCoord(Coord from)
		=> ConnectedLocationCoords.Single(x => x.Y != from.Y || x.X != from.X);
}

public class Coord : IEquatable<Coord>
{
	public Coord(int x, int y)
	{
		X = x;
		Y = y;
	}
	public int X { get; set; }
	public int Y { get; set; }

	public bool Equals(Coord? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return X == other.X && Y == other.Y;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Coord)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}
}