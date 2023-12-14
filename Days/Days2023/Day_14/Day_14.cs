namespace Days2023;

public class Day_14 : Day
{
	public Day_14()
	{
		Title = "Parabolic Reflector Dish";
		DayNumber = 14;
		Year = 2023;
	}

	public List<string> Spaces;

	public override void Gather_input()
	{
		Spaces = Input.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
	}

	public override string HandlePart1()
	{
		TiltNorth(Spaces);
		return Spaces.Select((x, index) => (Spaces.Count -index) * x.Count(y => y == 'O')).Sum().ToString();
	}

	public override string HandlePart2()
	{
		var dict = new Dictionary<string, long>();
		var counter = 0l;
		var resp = new List<string>();
		
		while (counter < 1000000000)
		{
			counter++;
			TiltNorth(Spaces);
			TiltWest(Spaces);
			TilthSouth(Spaces);
			TiltEast(Spaces);

			if (dict.ContainsKey(string.Join(' ', Spaces.ToList())))
			{
				var t = dict.GetValueOrDefault(string.Join(' ', Spaces));
				var mod = (1000000000- t) % (counter - t);
				resp = dict.Single(x => x.Value ==  t + mod).Key.Split(' ').ToList();
				break;
			}
			dict.Add(string.Join(' ', Spaces.ToList()), counter);
		}
		
		return resp.Select((x, index) => (resp.Count -index) * x.Count(y => y == 'O')).Sum().ToString();
	}
	
	
	private static void TiltNorth(List<string> r)
	{
		for (var y = 0; y < r.Count; y++)
		{
			for (var x = 0; x < r.First().Length; x++)
			{
				if (r[y][x] == 'O')
				{
					var j = 0;
					for (j = y; j > 0; j --)
						if (r[j-1][x] != '.')
							break;
					r[y] = r[y].Remove(x,1).Insert(x, ".");
					r[j] = r[j].Remove(x,1).Insert(x, "O");
				}
			}
		}
	}
	
	private static void TiltWest(List<string> r)
	{
		for (var y = 0; y < r.Count; y++)
		{
			for (var x = 0; x < r.First().Length; x++)
			{
				if (r[y][x] == 'O')
				{
					var j = 0;
					for (j = x; j > 0; j --)
						if (r[y][j-1] != '.')
							break;
					
					r[y] = r[y].Remove(x,1).Insert(x, ".");
					r[y] = r[y].Remove(j,1).Insert(j, "O");
				}
			}
		}
	}
	
	private static void TiltEast(List<string> r)
	{
		for (var y = 0; y < r.Count; y++)
		{
			for (var x = r.First().Length-1; x >= 0; x--)
			{
				if (r[y][x] == 'O')
				{
					var j = 0;
					for (j = x; j < (r.First().Length -1); j ++)
						if (r[y][j+1] != '.')
							break;
					
					r[y] = r[y].Remove(x,1).Insert(x, ".");
					r[y] = r[y].Remove(j,1).Insert(j, "O");
				}
			}
		}
	}
	
	private static void TilthSouth(List<string> r)
	{
		for (var y = r.Count-1; y >= 0; y--)
		{
			for (var x = 0; x < r.First().Length; x++)
			{
				if (r[y][x] == 'O')
				{
					var j = 0;
					for (j = y; j < r.Count-1; j ++)
						if (r[j+1][x] != '.')
							break;
					r[y] = r[y].Remove(x,1).Insert(x, ".");
					r[j] = r[j].Remove(x,1).Insert(x, "O");
				}
			}
		}
	}

}