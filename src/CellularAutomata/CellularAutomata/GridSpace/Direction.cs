namespace CellularAutomata.GridSpace;

public record Direction
{
    public Direction(int x, int y)
    {
        if (x == 0 && y == 0)
            throw new InvalidDirectionException("Direction cannot be 0,0");
        
        X = x;
        Y = y;
    }

    public int Y { get; }

    public int X { get; }

    public Direction Opposite() => new(X * -1, Y * -1);
}