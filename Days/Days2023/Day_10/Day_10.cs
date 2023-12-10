using System.Drawing;

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
			var newCoord = currentTile.GetNextLocationCoord(coordsInLoop[coordsInLoop.IndexOf(currentCoord) - 1]);
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
			var newCoord = currentTile.GetNextLocationCoord(coordsInLoop[coordsInLoop.IndexOf(currentCoord) - 1]);
			coordsInLoop.Add(newCoord);
			currentCoord = newCoord;
		}

		foreach (var tile in Tiles.Where(x => !coordsInLoop.Contains(x.Key)))
		{
			tile.Value.Description = '.';
		}

		var startTile = Tiles.GetValueOrDefault(StartCoord);
		var tCoordConnected = startTile.ConnectedLocationCoords.Where(x =>
			Tiles.GetValueOrDefault(x)?.ConnectedLocationCoords.Contains(currentCoord) == true).ToList();
		if (tCoordConnected.All(z => z.X == StartCoord.X)) startTile.Description = '|';
		if (tCoordConnected.All(z => z.Y == StartCoord.Y)) startTile.Description = '-';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y - 1) &&
		    tCoordConnected.Any(z => z.X == StartCoord.X - 1 && z.Y == StartCoord.Y)) startTile.Description = 'J';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y - 1) &&
		    tCoordConnected.Any(z => z.X == StartCoord.X + 1 && z.Y == StartCoord.Y)) startTile.Description = 'L';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y + 1) &&
		    tCoordConnected.Any(z => z.X == StartCoord.X + 1 && z.Y == StartCoord.Y)) startTile.Description = 'F';
		if (tCoordConnected.Any(z => z.X == StartCoord.X && z.Y == StartCoord.Y + 1) &&
		    tCoordConnected.Any(z => z.X == StartCoord.X - 1 && z.Y == StartCoord.Y)) startTile.Description = '7';


		//var insideCoords = new List<Coord>();
		var insideCount = 0;
		for (var y = 0; y <= Tiles.Select(x => x.Key).Max(x => x.Y); y++)
		{
			var row = Tiles.Where(x => x.Key.Y == y).ToList();

			for (var x = 0; x <= Tiles.Select(z => z.Key).Max(z => z.X); x++)
			{
				var tCoord = new Coord(x, y);
				var tile = Tiles.GetValueOrDefault(tCoord);
				if (tile.Description != '.') continue;
				if (coordsInLoop.Contains(tile.Location)) continue;

				var left = row.Where(z => z.Key.X < tCoord.X).ToList();
				var leftLines = left.Where(z => z.Value.Description == '|').ToList();
				var leftCorners = left.Where(z => z.Value.Description is 'J' or '7' or 'L' or 'F').ToList();
				
				var Ls = leftCorners.Count(z => z.Value.Description == 'L');
				var Js = leftCorners.Count(z => z.Value.Description == 'J');
				var Fs = leftCorners.Count(z => z.Value.Description == 'F');
				var sevens = leftCorners.Count(z => z.Value.Description == '7');

				var temp = leftLines.Count + ((Fs + Ls + Js + sevens - (2 * Math.Min(Fs, sevens)) - (2 * Math.Min(Js, Ls)))/2);

				if (temp % 2 != 0)
				{
					//insideCoords.Add(tile.Location);
					insideCount++;
				}
			}
		}
		
		//Print(coordsInLoop, insideCoords);

		return insideCount.ToString();
	}

	private void Print(List<Coord> coordsInLoop, List<Coord> insideCoords)
	{
		for (var y = 0; y <= Tiles.Select(x => x.Key).Max(x => x.Y); y++)
		{
			for (var x = 0; x <= Tiles.Select(z => z.Key).Max(z => z.X); x++)
			{
				Console.ResetColor();
				if (coordsInLoop.Contains(new Coord(x, y))) Console.ForegroundColor = ConsoleColor.Green;
				if (insideCoords.Contains(new Coord(x, y))) Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(Tiles.GetValueOrDefault(new(x, y)).Description.ToPrintChar());
			}

			Console.WriteLine();
		}
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

public static partial class Extensions
{
	public static char ToPrintChar(this char ch)
		=> ch switch
		{
			'7' => '\u2510',
			'J' => '\u2518',
			'L' => '\u2514',
			'F' => '\u250c',
			_ => ch
		};
}