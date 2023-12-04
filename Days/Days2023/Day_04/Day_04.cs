using Shared;

namespace Days2023;

public class Day_04 : Day
{
    public List<Card> Cards;
    public List<int> CardCounts;
    public Day_04()
    {
        Title = "Scratchcards";
        DayNumber = 4;
        Year = 2023;
    }
    public override void Gather_input()
    {
        Cards = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x =>
            new Card()
            {
                Number = Convert.ToInt32(x.Split(":").First().Split(" ").Last().Trim()),
                WinningNumbers = x.Split(":").Last().Split("|").First().Trim().Split(" ")
                    .Where(y => !string.IsNullOrWhiteSpace(y)).Select(int.Parse).ToList(),
                PlayerNumbers = x.Split(":").Last().Split("|").Last().Trim().Split(" ")
                    .Where(y => !string.IsNullOrWhiteSpace(y)).Select(int.Parse).ToList(),
            }
        ).ToList();
        CardCounts = Input.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => 1).ToList();
    }

    public override string HandlePart1()
    {
        return Cards.Sum(x => x.points).ToString();
    }

    public override string HandlePart2()
    {
        for (var i = 0; i < Cards.Count-1; i++)
        {
            var card = Cards[i];
            for (var j = 0; j < card.WinningPlayerNumbers.Count; j++)
            {
                if(i + 1 + j >= Cards.Count) continue;
                CardCounts[i + 1 + j] += CardCounts[i];
            }
        }

        return CardCounts.Sum().ToString();
    }
}

public class Card
{
    public int Number { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> PlayerNumbers { get; set; }
    public List<int> WinningPlayerNumbers => PlayerNumbers.Where(x => WinningNumbers.Contains(x)).ToList();
    public int points => Convert.ToInt32(Math.Pow(2, WinningPlayerNumbers.Count - 1));
}