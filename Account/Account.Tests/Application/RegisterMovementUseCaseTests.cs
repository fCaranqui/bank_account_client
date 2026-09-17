using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Application.UseCases;
using Account.Domain;
using Account.Domain.Exceptions;

namespace Account.Tests.Application;

public class RegisterMovementUseCaseTests
{
    [Fact]
    public async Task Execute_Deposito_IncreasesSaldoAndReturnsMovementDto()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        await repository.CreateAccount(cuenta);
        var useCase = new RegisterMovementUseCase(repository);

        var result = await useCase.Execute(new RegisterMovementDto { CuentaId = cuenta.Id, TipoMovimiento = TipoMovimiento.Deposito, Valor = 50m });

        Assert.Equal(50m, result.Valor);
        Assert.Equal(150m, result.Saldo);
    }

    [Fact]
    public async Task Execute_NonExistingAccount_ThrowsAccountNotFoundException()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new RegisterMovementUseCase(repository);

        await Assert.ThrowsAsync<AccountNotFoundException>(
            () => useCase.Execute(new RegisterMovementDto { CuentaId = Guid.NewGuid(), TipoMovimiento = TipoMovimiento.Deposito, Valor = 10m }));
    }

    [Fact]
    public async Task Execute_RetiroMayorQueSaldo_PropagatesSaldoNoDisponibleExceptionUncaught()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        await repository.CreateAccount(cuenta);
        var useCase = new RegisterMovementUseCase(repository);

        await Assert.ThrowsAsync<SaldoNoDisponibleException>(
            () => useCase.Execute(new RegisterMovementDto { CuentaId = cuenta.Id, TipoMovimiento = TipoMovimiento.Retiro, Valor = 500m }));
    }

    [Fact]
    public async Task Execute_CuentaDesactivada_PropagatesCuentaInactivaExceptionUncaught()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        cuenta.Deactivate();
        await repository.CreateAccount(cuenta);
        var useCase = new RegisterMovementUseCase(repository);

        await Assert.ThrowsAsync<CuentaInactivaException>(
            () => useCase.Execute(new RegisterMovementDto { CuentaId = cuenta.Id, TipoMovimiento = TipoMovimiento.Deposito, Valor = 10m }));
    }
}
