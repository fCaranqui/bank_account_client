using Client.Domain;

namespace Client.Application.DTO;

public class UpdateClientDto
{
    public string Nombre { get; set; } = null!;

    public Genero Genero { get; set; }

    public int Edad { get; set; }

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;
}
