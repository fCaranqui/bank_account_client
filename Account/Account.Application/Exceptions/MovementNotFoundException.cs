namespace Account.Application.Exceptions;

public class MovementNotFoundException : Exception
{
    public MovementNotFoundException()
        : base("El movimiento no fue encontrado.")
    {
    }

    public MovementNotFoundException(string message)
        : base(message)
    {
    }
}
