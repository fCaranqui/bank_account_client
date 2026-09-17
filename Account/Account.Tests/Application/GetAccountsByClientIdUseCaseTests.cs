using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class GetAccountsByClientIdUseCaseTests
{
    [Fact]
    public async Task Execute_ReturnsOnlyAccountsForGivenClient()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var clienteId = Guid.NewGuid();
        await repository.CreateAccount(new Cuenta("001-001", TipoCuenta.Ahorro, 100m, clienteId));
        await repository.CreateAccount(new Cuenta("001-002", TipoCuenta.Corriente, 200m, clienteId));
        await repository.CreateAccount(new Cuenta("001-003", TipoCuenta.Ahorro, 300m, Guid.NewGuid()));
        var useCase = new GetAccountsByClientIdUseCase(repository);

        var result = await useCase.Execute(clienteId);

        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Equal(clienteId, a.ClienteId));
    }

    [Fact]
    public async Task Execute_NoAccountsForClient_ReturnsEmptyList()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new GetAccountsByClientIdUseCase(repository);

        var result = await useCase.Execute(Guid.NewGuid());

        Assert.Empty(result);
    }
}
