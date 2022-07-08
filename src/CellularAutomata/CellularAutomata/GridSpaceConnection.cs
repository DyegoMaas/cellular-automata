namespace CellularAutomata;

public class GridSpaceConnection
{
    public GridSpace TargetSpace { get; }
    public Direction Direction { get; }
    
    private GridSpaceConnection(GridSpace targetSpace, Direction direction)
    {
        TargetSpace = targetSpace;
        Direction = direction;
    }

    public static void Connect(GridSpace origin, GridSpace to, Direction direction)
    {
        var connectionA = new GridSpaceConnection(to, direction);

        var inverseDirection = new Direction(direction.X * -1, direction.Y * -1);
        var connectionB = new GridSpaceConnection(origin, inverseDirection);

        origin.AddConnection(connectionA);
        to.AddConnection(connectionB);
    }
}