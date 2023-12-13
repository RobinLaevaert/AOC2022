namespace Days2023;

public class Day_13 : Day
{
	private List<Pattern> Patterns;
	public Day_13()
	{
		Title = "Point of Incidence";
		DayNumber = 13;
		Year = 2023;
	}

	public override void Gather_input()
	{
		Patterns = new List<Pattern>();
		var temp = new Pattern();
		foreach (var line in Input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				Patterns.Add(temp);
				temp = new Pattern();
				continue;
			}

			temp.Lines.Add(line);
		}

		if (temp.Lines.Any()) Patterns.Add(temp);
	}

	public override string HandlePart1()
	{
		var points = 0l;
		var patternIndex = 0;
		foreach (var pattern in Patterns)
		{
			var possibleLineSplits = pattern.Lines.Aggregate(Enumerable.Range(0, pattern.Lines.First().Length).ToList(), (current, line) => PossibleLineSplits(current, line));

			if (possibleLineSplits.Count == 1)
			{
				points += possibleLineSplits.First();
				continue;
			}
			
			var possibleColumnSplits = Enumerable.Range(0, pattern.Lines.Count).ToList();
			for (var i = 0; i < pattern.Lines.First().Length; i++)
			{
				if (!possibleColumnSplits.Any()) break;
				
				var str = pattern.Lines.Aggregate("", (current, line) => current + line[i]);
				
				possibleColumnSplits = PossibleLineSplits(possibleColumnSplits, str);
			}
			if (possibleColumnSplits.Count == 1)
			{
				points += possibleColumnSplits.First() * 100;
			}
		}

		return points.ToString();
	}
	
	public override string HandlePart2()
	{
		var points = 0l;
		foreach (var pattern in Patterns)
		{
			var ogPatternPoints = 0;
			var possibleLineSplits1 = pattern.Lines.Aggregate(
				Enumerable.Range(0, pattern.Lines.First().Length).ToList(),
				(current, line) => PossibleLineSplits(current, line));

			if (possibleLineSplits1.Count == 1)
				ogPatternPoints += possibleLineSplits1.First();

			if (ogPatternPoints == 0)
			{
				var possibleColumnSplits1 = Enumerable.Range(0, pattern.Lines.Count).ToList();
				for (var i = 0; i < pattern.Lines.First().Length; i++)
				{
					if (!possibleColumnSplits1.Any()) continue;
				
					var str = pattern.Lines.Aggregate("", (current, line) => current + line[i]);
				
					possibleColumnSplits1 = PossibleLineSplits(possibleColumnSplits1, str);
				}
				if (possibleColumnSplits1.Count == 1)
				{
					ogPatternPoints += possibleColumnSplits1.First() * 100;
				}
			}
			
			
			var currPatternPoints = 0;
			for (var p = 0; p < (pattern.Lines.Count * pattern.Lines.First().Length); p++)
			{
				if (currPatternPoints != 0) break;
				
				var curPattern = SmudgePattern(pattern, p);
				
				var possibleLineSplits = curPattern.Aggregate(Enumerable.Range(0, curPattern.First().Length).ToList(),
					(current, line) => PossibleLineSplits(current, line));
				
				if (possibleLineSplits.Count(x => x != ogPatternPoints) == 1)
				{
					currPatternPoints = possibleLineSplits.First(x => x != ogPatternPoints);
					continue;
				}
			
				var possibleColumnSplits = Enumerable.Range(0, curPattern.Count).ToList();
				for (var i = 0; i < curPattern.First().Length; i++)
				{
					if (!possibleColumnSplits.Any()) break;
				
					var str = curPattern.Aggregate("", (current, line) => current + line[i]);

					possibleColumnSplits = PossibleLineSplits(possibleColumnSplits, str);
				}

				if (possibleColumnSplits.Count(x => x*100 != ogPatternPoints) == 1)
				{
					currPatternPoints += possibleColumnSplits.First(x => x*100 != ogPatternPoints) * 100;
				}
			}

			if (currPatternPoints != 0)
			{
				points += currPatternPoints;
			}
		}

		return points.ToString();
	}
	
	private static List<int> PossibleLineSplits(List<int> possibleLineSplits, string line)
	{
		return possibleLineSplits.Where((x,i) =>
		{
			var left = new string(line[..possibleLineSplits[i]].Reverse().ToArray());
			var right = line[possibleLineSplits[i]..];

			var isMirrorPossible = left.Any() && right.Any();
			var smallestLength = Math.Min(left.Length, right.Length);
			isMirrorPossible &= left[..smallestLength].SequenceEqual(right[..smallestLength]);
			return isMirrorPossible;
		}).ToList();
	}

	private static List<string> SmudgePattern(Pattern pattern, int p)
	{
		var curPattern = pattern.Lines.ToList();

		var lineNumber = Convert.ToInt32(Math.Floor((decimal)p/curPattern.First().Length));
		var charNumber = (p % curPattern.First().Length);
		var charToSwitch = curPattern[lineNumber][charNumber];
		curPattern[lineNumber] = curPattern[lineNumber].Remove(charNumber, 1)
			.Insert(charNumber, charToSwitch == '#' ? "." : "#");
		return curPattern;
	}
}

public class Pattern
{
	public List<string> Lines { get; set; } = new List<string>();
}