using Account.Application.Common;
using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class UpdateAccountUseCase : IUseCase<UpdateAccountRequest, AccountDto>
{
    private readonly ICuentaRepository cuentaRepository;

    public UpdateAccountUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<AccountDto> Execute(UpdateAccountRequest input)
    {
        var cuenta = await this.cuentaRepository.GetAccountById(input.Id);
        if (cuenta == null)
        {
            throw new AccountNotFoundException();
        }

        if (input.Dto.Estado)
        {
            cuenta.Activate();
        }
        else
        {
            cuenta.Deactivate();
        }

        await this.cuentaRepository.UpdateAccount(cuenta);

        return AccountDtoFactory.CreateFromEntity(cuenta);
    }
}
