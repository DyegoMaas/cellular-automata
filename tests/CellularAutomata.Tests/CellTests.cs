using CellularAutomata.Cells;
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