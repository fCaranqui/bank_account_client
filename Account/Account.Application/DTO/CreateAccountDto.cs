using Account.Domain;

namespace Account.Application.DTO;

public class CreateAccountDto
{
    public string NumeroCuenta { get; set; } = null!;

    public TipoCuenta TipoCuenta { get; set; }

    public decimal SaldoInicial { get; set; }

    public Guid ClienteId { get; set; }
}
