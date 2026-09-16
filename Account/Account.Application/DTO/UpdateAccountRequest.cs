namespace Account.Application.DTO;

public class UpdateAccountRequest
{
    public Guid Id { get; set; }

    public UpdateAccountDto Dto { get; set; } = null!;
}
