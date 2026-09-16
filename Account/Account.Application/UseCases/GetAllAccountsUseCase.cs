using Account.Application.Common;
using Account.Application.DTO;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class GetAllAccountsUseCase : IUseCase<Unit, List<AccountDto>>
{
    private readonly ICuentaRepository cuentaRepository;

    public GetAllAccountsUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<List<AccountDto>> Execute(Unit input)
    {
        var cuentas = await this.cuentaRepository.GetAllAccounts();
        return cuentas.Select(AccountDtoFactory.CreateFromEntity).ToList();
    }
}
