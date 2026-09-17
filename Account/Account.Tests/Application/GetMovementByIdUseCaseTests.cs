using Account.Application.Exceptions;
using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class GetMovementByIdUseCaseTests
{
    [Fact]
    public async Task Execute_ExistingMovement_ReturnsMovementDto()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        await repository.CreateAccount(cuenta);
        var movimiento = await repository.RegisterMovement(cuenta.Id, TipoMovimiento.Deposito, 20m);
        var useCase = new GetMovementByIdUseCase(repository);

        var result = await useCase.Execute(movimiento!.Id);

        Assert.Equal(movimiento.Id, result.Id);
        Assert.Equal(20m, result.Valor);
    }

    [Fact]
    public async Task Execute_NonExistingMovement_ThrowsMovementNotFoundException()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new GetMovementByIdUseCase(repository);

        await Assert.ThrowsAsync<MovementNotFoundException>(() => useCase.Execute(Guid.NewGuid()));
    }
}
