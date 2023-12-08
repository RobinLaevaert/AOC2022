namespace Days2022;

public class Day_08 : Day
{
	public List<Tree> Trees;
	public Day_08()
	{
		Title = "Treetop Tree House";
		DayNumber = 8;
		Year = 2022;
	}

	public override void Gather_input()
	{
		Trees = new List<Tree>();
		Trees = Input.SelectMany((line, y) => line.Select((tree, x) => new Tree()
		{
			X = x,
			Y = y,
			Height = tree - '0'
		})).ToList();
	}

	public override string HandlePart1()
	{
		return Trees.Count(tree =>
		{
			var treesInSameRow = Trees.Where(y => y.Y == tree.Y);
			var treesInSameColumn = Trees.Where(y => y.X == tree.X);

			return treesInSameRow.Where(x => x.X > tree.X).All(x => x.Height < tree.Height) ||
			       treesInSameRow.Where(x => x.X < tree.X).All(x => x.Height < tree.Height) ||
			       treesInSameColumn.Where(x => x.Y < tree.Y).All(x => x.Height < tree.Height) ||
			       treesInSameColumn.Where(x => x.Y > tree.Y).All(x => x.Height < tree.Height);

		}).ToString();
	}

	public override string HandlePart2()
	{
		return Trees.Max(tree =>
		{
			var treesInSameRow = Trees.Where(y => y.Y == tree.Y).ToList();
			var treesInSameColumn = Trees.Where(y => y.X == tree.X).ToList();

			var left = treesInSameRow.Where(x => x.X < tree.X).OrderByDescending(x => x.X).ToList();
			var right = treesInSameRow.Where(x => x.X > tree.X).OrderBy(x => x.X).ToList();
			var top = treesInSameColumn.Where(x => x.Y < tree.Y).OrderByDescending(x => x.Y).ToList();
			var bottom = treesInSameColumn.Where(x => x.Y > tree.Y).OrderBy(x => x.Y).ToList();

			return Counter(left, tree) * Counter(right, tree) * Counter(top, tree) * Counter(bottom, tree);
		}).ToString();

		int Counter (List<Tree> trees, Tree originalHeight)
		{
			var barrierTree = trees.FirstOrDefault(y => y.Height >= originalHeight.Height);
			return trees
				.Where((x, index) => barrierTree == null || index <= trees.IndexOf(barrierTree))
				.Count();
		}
	}
}

public class Tree
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Height { get; set; }
	public int ScenicScore { get; set; }
}
