using Client.Domain;

namespace Client.Application.DTO;

public class CreateClientDto
{
    public string Nombre { get; set; } = null!;

    public Genero Genero { get; set; }

    public int Edad { get; set; }

    public string Identificacion { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string ClienteId { get; set; } = null!;

    public string Contrasena { get; set; } = null!;
}
