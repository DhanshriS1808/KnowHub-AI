namespace KnowHub.Application.Exceptions;

/// <summary>
/// Base for errors that map to a deliberate HTTP status rather than a 500.
/// </summary>
public abstract class AppException : Exception
{
    protected AppException(string message) : base(message)
    {
    }

    public abstract int StatusCode { get; }
}
