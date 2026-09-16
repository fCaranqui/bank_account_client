namespace Client.Application.Exceptions;

public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException()
        : base("La contraseña actual es incorrecta.")
    {
    }

    public IncorrectPasswordException(string message)
        : base(message)
    {
    }
}
