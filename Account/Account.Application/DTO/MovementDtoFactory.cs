using Account.Domain;

namespace Account.Application.DTO;

public static class MovementDtoFactory
{
    public static MovementDto CreateFromEntity(Movimiento movimiento)
    {
        return new MovementDto
        {
            Id = movimiento.Id,
            CuentaId = movimiento.CuentaId,
            Fecha = movimiento.Fecha,
            TipoMovimiento = movimiento.TipoMovimiento,
            Valor = movimiento.Valor,
            Saldo = movimiento.Saldo,
        };
    }
}
