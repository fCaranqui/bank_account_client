using Account.Domain;

namespace Account.Application.DTO;

public class AccountStatementDto
{
    public string NumeroCuenta { get; set; } = null!;

    public TipoCuenta TipoCuenta { get; set; }

    public decimal SaldoDisponible { get; set; }

    public List<MovementDto> Movimientos { get; set; } = new();
}
