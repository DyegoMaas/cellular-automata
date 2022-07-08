using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CellularAutomata.Tests;

public class GridSpaceTests
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