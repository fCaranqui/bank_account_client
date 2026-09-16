using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class GetAccountsByClientIdUseCaseTests
{
    [Fact]
    public async Task Execute_ReturnsOnlyAccountsForGivenClient()
    {
        var repository = TestDbContextFactory.CreateRepository();
        await repository.CreateAccount(new Cuenta("001-001", TipoCuenta.Ahorro, 100m, "CLI-001"));
        await repository.CreateAccount(new Cuenta("001-002", TipoCuenta.Corriente, 200m, "CLI-001"));
        await repository.CreateAccount(new Cuenta("001-003", TipoCuenta.Ahorro, 300m, "CLI-002"));
        var useCase = new GetAccountsByClientIdUseCase(repository);

        var result = await useCase.Execute("CLI-001");

        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Equal("CLI-001", a.ClienteId));
    }

    [Fact]
    public async Task Execute_NoAccountsForClient_ReturnsEmptyList()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new GetAccountsByClientIdUseCase(repository);

        var result = await useCase.Execute("CLI-999");

        Assert.Empty(result);
    }
}
