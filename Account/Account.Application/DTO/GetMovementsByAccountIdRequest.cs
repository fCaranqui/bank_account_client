namespace Account.Application.DTO;

public class GetMovementsByAccountIdRequest
{
    public Guid CuentaId { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }

    public int Skip { get; set; }

    public int Take { get; set; }
}
