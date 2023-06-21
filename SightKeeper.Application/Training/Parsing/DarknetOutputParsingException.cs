namespace SightKeeper.Application.Training.Parsing;

public class DarknetOutputParsingException : Exception
{
    public DarknetOutputParsingException(string message) : base(message)
    {
    }

    public DarknetOutputParsingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}