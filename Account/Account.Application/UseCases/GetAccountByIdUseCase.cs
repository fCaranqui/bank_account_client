using Account.Application.Common;
using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class GetAccountByIdUseCase : IUseCase<Guid, AccountDto>
{
    private readonly ICuentaRepository cuentaRepository;

    public GetAccountByIdUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<AccountDto> Execute(Guid input)
    {
        var cuenta = await this.cuentaRepository.GetAccountById(input);
        if (cuenta == null)
        {
            throw new AccountNotFoundException();
        }

        return AccountDtoFactory.CreateFromEntity(cuenta);
    }
}
