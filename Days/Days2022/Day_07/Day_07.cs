namespace Days2022;

public class Day_07 : Day
{
	public List<Item> Tree { get; set; }
	public Day_07()
	{
		Title = "No Space Left On Device";
		DayNumber = 7;
		Year = 2022;
	}

	public override void Gather_input()
	{
		Tree = new List<Item>();
		Tree.Add(new Dir("/"));
		var currentPath = new List<string>() { };
		Dir currentDir = Tree.Single() as Dir;
		foreach (var line in Input.Where(x => !string.IsNullOrWhiteSpace(x)))
		{
			if (line.StartsWith($"$"))
			{
				if (line.StartsWith($"$ cd "))
				{
					var path = line.Split("$ cd ").Last().Trim();
					if (path == "/")
					{
						currentDir = Tree.Single() as Dir;
						currentPath = new List<string>() { "/" };
					}
					else if (path == "..")
					{
						currentPath = currentPath.ToArray()[0..^1].ToList();
						currentDir = Tree.Single() as Dir;
						foreach (var p in currentPath)
						{
							if (p == "/") continue;
							currentDir = currentDir.SubItems.Single(x => x.Name == p) as Dir;
						}
					}
					else
					{
						currentPath.Add(path);
						if(!currentDir.SubItems.Any(y => y.Name == path))
							currentDir.SubItems.Add(new Dir(path));

						currentDir = currentDir.SubItems.Single(y => y.Name == path) as Dir;

					}
				}
			}
			else
			{
				if (line == "ls") continue;
				if (line.StartsWith("dir"))
				{
					var path = line.Split("dir").Last().Trim();
					if(!currentDir.SubItems.Any(y => y.Name == path))
						currentDir.SubItems.Add(new Dir(path));
				}
				else
				{
					var item = line.Split(" ");
					currentDir.SubItems.Add(new File(long.Parse(item[0]), item[1]));
				}
			}
		}
	}

	public override string HandlePart1()
	{
		return GetDirSizesUnder(Tree.Single() as Dir, 100000).Sum().ToString();
	}

	public override string HandlePart2()
	{
		var sizeNeedToBeRemoved = 30000000 - (70000000 - Tree.Single().Size);
		var dirSizes = GetDirSizesOver(Tree.Single() as Dir, sizeNeedToBeRemoved);
		return dirSizes.Min().ToString();
	}

	public List<long> GetDirSizesUnder(Dir dir, long limit)
	{
		return dir.SubItems.Where(x => x is Dir dir1 && dir1.Size <= limit).Select(x => x.Size)
			.Concat(dir.SubItems.Where(x => x is Dir).SelectMany(x => GetDirSizesUnder(x as Dir, limit))).ToList();
	}
	
	public List<long> GetDirSizesOver(Dir dir, long limit)
	{
		return dir.SubItems.Where(x => x is Dir dir && dir.Size>= limit).Select(x => x.Size)
			.Concat(dir.SubItems.Where(x => x is Dir).SelectMany(x => GetDirSizesOver(x as Dir, limit))).ToList();
	}
}

public abstract class Item
{
	public string Name { get; set; }
	public abstract long Size { get; }
}

public class Dir : Item
{
	public Dir(string name)
	{
		Name = name;
		SubItems = new List<Item>();
	}
	public List<Item> SubItems { get; set; }
	public override long Size => SubItems.Sum(x => x.Size);
}

public class File : Item
{
	public File(long size, string name)
	{
		Name = name;
		Size = size;
	}
	public override long Size { get; }
}

