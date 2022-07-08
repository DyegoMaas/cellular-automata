namespace CellularAutomata.Space;

public class InvalidDirectionException : Exception
{
    public InvalidDirectionException(string message) : base(message)
    {
    }
}