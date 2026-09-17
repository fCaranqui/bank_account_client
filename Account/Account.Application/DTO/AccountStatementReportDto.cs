namespace Account.Application.DTO;

public class AccountStatementReportDto
{
    public Guid ClienteId { get; set; }

    public DateTime Desde { get; set; }

    public DateTime Hasta { get; set; }

    public List<AccountStatementDto> Cuentas { get; set; } = new();
}
