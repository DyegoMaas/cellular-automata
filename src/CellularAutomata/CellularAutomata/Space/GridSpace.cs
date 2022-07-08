using CellularAutomata.Cells;

namespace CellularAutomata.Space;

public class GridSpace
{
    public GridSpace(Cell cell)
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
    
    protected bool Equals(GridSpace other)
    {
        return Identifier.Equals(other.Identifier);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GridSpace) obj);
    }

    public override int GetHashCode()
    {
        return Identifier.GetHashCode();
    }

    public static bool operator ==(GridSpace spaceA, GridSpace spaceB)
    {
        return spaceA.Identifier == spaceB.Identifier;
    } 
    
    public static bool operator !=(GridSpace spaceA, GridSpace spaceB)
    {
        return spaceA.Identifier != spaceB.Identifier;
    }

    public override string ToString()
    {
        return Identifier.ToString();
    }
}