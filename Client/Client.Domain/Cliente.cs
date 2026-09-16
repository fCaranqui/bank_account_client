namespace Client.Domain;

public class Cliente : Persona
{
    public string ClienteId { get; private set; } = null!;

    public string ContrasenaHash { get; private set; } = null!;

    public bool Estado { get; private set; }

    protected Cliente()
    {
    }

    public Cliente(
        string nombre,
        Genero genero,
        int edad,
        string identificacion,
        string direccion,
        string telefono,
        string clienteId,
        string contrasenaHash)
        : base(nombre, genero, edad, identificacion, direccion, telefono)
    {
        if (string.IsNullOrWhiteSpace(clienteId))
        {
            throw new ArgumentException("ClienteId es obligatorio.", nameof(clienteId));
        }

        if (string.IsNullOrWhiteSpace(contrasenaHash))
        {
            throw new ArgumentException("La contraseña es obligatoria.", nameof(contrasenaHash));
        }

        ClienteId = clienteId;
        ContrasenaHash = contrasenaHash;
        Estado = true;
    }

    public void Deactivate() => Estado = false;

    public void Activate() => Estado = true;

    public void Delete() => this.DeletedAt = DateTime.UtcNow;

    public void SetPasswordHash(string contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(contrasenaHash))
        {
            throw new ArgumentException("La contraseña es obligatoria.", nameof(contrasenaHash));
        }

        ContrasenaHash = contrasenaHash;
    }
}
