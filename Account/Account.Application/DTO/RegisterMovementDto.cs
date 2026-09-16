using Account.Domain;

namespace Account.Application.DTO;

public class RegisterMovementDto
{
    public Guid CuentaId { get; set; }

    public TipoMovimiento TipoMovimiento { get; set; }

    public decimal Valor { get; set; }
}
