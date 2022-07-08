using System.Linq;
using CellularAutomata.Cells;
using CellularAutomata.GridSpace;
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

public static class GridSpaceTestExtensions
{
    public static void ShouldBeConnectedTo(this GridNode gridNode, GridNode otherNode, Direction inDirection)
    {
        var gridSpaceConnection = gridNode.Connections.FirstOrDefault(x => x.TargetNode == otherNode);
        gridSpaceConnection.Should().NotBeNull($"Space {gridNode} should have a connection to space {otherNode}");
        gridSpaceConnection.Direction.Should().Be(inDirection);
    }
}