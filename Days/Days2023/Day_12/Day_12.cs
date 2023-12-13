using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Days2023;


public class Day_12 : Day
{
	private List<Tuple<string, int[]>> lines;

	public Day_12()
	{
		Title = "Hot Springs";
		DayNumber = 12;
		Year = 2023;
	}

	public override void Gather_input()
	{
		lines = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x =>
				Tuple.Create(
					x.Split(' ').First(),
					x.Split(' ').Last()
						.Split(',')
						.Select(int.Parse)
						.ToArray()))
			.ToList();
	}

	public override string HandlePart1()
	{
		var CorrectCounters = new ConcurrentBag<int>();

		Parallel.ForEach(lines, line =>
		{
			var combinations = Combinations(line.Item1);
			var combinationsEvalued =
				combinations.Select(x => x.Split('.').Select(c => c.Length).Where(c => c != 0).ToArray())
					.Where(x => x.SequenceEqual(line.Item2));
			CorrectCounters.Add(combinationsEvalued.Count());
		});

		return CorrectCounters.Sum().ToString();
	}

	public static IEnumerable<string> Combinations(string input)
	{
		var replacementChars = "#.";

		var sets = input.Select(ch => ch == '?' ? replacementChars : ch.ToString());

		return Combine(sets).Select(x => new string(x.ToArray()));
	}

	public static IEnumerable<IEnumerable<T>> Combine<T>(IEnumerable<IEnumerable<T>> sequences)
	{
		IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

		return sequences.Aggregate(
			emptyProduct,
			(accumulator, sequence) =>
				from accseq in accumulator
				from item in sequence
				select accseq.Concat(new[] { item }));
	}

	public override string HandlePart2()
	{
		var CorrectCounters = new ConcurrentBag<long>();

		Parallel.ForEach(lines, line =>
		{
			CorrectCounters.Add(CalculateRow(string.Join('?', Enumerable.Range(0, 5).Select(x => line.Item1)),
				ImmutableStack.CreateRange(Enumerable.Range(0, 5).Select(x => line.Item2).SelectMany(x => x).Reverse()),
				new Dictionary<State, long>()));
		});

		return CorrectCounters.Sum().ToString();
	}


	private long CalculateRow(string row, ImmutableStack<int> nums,
		Dictionary<State, long> cache)
	{
		if (!cache.ContainsKey(new (row, nums)))
		{
			cache[new(row, nums)] = row.FirstOrDefault() switch
			{
				// Operational => remove dot and compute again
				'.' => CalculateRow(row[1..], nums, cache),
				// Handle as if it's both . and # if ?
				'?' => CalculateRow("." + row[1..], nums, cache) + CalculateRow("#" + row[1..], nums, cache),
				'#' => ActuallyDoSomething(row, nums, cache),
				// If any numbers left => Not correct; return 0 else correct and return 1
				_ => nums.Any() ? 0 : 1,
			};
		}

		return cache[new(row, nums)];
	}

	private long ActuallyDoSomething(string row, ImmutableStack<int> nums,
		Dictionary<State, long> cache)
	{
		// No number left even tho we have a damaged spring => not correct
		if (!nums.Any()) return 0;

		var nextNumber = nums.Peek();
		nums = nums.Pop();

		var maximumPossibleDeadSprings = row.TakeWhile(s => s is '#' or '?').Count();

		// number = 5, first part of row = #??# => false
		if (maximumPossibleDeadSprings < nextNumber) return 0;

		// End of the line
		if (row.Length == nextNumber) return CalculateRow("", nums, cache);

		// number = 5, first part of row = ##???#??# => false, number can never be 5 (either 3,4 or 6+)
		if (row[nextNumber] == '#') return 0;

		return CalculateRow(row[(nextNumber + 1)..], nums, cache);
	}
}

public class State : IEquatable<State>
{
	public State(string row, ImmutableStack<int> validator)
	{
		Row = row;
		Validator = validator;
	}
	public string Row { get; set; }
	public ImmutableStack<int> Validator { get; set; }

	public bool Equals(State? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Row == other.Row && Validator.Equals(other.Validator);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((State)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Row, Validator);
	}
}
