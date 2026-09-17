using Account.Domain;

namespace Account.Application.DTO;

public class AccountDto
{
    public Guid Id { get; set; }

    public string NumeroCuenta { get; set; } = null!;

    public TipoCuenta TipoCuenta { get; set; }

    public decimal SaldoInicial { get; set; }

    public decimal SaldoDisponible { get; set; }

    public bool Estado { get; set; }

    public Guid ClienteId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
