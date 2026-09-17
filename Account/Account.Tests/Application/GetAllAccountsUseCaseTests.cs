using Account.Application.Common;
using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class GetAllAccountsUseCaseTests
{
    [Fact]
    public async Task Execute_ReturnsAllAccounts()
    {
        var repository = TestDbContextFactory.CreateRepository();
        await repository.CreateAccount(new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid()));
        await repository.CreateAccount(new Cuenta("001-002", TipoCuenta.Corriente, 200m, Guid.NewGuid()));
        var useCase = new GetAllAccountsUseCase(repository);

        var result = await useCase.Execute(Unit.Value);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Execute_NoAccounts_ReturnsEmptyList()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new GetAllAccountsUseCase(repository);

        var result = await useCase.Execute(Unit.Value);

        Assert.Empty(result);
    }
}
