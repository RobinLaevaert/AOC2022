namespace Days2023;

public class Day_09 : Day
{
	public List<List<long>> sequences;
	public Day_09()
	{
		Title = "Mirage Maintenance";
		DayNumber = 9;
		Year = 2023;
	}

	public override void Gather_input()
	{
		sequences = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Split(' ').Select(long.Parse).ToList())
			.ToList();
	}

	public override string HandlePart1()
	{
		var newNumbers = new List<long>();
		foreach (var seq in sequences)
		{
			var curSeq = seq;
			var seqs = new List<List<long>>() { seq };
			
			while (curSeq.Any(x => x != 0))
			{
				curSeq = curSeq.Zip(curSeq.Skip(1), (a, b) => b - a).ToList();
				seqs.Add(curSeq);
			}

			seqs.Reverse();
			for (var i = 0; i < seqs.Count; i++)
			{
				if(i == 0) seqs[i].Add(seqs[i].Last());
				else
				{
					seqs[i].Add(seqs[i].Last() + seqs[i - 1].Last());
				}
			}
			seqs.Reverse();
			newNumbers.Add(seqs.First().Last());
		}

		return newNumbers.Sum().ToString();
	}

	public override string HandlePart2()
	{
		var newNumbers = new List<long>();
		foreach (var seq in sequences)
		{
			var curSeq = seq;
			var seqs = new List<List<long>>() { seq };
			
			while (curSeq.Any(x => x != 0))
			{
				curSeq = curSeq.Zip(curSeq.Skip(1), (a, b) => b - a).ToList();
				seqs.Add(curSeq);
			}

			seqs.Reverse();
			for (var i = 0; i < seqs.Count; i++)
			{
				var tempSeq = seqs[i].ToList();
				tempSeq.Reverse();
				if(i == 0) tempSeq.Add(seqs[i].First());
				else
				{
					tempSeq.Add(tempSeq.Last() - seqs[i-1].First());
				}

				tempSeq.Reverse();
				seqs[i] = tempSeq;
			}
			seqs.Reverse();
			newNumbers.Add(seqs.First().First());
		}

		return newNumbers.Sum().ToString();
	}
}
