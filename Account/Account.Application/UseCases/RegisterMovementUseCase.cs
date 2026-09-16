using Account.Application.Common;
using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class RegisterMovementUseCase : IUseCase<RegisterMovementDto, MovementDto>
{
    private readonly ICuentaRepository cuentaRepository;

    public RegisterMovementUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<MovementDto> Execute(RegisterMovementDto input)
    {
        // No try/catch around this call: SaldoNoDisponibleException and CuentaInactivaException
        // are expected business-rule violations that must reach the caller uncaught.
        var movimiento = await this.cuentaRepository.RegisterMovement(input.CuentaId, input.TipoMovimiento, input.Valor);
        if (movimiento == null)
        {
            throw new AccountNotFoundException();
        }

        return MovementDtoFactory.CreateFromEntity(movimiento);
    }
}
