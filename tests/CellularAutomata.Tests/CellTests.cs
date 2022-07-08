using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CellularAutomata.Tests;

public class CellTests
{
    [Fact(DisplayName = "A cell should have a state")]
    public void ACellShouldHaveAState()
    {
        Cell cell = new(CellState.White);
        cell.State.Should().Be(CellState.White);
    }

    [Fact(DisplayName = "Changing the state produces a new T+1 cell")]
    public void ChangingCellStateShouldProduceANewCellWithThatState()
    {
        Cell cell = new(CellState.White);

        var newCell = cell with {State = CellState.Black};

        newCell.State.Should().Be(CellState.Black);
    }
}

public class GridTests
{
    [Fact(DisplayName = "A grid space should have an unique identifier")]
    public void AGridSpaceShouldHaveAnUniqueIdentifier()
    {
        var cell = new Cell(CellState.White);
        GridSpace space = new(cell);

        space.Identifier.Should().NotBeEmpty();
        space.Should().Be(space, "a space is unique by its Identifier");
    }
    
    [Fact(DisplayName = "Two grid spaces are always different")]
    public void TwoGridSpacesAreDifferent()
    {
        var cell = new Cell(CellState.White);
        GridSpace spaceA = new(cell);
        GridSpace spaceB = new(cell);

        spaceA.Should().NotBe(spaceB);
        spaceA.Identifier.Should().NotBe(spaceB.Identifier);
    }
    
    [Fact(DisplayName = "A grid space should contain a cell")]
    public void AGridSpaceShouldContainACell()
    {
        var cell = new Cell(CellState.White);
        GridSpace space = new(cell);

        space.Cell.Should().Be(cell);
    }

    [Fact(DisplayName = "A grid space can have its cell replaced by a new one")]
    public void ItShouldBePossibleToReplaceTheCellOccupyingASpace()
    {
        var oldCell = new Cell(CellState.White);
        GridSpace space = new(oldCell);

        var newCell = new Cell(CellState.Black);
        space.ReplaceCell(newCell);

        space.Cell.Should().Be(newCell);
    }

    [Fact(DisplayName = "A grid is formed by a graph of interconnected grid spaces")]
    public void AGridSpaceShouldBeConnectedToOtherGridSpaces()
    {
        var spaceA = NewGridSpace();
        var spaceB = NewGridSpace();

        GridSpaceConnection.Connect(spaceA, to: spaceB, new Direction(1, 0));

        IEnumerable<GridSpaceConnection> spacesConnectedToSpaceA = spaceA.Connections;
        spacesConnectedToSpaceA.First().TargetSpace.Should().Be(spaceB);
        
        IEnumerable<GridSpaceConnection> spacesConnectedToSpaceB = spaceB.Connections;
        spacesConnectedToSpaceB.First().TargetSpace.Should().Be(spaceA);
    }
    
    private static GridSpace NewGridSpace() => new GridSpace(new Cell(CellState.White));

    [Theory(DisplayName = "Each connection should have opposite directions")]
    [InlineData(1, 0, -1, 0)]
    [InlineData(0, 1, 0, -1)]
    public void ConnectionsShouldHaveOppositeDirections(int x, int y, int oppositeX, int oppositeY)
    {
        var spaceA = NewGridSpace();
        var spaceB = NewGridSpace();

        GridSpaceConnection.Connect(spaceA, to: spaceB, new Direction(x, y));
        
        var connectionFromAToB = spaceA.Connections.First();
        connectionFromAToB.Direction.Should().Be(new Direction(x, y));
        
        var connectionFromBToA = spaceB.Connections.First();
        connectionFromBToA.Direction.Should().Be(new Direction(oppositeX, oppositeY));
    }

    [Fact(DisplayName = "Direction should point to a direction")]
    public void ADirectionShouldPointSomewhere()
    {
        var instantiateInvalidDirection = () =>
        {
            var invalidDirection = new Direction(0, 0);
        };

        instantiateInvalidDirection.Should().Throw<InvalidDirectionException>().WithMessage("Direction cannot be 0,0");
    }

    [Fact(DisplayName = "A grid should be able to define a Moore's Neighborhood")]
    public void ItShouldBePossibleToModelAMooreNeighorhood()
    {
        var centralSpace = NewGridSpace();
        var leftSpace = NewGridSpace();
        var rightSpace = NewGridSpace();
        var topSpace = NewGridSpace();
        var bottomSpace = NewGridSpace();
        var topLeftSpace = NewGridSpace();
        var topRightSpace = NewGridSpace();
        var bottomLeftSpace = NewGridSpace();
        var bottomRightSpace = NewGridSpace();

        GridSpaceConnection.Connect(centralSpace, to: leftSpace, new Direction(-1, 0));
        GridSpaceConnection.Connect(centralSpace, to: rightSpace, new Direction(1, 0));
        GridSpaceConnection.Connect(centralSpace, to: topSpace, new Direction(0, -1));
        GridSpaceConnection.Connect(centralSpace, to: bottomSpace, new Direction(0, 1));
        GridSpaceConnection.Connect(centralSpace, to: topLeftSpace, new Direction(-1, -1));
        GridSpaceConnection.Connect(centralSpace, to: topRightSpace, new Direction(1, -1));
        GridSpaceConnection.Connect(centralSpace, to: bottomLeftSpace, new Direction(-1, 1));
        GridSpaceConnection.Connect(centralSpace, to: bottomRightSpace, new Direction(1, 1));
    
        var surroundingSpaces = centralSpace.Connections.Select(x => x.TargetSpace);
        surroundingSpaces.Should().Contain(new[]
        {
            leftSpace,
            rightSpace,
            topSpace,
            bottomSpace,
            topLeftSpace,
            topRightSpace,
            bottomLeftSpace,
            bottomRightSpace,
        });

        leftSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(1, 0));
        rightSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(-1, 0));
        topSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(0, 1));
        bottomSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(0, -1));
        topLeftSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(1, 1));
        topRightSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(-1, 1));
        bottomLeftSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(1, -1));
        bottomRightSpace.ShouldBeConnectedTo(centralSpace, inDirection: new Direction(-1, -1));
        
        centralSpace.ShouldBeConnectedTo(leftSpace, inDirection: new Direction(-1, 0));
        centralSpace.ShouldBeConnectedTo(rightSpace, inDirection: new Direction(1, 0));
        centralSpace.ShouldBeConnectedTo(topSpace, inDirection: new Direction(0, -1));
        centralSpace.ShouldBeConnectedTo(bottomSpace, inDirection: new Direction(0, 1));
        centralSpace.ShouldBeConnectedTo(topLeftSpace, inDirection: new Direction(-1, -1));
        centralSpace.ShouldBeConnectedTo(topRightSpace, inDirection: new Direction(1, -1));
        centralSpace.ShouldBeConnectedTo(bottomLeftSpace, inDirection: new Direction(-1, 1));
        centralSpace.ShouldBeConnectedTo(bottomRightSpace, inDirection: new Direction(1, 1));
    }
}

public static class GridSpaceTestExtensions
{
    public static void ShouldBeConnectedTo(this GridSpace gridSpace, GridSpace otherSpace, Direction inDirection)
    {
        var gridSpaceConnection = gridSpace.Connections.FirstOrDefault(x => x.TargetSpace == otherSpace);
        gridSpaceConnection.Should().NotBeNull($"Space {gridSpace} should have a connection to space {otherSpace}");
        gridSpaceConnection.Direction.Should().Be(inDirection);
    }
}

public class InvalidDirectionException : Exception
{
    public InvalidDirectionException(string message) : base(message)
    {
    }
}

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
}

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

public record Cell(CellState State);

public enum CellState
{
    White = 0,
    Black = 1
}