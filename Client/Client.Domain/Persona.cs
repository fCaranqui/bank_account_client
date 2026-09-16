namespace Client.Domain;

public class Persona
{
    public Guid Id { get; protected set; }

    public string Nombre { get; private set; } = null!;

    public Genero Genero { get; private set; }

    public int Edad { get; private set; }

    public string Identificacion { get; private set; } = null!;

    public string Direccion { get; private set; } = null!;

    public string Telefono { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; protected set; }

    protected Persona()
    {
    }

    public Persona(string nombre, Genero genero, int edad, string identificacion, string direccion, string telefono)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
        }

        if (string.IsNullOrWhiteSpace(identificacion))
        {
            throw new ArgumentException("La identificación es obligatoria.", nameof(identificacion));
        }

        if (edad < 0 || edad > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(edad), "La edad no es válida.");
        }

        Id = Guid.NewGuid();
        Nombre = nombre;
        Genero = genero;
        Edad = edad;
        Identificacion = identificacion;
        Direccion = direccion;
        Telefono = telefono;
    }

    public void UpdateDetails(string nombre, Genero genero, int edad, string direccion, string telefono)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
        }

        if (edad < 0 || edad > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(edad), "La edad no es válida.");
        }

        Nombre = nombre;
        Genero = genero;
        Edad = edad;
        Direccion = direccion;
        Telefono = telefono;
    }
}
