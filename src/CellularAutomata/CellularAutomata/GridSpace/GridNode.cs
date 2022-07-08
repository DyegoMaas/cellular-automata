using CellularAutomata.Cells;

namespace CellularAutomata.GridSpace;

public class GridNode
{
    public GridNode(Cell cell)
    {
        Cell = cell;
        Identifier = Guid.NewGuid();
    }

    private readonly List<GridSpaceConnection> _connections = new();

    public Guid Identifier { get; }
    public Cell Cell { get; private set; }
    public IEnumerable<GridSpaceConnection> Connections => _connections;

    public void ReplaceCell(Cell newCell)
    {
        Cell = newCell;
    }

    public void AddConnection(GridSpaceConnection connection)
    {
        _connections.Add(connection);
    }
    
    protected bool Equals(GridNode other)
    {
        return Identifier.Equals(other.Identifier);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GridNode) obj);
    }

    public override int GetHashCode()
    {
        return Identifier.GetHashCode();
    }

    public static bool operator ==(GridNode nodeA, GridNode nodeB)
    {
        return nodeA.Identifier == nodeB.Identifier;
    } 
    
    public static bool operator !=(GridNode nodeA, GridNode nodeB)
    {
        return nodeA.Identifier != nodeB.Identifier;
    }

    public override string ToString()
    {
        return Identifier.ToString();
    }
}