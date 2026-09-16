namespace Client.Application.Exceptions;

public class DuplicateIdentificationException : Exception
{
    public DuplicateIdentificationException()
        : base("Ya existe un cliente con esa identificación.")
    {
    }

    public DuplicateIdentificationException(string message)
        : base(message)
    {
    }
}
