namespace Account.Application.DTO;

public class GenerateAccountStatementReportRequest
{
    public Guid ClienteId { get; set; }

    public DateTime Desde { get; set; }

    public DateTime Hasta { get; set; }
}
