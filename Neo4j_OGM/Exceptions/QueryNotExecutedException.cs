namespace TMC.Domain.Exceptions;

public class QueryNotExecutedException : Exception
{
    public readonly int Code = 400;

    public QueryNotExecutedException(string message) : base(message)
    {
    }
}