using Account.Application.Common;
using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Domain;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class CreateAccountUseCase : IUseCase<CreateAccountDto, AccountDto>
{
    private readonly ICuentaRepository cuentaRepository;
    private readonly IClientReplicaRepository clientReplicaRepository;

    public CreateAccountUseCase(ICuentaRepository cuentaRepository, IClientReplicaRepository clientReplicaRepository)
    {
        this.cuentaRepository = cuentaRepository;
        this.clientReplicaRepository = clientReplicaRepository;
    }

    public async Task<AccountDto> Execute(CreateAccountDto input)
    {
        if (await this.cuentaRepository.ExistsAccountWithNumber(input.NumeroCuenta))
        {
            throw new DuplicateAccountNumberException();
        }

        // ClienteId is checked against the local ClientReplica table, populated asynchronously by
        // the ClientCreatedEvent consumer rather than by a synchronous call to the Client service.
        if (!await this.clientReplicaRepository.Exists(input.ClienteId))
        {
            throw new ClientNotFoundException();
        }

        var cuenta = new Cuenta(input.NumeroCuenta, input.TipoCuenta, input.SaldoInicial, input.ClienteId);

        await this.cuentaRepository.CreateAccount(cuenta);

        return AccountDtoFactory.CreateFromEntity(cuenta);
    }
}
