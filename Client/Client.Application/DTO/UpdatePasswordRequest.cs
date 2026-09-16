namespace Client.Application.DTO;

public class UpdatePasswordRequest
{
    public Guid ClienteId { get; set; }

    public UpdatePasswordDto Dto { get; set; } = null!;
}
