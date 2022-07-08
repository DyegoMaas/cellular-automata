using System.Linq;
using CellularAutomata.Cells;
using CellularAutomata.GridSpace;
using FluentAssertions;
using Xunit;

namespace CellularAutomata.Tests;

public class ElementaryAutomataTests
{
    [Fact(DisplayName = "A navigator should be able to traverse the automata left to right")]
    public void ANavigatorShouldTraverseLeftToRight()
    {
        Automata elementaryAutomata = ProduceOneDimensionalAutomata(initialStates: new[]
        {
            CellState.White,
            CellState.Black,
            CellState.White,
            CellState.White,
            CellState.Black,
        });

        LeftToRightNavigator navigator = new LeftToRightNavigator(elementaryAutomata);
        GridNode? firstGridNode = navigator.GetNext();
        GridNode? secondGridNode = navigator.GetNext();
        GridNode? thirdGridNode = navigator.GetNext();
        GridNode? fourthGridNode = navigator.GetNext();
        GridNode? fifthGridNode = navigator.GetNext();
        GridNode? nullNode = navigator.GetNext();

        firstGridNode!.Cell.State.Should().Be(CellState.White);
        secondGridNode!.Cell.State.Should().Be(CellState.Black);
        thirdGridNode!.Cell.State.Should().Be(CellState.White);
        fourthGridNode!.Cell.State.Should().Be(CellState.White);
        fifthGridNode!.Cell.State.Should().Be(CellState.Black);
        nullNode.Should().BeNull("navigated the whole automata");
    }
    
    [Fact(DisplayName = "A navigator should be able to traverse the automata right to left")]
    public void ANavigatorShouldTraverseRightToLeft()
    {
        Automata elementaryAutomata = ProduceOneDimensionalAutomata(initialStates: new[]
        {
            CellState.White,
            CellState.Black,
            CellState.White,
            CellState.White,
            CellState.Black,
        });

        RightToLeftNavigator navigator = new RightToLeftNavigator(elementaryAutomata);
        GridNode? firstGridNode = navigator.GetNext();
        GridNode? secondGridNode = navigator.GetNext();
        GridNode? thirdGridNode = navigator.GetNext();
        GridNode? fourthGridNode = navigator.GetNext();
        GridNode? fifthGridNode = navigator.GetNext();
        GridNode? nullNode = navigator.GetNext();

        firstGridNode!.Cell.State.Should().Be(CellState.Black);
        secondGridNode!.Cell.State.Should().Be(CellState.White);
        thirdGridNode!.Cell.State.Should().Be(CellState.White);
        fourthGridNode!.Cell.State.Should().Be(CellState.Black);
        fifthGridNode!.Cell.State.Should().Be(CellState.White);
        nullNode.Should().BeNull("navigated the whole automata");
    }
    
    // [Fact(DisplayName = "A rule should produce a new state based on the current state of a cell and the previous state of surrounding cells")]
    // public void ShouldProduceANewStateBasedOnSurroundingCells()
    // {
    //     Automata elementaryAutomata = ProduceOneDimensionalAutomata(initialStates: new[]
    //     {
    //         CellState.White,
    //         CellState.Black,
    //         CellState.White,
    //         CellState.White,
    //         CellState.White,
    //         CellState.White,
    //         CellState.White,
    //         CellState.White,
    //     });
    //     
    //     elementaryAutomata.Apply
    // }

    private Automata ProduceOneDimensionalAutomata(params CellState[] initialStates)
    {
        var gridNodes = initialStates
            .Select(state => new GridNode(new Cell(state)))
            .ToArray();

        for (var i = 0; i < gridNodes.Length - 1; i++)
        {
            var leftCell = gridNodes[i];
            var rightCell = gridNodes[i + 1];
            GridSpaceConnection.Connect(leftCell, rightCell, new Direction(1, 0));
        }

        return new Automata(gridNodes);
    }

    private static GridNode NewGridSpace(CellState state) => new GridNode(new Cell(state));
}

public class LeftToRightNavigator
{
    private readonly Automata _automata;
    private int _index = 0;

    public LeftToRightNavigator(Automata automata)
    {
        _automata = automata;
    }

    public GridNode? GetNext()
    {
        var nodes = _automata.GridNodes;
        if (_index == nodes.Length)
            return null;
        
        var currentNode = nodes[_index++];
        return currentNode;
    }
}

public class RightToLeftNavigator
{
    private readonly Automata _automata;
    private int _index = 0;

    public RightToLeftNavigator(Automata automata)
    {
        _automata = automata;
        _index = automata.GridNodes.Length;
    }

    public GridNode? GetNext()
    {
        var nodes = _automata.GridNodes;
        if (_index == 0)
            return null;
        
        var currentNode = nodes[--_index];
        return currentNode;
    }
}

public class Automata
{
    public Automata(GridNode[] gridNodes)
    {
        GridNodes = gridNodes;
    }

    public GridNode[] GridNodes { get; }
}