namespace Days2024;

public class Day_06 : Day
{
    public Day_06()
    {
        Title = "Guard Gallivant";
        DayNumber = 6;
        Year = 2024;
    }

    private List<MapCoordinate> mapCoordinates;
    public override void Gather_input()
    {
        var t =
            Input.SelectMany((line, y) => line.Select((letter, x) => new MapCoordinate(letter, x, y))).ToList();
        mapCoordinates = t.Select(y =>
        {
            y.Neighbours = t.Where(z =>
                (z.X - 1 == y.X || y.X == z.X + 1 || y.X == z.X) &&
                (z.Y - 1 == y.Y || y.Y == z.Y + 1 || y.Y == z.Y) &&
                (z.Y != y.Y || z.X != y.X)).ToList();
            return y;
        }).ToList();
    }

    public override string HandlePart1()
    {
        var passedCoordinates = new List<MapCoordinate>();
        var isOutside = false;
        var currentPos = mapCoordinates.Single(x => x.IsStart);
        passedCoordinates.Add(currentPos);
        var direction = currentPos.Letter switch
        {
            '^' => Direction.North,
            'v' => Direction.South,
            '<' => Direction.West,
            '>' => Direction.East,
            _ => throw new NotImplementedException()
        };
        while (!isOutside)
        {
            var nextNeighbour = currentPos.GetNeighbour(direction);
            while (nextNeighbour is { IsObstructions: false, IsEdge: false })
            {
                currentPos = nextNeighbour;
                nextNeighbour = currentPos.GetNeighbour(direction);
                passedCoordinates.Add(currentPos);
            }

            if (nextNeighbour.IsObstructions)
            {
                direction = direction switch
                {
                    Direction.North => Direction.East,
                    Direction.East => Direction.South,
                    Direction.South => Direction.West,
                    Direction.West => Direction.North,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (nextNeighbour.IsEdge)
            {
                isOutside = true;
            }
        }
        return passedCoordinates.DistinctBy(x => new {x.X, x.Y}).Count().ToString();
    }

    public override string HandlePart2()
    {
        var passedCoordinates = new List<Tuple<MapCoordinate,Direction>>();
        var potentialLoopingObstructions = new List<MapCoordinate>();
        var isOutside = false;
        var currentPos = mapCoordinates.Single(x => x.IsStart);
        var direction = currentPos.Letter switch
        {
            '^' => Direction.North,
            'v' => Direction.South,
            '<' => Direction.West,
            '>' => Direction.East,
            _ => throw new NotImplementedException()
        };
        passedCoordinates.Add(Tuple.Create(currentPos, direction));
        while (!isOutside)
        {
            var nextNeighbour = currentPos.GetNeighbour(direction);
            while (nextNeighbour is { IsObstructions: false, IsEdge: false })
            {
                var newDirection = RotateDirection(direction);
                var nextPotentialCoordinate = currentPos.GetNeighbour(newDirection);
                // This is too simple, can get in a loop even if the next neigbour isn't in the loop, could be in the loop later down the road.
                if (passedCoordinates.Any(x =>
                        x.Item2 == newDirection && x.Item1.X == nextPotentialCoordinate.X &&
                        x.Item1.Y == nextPotentialCoordinate.Y))
                {
                    potentialLoopingObstructions.Add(nextNeighbour);
                }
                else
                {
                    var steps = 0;
                    var tempCurrentPos = nextPotentialCoordinate;
                    var tempDirection = newDirection;
                    var tempPassedCoordinates = passedCoordinates.ToList();
                    tempPassedCoordinates.Add(Tuple.Create(tempCurrentPos, tempDirection));
                    var isLoop = false;
                    while (steps <= 20000)
                    {
                        var tempNextNeighbour = tempCurrentPos.GetNeighbour(tempDirection);
                        if (tempPassedCoordinates.Any(x => x.Item2 == tempDirection && x.Item1.X == tempNextNeighbour.X &&
                                                           x.Item1.Y == tempNextNeighbour.Y))
                        {
                            isLoop = true;
                            break;
                        }

                        tempCurrentPos = tempNextNeighbour;
                        tempNextNeighbour = tempCurrentPos.GetNeighbour(tempDirection);
                        tempPassedCoordinates.Add(Tuple.Create(tempCurrentPos, tempDirection));

                        if (tempNextNeighbour.IsObstructions)
                            tempDirection = RotateDirection(tempDirection);

                        if (tempNextNeighbour.IsEdge)
                            break;
                        steps++;
                    }
                    if(isLoop)
                        potentialLoopingObstructions.Add(nextNeighbour);
                }
                
                currentPos = nextNeighbour;
                nextNeighbour = currentPos.GetNeighbour(direction);
                passedCoordinates.Add(Tuple.Create(currentPos, direction));
            }

            if (nextNeighbour.IsObstructions)
            {
                direction = RotateDirection(direction);
            }

            if (nextNeighbour.IsEdge)
            {
                isOutside = true;
            }
        }

        return potentialLoopingObstructions
            .Where(x => !mapCoordinates.Single(y => y.X == x.X && y.Y == x.Y).IsObstructions)
            .Where(x => !x.IsStart)
            .DistinctBy(x => new{x.X, x.Y}).Count().ToString();
    }

    private Direction RotateDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
public class MapCoordinate
{
    public MapCoordinate(){}
    public MapCoordinate(char letter, int x, int y)
    {
        Letter = letter;
        X = x;
        Y = y;
        IsObstructions = letter == '#';
        IsStart = letter is '^' or '>' or '<' or 'v';
    }
    public MapCoordinate Edge()
    {
        return new MapCoordinate
        {
            IsEdge = true
        };
    }
    public char Letter { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsStart { get; set; }
    public bool IsObstructions { get; set; }
    public bool IsEdge { get; set; }
    public List<MapCoordinate> Neighbours = new List<MapCoordinate>();
    public MapCoordinate GetNeighbour(Direction direction)
    {
        var temp =  direction switch
        {
            Direction.North => Neighbours.SingleOrDefault(x => x.X == X && x.Y == Y - 1),
            Direction.East => Neighbours.SingleOrDefault(x => x.X == X + 1 && x.Y == Y),
            Direction.South => Neighbours.SingleOrDefault(x => x.X == X && x.Y == Y + 1),
            Direction.West => Neighbours.SingleOrDefault(x => x.X == X - 1 && x.Y == Y),
            _ => Edge()
        };
        return temp ?? Edge();
    }
}

public enum Direction
{
    North,
    East,
    South,
    West,
}