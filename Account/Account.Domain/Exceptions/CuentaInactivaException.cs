namespace Account.Domain.Exceptions;

public class CuentaInactivaException : Exception
{
    public CuentaInactivaException()
        : base("La cuenta se encuentra inactiva")
    {
    }
}
