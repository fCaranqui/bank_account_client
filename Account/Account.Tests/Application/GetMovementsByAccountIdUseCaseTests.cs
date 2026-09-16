using Account.Application.DTO;
using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class GetMovementsByAccountIdUseCaseTests
{
    [Fact]
    public async Task Execute_ReturnsMovementsForGivenAccount()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, "CLI-001");
        await repository.CreateAccount(cuenta);
        await repository.RegisterMovement(cuenta.Id, TipoMovimiento.Deposito, 10m);
        await repository.RegisterMovement(cuenta.Id, TipoMovimiento.Deposito, 20m);
        var useCase = new GetMovementsByAccountIdUseCase(repository);

        var result = await useCase.Execute(new GetMovementsByAccountIdRequest { CuentaId = cuenta.Id, Skip = 0, Take = 10 });

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Execute_SkipTake_PaginatesResults()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, "CLI-001");
        await repository.CreateAccount(cuenta);
        await repository.RegisterMovement(cuenta.Id, TipoMovimiento.Deposito, 10m);
        await repository.RegisterMovement(cuenta.Id, TipoMovimiento.Deposito, 20m);
        await repository.RegisterMovement(cuenta.Id, TipoMovimiento.Deposito, 30m);
        var useCase = new GetMovementsByAccountIdUseCase(repository);

        var result = await useCase.Execute(new GetMovementsByAccountIdRequest { CuentaId = cuenta.Id, Skip = 1, Take = 1 });

        Assert.Single(result);
        Assert.Equal(20m, result[0].Valor);
    }

    [Fact]
    public async Task Execute_NoMovementsForAccount_ReturnsEmptyList()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, "CLI-001");
        await repository.CreateAccount(cuenta);
        var useCase = new GetMovementsByAccountIdUseCase(repository);

        var result = await useCase.Execute(new GetMovementsByAccountIdRequest { CuentaId = cuenta.Id, Skip = 0, Take = 10 });

        Assert.Empty(result);
    }
}
