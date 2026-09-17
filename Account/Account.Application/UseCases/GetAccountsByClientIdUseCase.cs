using Account.Application.Common;
using Account.Application.DTO;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class GetAccountsByClientIdUseCase : IUseCase<Guid, List<AccountDto>>
{
    private readonly ICuentaRepository cuentaRepository;

    public GetAccountsByClientIdUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<List<AccountDto>> Execute(Guid input)
    {
        var cuentas = await this.cuentaRepository.GetAccountsByClientId(input);
        return cuentas.Select(AccountDtoFactory.CreateFromEntity).ToList();
    }
}
