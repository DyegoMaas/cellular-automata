namespace CellularAutomata.GridSpace;

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

        var oppositeDirection = direction.Opposite();
        var connectionB = new GridSpaceConnection(origin, oppositeDirection);

        origin.AddConnection(connectionA);
        to.AddConnection(connectionB);
    }
}