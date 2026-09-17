using Account.Domain;

namespace Account.Tests.Domain;

public class MovimientoTests
{
    [Fact]
    public void RegisterMovement_Deposito_CreatesMovimientoWithPositiveValor()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        var movimiento = cuenta.RegisterMovement(TipoMovimiento.Deposito, 25m);

        Assert.Equal(TipoMovimiento.Deposito, movimiento.TipoMovimiento);
        Assert.Equal(25m, movimiento.Valor);
        Assert.Equal(cuenta.Id, movimiento.CuentaId);
    }

    [Fact]
    public void RegisterMovement_Retiro_CreatesMovimientoWithNegativeValor()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        var movimiento = cuenta.RegisterMovement(TipoMovimiento.Retiro, 25m);

        Assert.Equal(TipoMovimiento.Retiro, movimiento.TipoMovimiento);
        Assert.Equal(-25m, movimiento.Valor);
    }

    [Fact]
    public void RegisterMovement_AddsToCuentaMovimientosCollection()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        cuenta.RegisterMovement(TipoMovimiento.Deposito, 10m);
        cuenta.RegisterMovement(TipoMovimiento.Deposito, 20m);

        Assert.Equal(2, cuenta.Movimientos.Count);
    }

    [Fact]
    public void RegisterMovement_SaldoReflectsRunningBalance()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        cuenta.RegisterMovement(TipoMovimiento.Deposito, 50m);
        var segundo = cuenta.RegisterMovement(TipoMovimiento.Retiro, 30m);

        Assert.Equal(120m, segundo.Saldo);
    }
}
