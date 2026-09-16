namespace Account.Application.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException()
        : base("La cuenta no fue encontrada.")
    {
    }

    public AccountNotFoundException(string message)
        : base(message)
    {
    }
}
