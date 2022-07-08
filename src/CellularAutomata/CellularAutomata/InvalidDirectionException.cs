namespace CellularAutomata;

public class InvalidDirectionException : Exception
{
    public InvalidDirectionException(string message) : base(message)
    {
    }
}