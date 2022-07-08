namespace CellularAutomata.Space;

public class GridSpaceConnection
{
    public GridNode TargetNode { get; }
    public Direction Direction { get; }
    
    private GridSpaceConnection(GridNode targetNode, Direction direction)
    {
        TargetNode = targetNode;
        Direction = direction;
    }

    public static void Connect(GridNode origin, GridNode to, Direction direction)
    {
        var connectionA = new GridSpaceConnection(to, direction);

        var inverseDirection = new Direction(direction.X * -1, direction.Y * -1);
        var connectionB = new GridSpaceConnection(origin, inverseDirection);

        origin.AddConnection(connectionA);
        to.AddConnection(connectionB);
    }
}