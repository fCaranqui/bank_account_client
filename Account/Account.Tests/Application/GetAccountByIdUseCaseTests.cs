using Account.Application.Exceptions;
using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class GetAccountByIdUseCaseTests
{
    [Fact]
    public async Task Execute_ExistingAccount_ReturnsAccountDto()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, "CLI-001");
        await repository.CreateAccount(cuenta);
        var useCase = new GetAccountByIdUseCase(repository);

        var result = await useCase.Execute(cuenta.Id);

        Assert.Equal(cuenta.Id, result.Id);
        Assert.Equal("001-001", result.NumeroCuenta);
    }

    [Fact]
    public async Task Execute_NonExistingAccount_ThrowsAccountNotFoundException()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new GetAccountByIdUseCase(repository);

        await Assert.ThrowsAsync<AccountNotFoundException>(() => useCase.Execute(Guid.NewGuid()));
    }
}
