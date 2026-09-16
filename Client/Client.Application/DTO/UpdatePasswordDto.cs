namespace Client.Application.DTO;

public class UpdatePasswordDto
{
    public string ContrasenaActual { get; set; } = null!;

    public string ContrasenaNueva { get; set; } = null!;
}
