namespace DigitalLibrary.BLL.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message)
        : base(message, 403)
    {
    }
}