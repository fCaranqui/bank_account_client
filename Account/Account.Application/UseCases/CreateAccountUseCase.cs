using Account.Application.Common;
using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Domain;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class CreateAccountUseCase : IUseCase<CreateAccountDto, AccountDto>
{
    private readonly ICuentaRepository cuentaRepository;

    public CreateAccountUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<AccountDto> Execute(CreateAccountDto input)
    {
        if (await this.cuentaRepository.ExistsAccountWithNumber(input.NumeroCuenta))
        {
            throw new DuplicateAccountNumberException();
        }

        // ClienteId is not validated against an existing client here: this service has no local
        // copy of client data to check against, so the check simply cannot be made yet.
        var cuenta = new Cuenta(input.NumeroCuenta, input.TipoCuenta, input.SaldoInicial, input.ClienteId);

        await this.cuentaRepository.CreateAccount(cuenta);

        return AccountDtoFactory.CreateFromEntity(cuenta);
    }
}
