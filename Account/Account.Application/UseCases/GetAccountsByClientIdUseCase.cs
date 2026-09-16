using Account.Application.Common;
using Account.Application.DTO;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class GetAccountsByClientIdUseCase : IUseCase<string, List<AccountDto>>
{
    private readonly ICuentaRepository cuentaRepository;

    public GetAccountsByClientIdUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<List<AccountDto>> Execute(string input)
    {
        var cuentas = await this.cuentaRepository.GetAccountsByClientId(input);
        return cuentas.Select(AccountDtoFactory.CreateFromEntity).ToList();
    }
}
