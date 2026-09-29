namespace KnowHub.Application.Exceptions;

/// <summary>
/// Raised when a request conflicts with existing state, such as a duplicate email.
/// </summary>
public class ConflictException : AppException
{
    public ConflictException(string message) : base(message)
    {
    }

    public override int StatusCode => 409;
}
