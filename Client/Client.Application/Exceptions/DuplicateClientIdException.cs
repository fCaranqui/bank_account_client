namespace Client.Application.Exceptions;

public class DuplicateClientIdException : Exception
{
    public DuplicateClientIdException()
        : base("Ya existe un cliente con ese ClienteId.")
    {
    }

    public DuplicateClientIdException(string message)
        : base(message)
    {
    }
}
