namespace KnowHub.Application.Exceptions;

/// <summary>
/// Raised for bad credentials or a disabled account. The message is intentionally
/// vague so it cannot be used to discover which emails are registered.
/// </summary>
public class AuthenticationFailedException : AppException
{
    public AuthenticationFailedException(string message = "Invalid email or password.")
        : base(message)
    {
    }

    public override int StatusCode => 401;
}
