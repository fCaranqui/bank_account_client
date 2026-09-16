using Account.Domain;

namespace Account.Application.DTO;

public class MovementDto
{
    public Guid Id { get; set; }

    public Guid CuentaId { get; set; }

    public DateTime Fecha { get; set; }

    public TipoMovimiento TipoMovimiento { get; set; }

    public decimal Valor { get; set; }

    public decimal Saldo { get; set; }
}
