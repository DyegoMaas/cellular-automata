using CellularAutomata.GridSpace;
using FluentAssertions;
using Xunit;

namespace CellularAutomata.Tests;

public class DirectionTests
{
    [Fact(DisplayName = "Direction should point to a direction")]
    public void ADirectionShouldPointSomewhere()
    {
        var instantiateInvalidDirection = () =>
        {
            var invalidDirection = new Direction(0, 0);
        };

        instantiateInvalidDirection.Should().Throw<InvalidDirectionException>().WithMessage("Direction cannot be 0,0");
    }
}