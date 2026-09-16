namespace Account.Application.Exceptions;

public class DuplicateAccountNumberException : Exception
{
    public DuplicateAccountNumberException()
        : base("Ya existe una cuenta con ese número de cuenta.")
    {
    }

    public DuplicateAccountNumberException(string message)
        : base(message)
    {
    }
}
