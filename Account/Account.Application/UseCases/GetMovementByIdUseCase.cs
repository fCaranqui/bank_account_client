using Account.Application.Common;
using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class GetMovementByIdUseCase : IUseCase<Guid, MovementDto>
{
    private readonly ICuentaRepository cuentaRepository;

    public GetMovementByIdUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<MovementDto> Execute(Guid input)
    {
        var movimiento = await this.cuentaRepository.GetMovementById(input);
        if (movimiento == null)
        {
            throw new MovementNotFoundException();
        }

        return MovementDtoFactory.CreateFromEntity(movimiento);
    }
}
