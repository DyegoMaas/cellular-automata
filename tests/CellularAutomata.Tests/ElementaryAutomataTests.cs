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

public class OneDimensionalNavigator : Navigator
{
    private readonly Direction _traverseDirection;
    private GridNode? _currentNode;
    private Automata _automata;

    public OneDimensionalNavigator(Automata automata, Direction traverseDirection)
    {
        _automata = automata;
        
        _traverseDirection = traverseDirection;
        var firstNode = automata.GridNodes.First();
        _currentNode = FindNavigationStartPoint(traverseDirection, firstNode) ?? firstNode;
    }

    private GridNode? FindNavigationStartPoint(Direction traverseDirection, GridNode node)
    {
        var oppositeDirection = traverseDirection.Opposite();
       
        var selectedConnection = node.Connections
            .FirstOrDefault(x => x.Direction.X == oppositeDirection.X);
        var lastConnection = selectedConnection;
        while (selectedConnection != null)
        {
            selectedConnection = selectedConnection.TargetNode.Connections
                .FirstOrDefault(x => x.Direction.X == oppositeDirection.X);
            if (selectedConnection != null)
            {
                lastConnection = selectedConnection;
            }
        }

        return lastConnection?.TargetNode;
    }
    
    public override GridNode? GetNext()
    {
        if (_currentNode is null)
            return null;

        var currentNode = _currentNode;
        var selectedConnection = _currentNode.Connections
            .FirstOrDefault(x => x.Direction.X == _traverseDirection.X);
        _currentNode = selectedConnection?.TargetNode; 
        return currentNode;
    }

    // public override GridNode? GetNext()
    // {
    //     if (_navigationStartPoint is null)
    //         return null;
    //    
    //     var selectedConnection = _navigationStartPoint.Connections
    //         .FirstOrDefault(x => x.Direction.X == _traverseDirection.X);
    //     var lastConnection = selectedConnection;
    //     while (selectedConnection != null)
    //     {
    //         selectedConnection = selectedConnection.TargetNode.Connections
    //             .FirstOrDefault(x => x.Direction.X == _traverseDirection.X);
    //         if (selectedConnection != null)
    //         {
    //             lastConnection = selectedConnection;
    //         }
    //     }
    //
    //     return lastConnection?.TargetNode;
    // }
}

// public class OneDimensionalNavigator2 : Navigator
// {
//     private readonly GridNode[] _gridNodes;
//     private readonly Direction _traverseDirection;
//
//     public OneDimensionalNavigator2(Automata automata, Direction traverseDirection)
//     {
//         _traverseDirection = traverseDirection;
//         _gridNodes = automata.GridNodes;
//     }
//
//     public override GridNode? GetNext()
//     {
//         _gridNodes.fir
//         return null;
//     }
// }

public abstract class Navigator
{
    public abstract GridNode? GetNext();
}

// public class LeftToRightNavigator : Navigator
// {
//     private readonly Automata _automata;
//     private int _index = 0;
//
//     public LeftToRightNavigator(Automata automata)
//     {
//         _automata = automata;
//     }
//
//     public override GridNode? GetNext()
//     {
//         var nodes = _automata.GridNodes;
//         if (_index == nodes.Length)
//             return null;
//         
//         var currentNode = nodes[_index++];
//         return currentNode;
//     }
// }
//
// public class RightToLeftNavigator : Navigator
// {
//     private readonly Automata _automata;
//     private int _index = 0;
//
//     public RightToLeftNavigator(Automata automata)
//     {
//         _automata = automata;
//         _index = automata.GridNodes.Length;
//     }
//
//     public override GridNode? GetNext()
//     {
//         var nodes = _automata.GridNodes;
//         if (_index == 0)
//             return null;
//         
//         var currentNode = nodes[--_index];
//         return currentNode;
//     }
// }

public class Automata
{
    public GridNode[] GridNodes { get; }

    public Automata(GridNode[] gridNodes)
    {
        GridNodes = gridNodes;
    }
}