namespace Days2023;

public class Day_15 : Day
{
	public List<string> Steps;
	public Day_15()
	{
		Title = "Lens Library";
		DayNumber = 15;
		Year = 2023;
	}

	public override void Gather_input()
	{
		Steps = Input.First(x => !string.IsNullOrWhiteSpace(x)).Split(',').ToList();
	}

	public override string HandlePart1()
	{
		return Steps.Select(x => Helper.Hash(0,x)).Sum().ToString();
	}

	public override string HandlePart2()
	{
		var boxes = Enumerable.Range(0, 256).Select(x => new Box(x)).ToList();

		foreach (var step in Steps)
		{
			var lens = new Lens(step);
			
			var box = boxes.Single(x => x.Number == lens.BoxNumber);
			if (step.Contains('=')) box.Add(lens);
			else box.Remove(lens);
		}

		return boxes.Where(x => x.Lenses.Any())
			.Select(x => x.Lenses.Select((y, index) => (x.Number + 1) * (index + 1) * y.FocalLength).Sum()).Sum()
			.ToString()!;
	}
}

public class Box
{
	public Box(int number)
	{
		Number = number;
		Lenses = new();
	}
	public int Number { get; set; }
	public List<Lens> Lenses { get; set; }

	public void Add(Lens lens)
	{
		var existingLens = Lenses.FirstOrDefault(x => x.Label == lens.Label);
		if (existingLens != null) existingLens.FocalLength = lens.FocalLength;
		else Lenses.Add(lens);
	}

	public void Remove(Lens lens)
	{
		var lensToRemove = Lenses.Where(x => x.Label == lens.Label).ToList();
		if (lensToRemove.Any())
			Lenses.Remove(lensToRemove.First());
	}
}

public class Lens
{
	public Lens(string input)
	{
		Label = new string(input.TakeWhile(x => x is not '=' and not '-').ToArray());
		if (input.Contains('=')) FocalLength = int.Parse(input.Split('=').Last());
		
		BoxNumber = Helper.Hash(0, Label);
	}
	public string Label { get; set; }
	public int? FocalLength { get; set; }
	public int BoxNumber { get; set; }
}

public static class Helper
{
	public static int Hash(int seed, string inp)
	{
		var curValue = seed;
		foreach (var ch in inp)
		{
			curValue += (int)ch;
			curValue *= 17;
			curValue %= 256;
		}

		return curValue;
	}
}