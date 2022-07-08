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

        Navigator navigator = new OneDimensionalNavigator(elementaryAutomata, traverseDirection: new Direction(1, 0));
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

        Navigator navigator = new OneDimensionalNavigator(elementaryAutomata, traverseDirection: new Direction(-1, 0));
        GridNode? firstGridNode = navigator.GetNext();
        GridNode? secondGridNode = navigator.GetNext();
        GridNode? thirdGridNode = navigator.GetNext();
        GridNode? fourthGridNode = navigator.GetNext();
        GridNode? fifthGridNode = navigator.GetNext();
        GridNode? nullNode = navigator.GetNext();
        GridNode? nullNode2 = navigator.GetNext();

        firstGridNode!.Cell.State.Should().Be(CellState.Black);
        secondGridNode!.Cell.State.Should().Be(CellState.White);
        thirdGridNode!.Cell.State.Should().Be(CellState.White);
        fourthGridNode!.Cell.State.Should().Be(CellState.Black);
        fifthGridNode!.Cell.State.Should().Be(CellState.White);
        nullNode.Should().BeNull("navigated the whole automata");
    }
    
    // TODO navigator.GetNextNeighrhood()
    
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

public class OneDimensionalNavigator : Navigator
{
    private readonly Direction _traverseDirection;
    private GridNode? _currentNode;
    private Automata _automata;
    private int _index = 0;

    public OneDimensionalNavigator(Automata automata, Direction traverseDirection)
    {
        _automata = automata;
        
        _traverseDirection = traverseDirection;
        var firstNode = automata.GridNodes.First();
        _currentNode = FindNavigationStartPoint(traverseDirection, firstNode);
        _index = 0;
    }

    private GridNode FindNavigationStartPoint(Direction traverseDirection, GridNode node)
    {
        var oppositeDirection = traverseDirection.Opposite();
        _currentNode = node;

        var nextNode = node;
        var lastNode = node;
        while (nextNode is not null)
        {
            nextNode = GetNext(oppositeDirection);
            if (nextNode is not null)
            {
                lastNode = nextNode;
            }
        }

        return lastNode;
    }

    public override GridNode? GetNext() => GetNext(_traverseDirection);

    private GridNode? GetNext(Direction direction)
    {
        if (_index == 0)
        {
            _index++;
            return _currentNode;
        }

        if (_currentNode is null)
            return null;
        
        var selectedConnection = _currentNode.Connections
            .FirstOrDefault(x => x.Direction.X == direction.X);
        _currentNode = selectedConnection?.TargetNode; 
        return _currentNode;
    }
}

public abstract class Navigator
{
    public abstract GridNode? GetNext();
}

public class Automata
{
    public GridNode[] GridNodes { get; }

    public Automata(GridNode[] gridNodes)
    {
        GridNodes = gridNodes;
    }
}