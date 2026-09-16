namespace Client.Application.DTO;

public class UpdateClientRequest
{
    public Guid Id { get; set; }

    public UpdateClientDto Dto { get; set; } = null!;
}
