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
        var spaceA = NewSpace();
        var spaceB = NewSpace();

        GridSpaceConnection.Connect(spaceA, to: spaceB);

        IEnumerable<GridSpace> spacesConnectedToSpaceA = spaceA.Connections;
        spacesConnectedToSpaceA.First().Should().Be(spaceB);
        
        IEnumerable<GridSpace> spacesConnectedToSpaceB = spaceB.Connections;
        spacesConnectedToSpaceB.Should().Contain(spaceA);
    }

    private static GridSpace NewSpace() => new GridSpace(new Cell(CellState.White));

    // [Fact(DisplayName = "A grid should be able to define a Moore's Neighborhood")]
    // public void ItShouldBePossibleToModelAMooreNeighorhood()
    // {
    //     var centralSpace = NewSpace();
    //     var leftSpace = NewSpace();
    //     var rightSpace = NewSpace();
    //     var topSpace = NewSpace();
    //     var bottomSpace = NewSpace();
    //     var topLeftSpace = NewSpace();
    //     var topRightSpace = NewSpace();
    //     var bottomLeftSpace = NewSpace();
    //     var bottomRightSpace = NewSpace();
    //     
    //     centralSpace.ConnectTo(leftSpace, new Direction());
    //     centralSpace.ConnectTo(rightSpace);
    //     centralSpace.ConnectTo(topSpace);
    //     centralSpace.ConnectTo(bottomSpace);
    //     centralSpace.ConnectTo(topLeftSpace);
    //     centralSpace.ConnectTo(topRightSpace);
    //     centralSpace.ConnectTo(bottomLeftSpace);
    //     centralSpace.ConnectTo(bottomRightSpace);
    //
    //     centralSpace.ConnectedSpaces.Should().BeEquivalentTo(new[]
    //     {
    //         leftSpace,
    //         rightSpace,
    //         topSpace,
    //         bottomSpace,
    //         topLeftSpace,
    //         topRightSpace,
    //         bottomLeftSpace,
    //         bottomRightSpace,
    //     });
    // }
}

public class GridSpace
{
    public GridSpace(Cell cell)
    {
        Cell = cell;
    }

    private readonly List<GridSpaceConnection> _connections = new();

    public Cell Cell { get; private set; }
    public IEnumerable<GridSpace> Connections => _connections.Select(x => x.TargetSpace);

    public void ReplaceCell(Cell newCell)
    {
        Cell = newCell;
    }

    public void AddConnection(GridSpaceConnection connection)
    {
        _connections.Add(connection);
    }
}

public class GridSpaceConnection
{
    public GridSpace TargetSpace { get; }

    private GridSpaceConnection(GridSpace targetSpace)
    {
        TargetSpace = targetSpace;
    }

    public static void Connect(GridSpace origin, GridSpace to)
    {
        var connectionA = new GridSpaceConnection(origin);
        var connectionB = new GridSpaceConnection(to);

        origin.AddConnection(connectionB);
        to.AddConnection(connectionA);
    }
}

public record Cell(CellState State);

public enum CellState
{
    White = 0,
    Black = 1
}