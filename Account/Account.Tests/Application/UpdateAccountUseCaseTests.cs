using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class UpdateAccountUseCaseTests
{
    [Fact]
    public async Task Execute_DeactivateAccount_SetsEstadoFalse()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        await repository.CreateAccount(cuenta);
        var useCase = new UpdateAccountUseCase(repository);

        var result = await useCase.Execute(new UpdateAccountRequest { Id = cuenta.Id, Dto = new UpdateAccountDto { Estado = false } });

        Assert.False(result.Estado);
    }

    [Fact]
    public async Task Execute_ActivateAccount_SetsEstadoTrue()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        cuenta.Deactivate();
        await repository.CreateAccount(cuenta);
        var useCase = new UpdateAccountUseCase(repository);

        var result = await useCase.Execute(new UpdateAccountRequest { Id = cuenta.Id, Dto = new UpdateAccountDto { Estado = true } });

        Assert.True(result.Estado);
    }

    [Fact]
    public async Task Execute_NonExistingAccount_ThrowsAccountNotFoundException()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new UpdateAccountUseCase(repository);

        await Assert.ThrowsAsync<AccountNotFoundException>(
            () => useCase.Execute(new UpdateAccountRequest { Id = Guid.NewGuid(), Dto = new UpdateAccountDto { Estado = true } }));
    }
}
