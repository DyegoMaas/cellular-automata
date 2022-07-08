using System.Linq;
using CellularAutomata.Cells;
using CellularAutomata.Space;
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
    public static void ShouldBeConnectedTo(this GridSpace gridSpace, GridSpace otherSpace, Direction inDirection)
    {
        var gridSpaceConnection = gridSpace.Connections.FirstOrDefault(x => x.TargetSpace == otherSpace);
        gridSpaceConnection.Should().NotBeNull($"Space {gridSpace} should have a connection to space {otherSpace}");
        gridSpaceConnection.Direction.Should().Be(inDirection);
    }
}