namespace Days2023;

public class Day_07 : Day
{
	private List<Hand> hands;
	public Day_07()
	{
		Title = "Camel Cards";
		DayNumber = 7;
		Year = 2023;
	}
	
	public override void Gather_input()
	{
		hands = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Hand(x)).ToList();
	}

	public override string HandlePart1()
	{
		var OrderedHands = hands.OrderByDescending(x => x.Points);
		return OrderedHands.Select((x, index) => (OrderedHands.Count() - index) * x.Bid).Sum().ToString();
	}

	public override string HandlePart2()
	{
		var OrderedHands = hands.OrderByDescending(x => x.PointsPt2);
		return OrderedHands.Select((x, index) => (OrderedHands.Count() - index) * x.Bid).Sum().ToString();
	}
}

public class Hand
{
	public Hand(string input)
	{
		Bid = int.Parse(input.Split(" ").Last());
		Cards = input.Split(" ").First();
	}

	public int Bid { get; set; }
	public string Cards { get; set; }

	public long Points => long.Parse(TypeStrength.ToString() + string.Join("",
		this.Cards.Select(x => Extensions.GetStrengthOfLetter(x).ToString().PadLeft(2, '0'))));

	private int TypeStrength => Tuple.Create(Cards.GroupBy(x => x).Max(x => x.Count()),
			Cards.GroupBy(x => x).OrderByDescending(x => x.Count()).Skip(1).FirstOrDefault()?.Count() ?? 0) switch
		{
			(5, 0) => 7,
			(4, 1) => 6,
			(3, 2) => 5,
			(3, 1) => 4,
			(2, 2) => 3,
			(2, 1) => 2,
			_ => 1,
		};

	public long PointsPt2 => long.Parse(TypeStrengthPt2().ToString() + string.Join("",
		this.Cards.Select(x => Extensions.GetStrengthOfLetterPt2(x).ToString().PadLeft(2, '0'))));

	private int TypeStrengthPt2()
	{
		var possibleChanges = new List<char> { 'A','K','Q','T','9','8','7','6','5','4','3','2' };
		var highestType = int.MinValue;
		for (var j = 0; j < possibleChanges.Count(); j++)
		{
			var temp = Cards.Replace('J', possibleChanges[j]);
			var points = Tuple.Create(temp.GroupBy(x => x).Max(x => x.Count()),
					Cards.GroupBy(x => x).OrderByDescending(x => x.Count()).Skip(1).FirstOrDefault()?.Count() ??
					0) switch
				{
					(5, _) => 7,
					(4, _) => 6,
					(3, 2) => 5,
					(3, _) => 4,
					(2, 2) => 3,
					(2, _) => 2,
					_ => 1,
				};
			if (points > highestType) highestType = points;
		}
		return highestType;
	}
}

public static partial class Extensions
{
	public static int GetStrengthOfLetter(this char letter) => letter switch
	{
		'A' => 13,
		'K' => 12,
		'Q' => 11,
		'J' => 10,
		'T' => 9,
		'9' => 8,
		'8' => 7,
		'7' => 6,
		'6' => 5,
		'5' => 4,
		'4' => 3,
		'3' => 2,
		'2' => 1,
		_ => 0
	};
	
	public static int GetStrengthOfLetterPt2(this char letter) => letter switch
	{
		'A' => 13,
		'K' => 12,
		'Q' => 11,
		'T' => 10,
		'9' => 9,
		'8' => 8,
		'7' => 7,
		'6' => 6,
		'5' => 5,
		'4' => 4,
		'3' => 3,
		'2' => 2,
		'J' => 1,
		_ => 0
	};

}
