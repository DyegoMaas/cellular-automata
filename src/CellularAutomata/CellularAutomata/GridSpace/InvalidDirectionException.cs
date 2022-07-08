namespace CellularAutomata.GridSpace;

public class InvalidDirectionException : Exception
{
    public InvalidDirectionException(string message) : base(message)
    {
    }
}