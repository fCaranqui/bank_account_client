namespace Account.Application.Exceptions;

public class ClientNotFoundException : Exception
{
    public ClientNotFoundException()
        : base("El cliente no fue encontrado.")
    {
    }

    public ClientNotFoundException(string message)
        : base(message)
    {
    }
}
