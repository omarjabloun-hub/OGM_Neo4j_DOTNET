﻿namespace TMC.Domain.Exceptions;

public class NotFoundNeo4jException : Exception
{
    public readonly int Code = 404;

    public NotFoundNeo4jException(string message) : base(message)
    {
    }
}