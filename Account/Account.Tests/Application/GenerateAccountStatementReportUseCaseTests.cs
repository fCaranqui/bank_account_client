using Account.Application.DTO;
using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class GenerateAccountStatementReportUseCaseTests
{
    [Fact]
    public async Task Execute_ReturnsAccountsWithOnlyMovementsInsideRange()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var clienteId = Guid.NewGuid();
        var cuenta1 = new Cuenta("rep-cta-1", TipoCuenta.Ahorro, 500m, clienteId);
        var cuenta2 = new Cuenta("rep-cta-2", TipoCuenta.Corriente, 300m, clienteId);
        await repository.CreateAccount(cuenta1);
        await repository.CreateAccount(cuenta2);

        var desde = DateTime.UtcNow;
        await Task.Delay(10);
        await repository.RegisterMovement(cuenta1.Id, TipoMovimiento.Deposito, 100m);
        await repository.RegisterMovement(cuenta2.Id, TipoMovimiento.Deposito, 50m);
        await Task.Delay(10);
        var hasta = DateTime.UtcNow;
        await Task.Delay(10);
        await repository.RegisterMovement(cuenta1.Id, TipoMovimiento.Retiro, 20m);

        var useCase = new GenerateAccountStatementReportUseCase(repository);

        var result = await useCase.Execute(new GenerateAccountStatementReportRequest
        {
            ClienteId = clienteId,
            Desde = desde,
            Hasta = hasta,
        });

        Assert.Equal(clienteId, result.ClienteId);
        Assert.Equal(2, result.Cuentas.Count);

        var statement1 = result.Cuentas.Single(c => c.NumeroCuenta == "rep-cta-1");
        Assert.Single(statement1.Movimientos);
        Assert.Equal(100m, statement1.Movimientos[0].Valor);

        var statement2 = result.Cuentas.Single(c => c.NumeroCuenta == "rep-cta-2");
        Assert.Single(statement2.Movimientos);
        Assert.Equal(50m, statement2.Movimientos[0].Valor);
    }
}
