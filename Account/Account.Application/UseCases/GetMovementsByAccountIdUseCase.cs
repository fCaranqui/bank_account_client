using Account.Application.Common;
using Account.Application.DTO;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class GetMovementsByAccountIdUseCase : IUseCase<GetMovementsByAccountIdRequest, List<MovementDto>>
{
    private readonly ICuentaRepository cuentaRepository;

    public GetMovementsByAccountIdUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<List<MovementDto>> Execute(GetMovementsByAccountIdRequest input)
    {
        var movimientos = await this.cuentaRepository.GetMovementsByAccountId(
            input.CuentaId,
            input.Desde,
            input.Hasta,
            input.Skip,
            input.Take);

        return movimientos.Select(MovementDtoFactory.CreateFromEntity).ToList();
    }
}
