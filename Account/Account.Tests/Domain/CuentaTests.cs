using Account.Domain;
using Account.Domain.Exceptions;

namespace Account.Tests.Domain;

public class CuentaTests
{
    [Fact]
    public void Constructor_ValidData_CreatesCuenta()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        Assert.Equal("001-001", cuenta.NumeroCuenta);
        Assert.Equal(TipoCuenta.Ahorro, cuenta.TipoCuenta);
        Assert.Equal(100m, cuenta.SaldoInicial);
        Assert.Equal(100m, cuenta.SaldoDisponible);
        Assert.True(cuenta.Estado);
    }

    [Fact]
    public void Constructor_EmptyNumeroCuenta_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Cuenta(string.Empty, TipoCuenta.Ahorro, 100m, Guid.NewGuid()));
    }

    [Fact]
    public void Constructor_NegativeSaldoInicial_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cuenta("001-001", TipoCuenta.Ahorro, -1m, Guid.NewGuid()));
    }

    [Fact]
    public void Constructor_EmptyClienteId_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.Empty));
    }

    [Fact]
    public void RegisterMovement_Deposito_IncreasesSaldoDisponible()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        var movimiento = cuenta.RegisterMovement(TipoMovimiento.Deposito, 50m);

        Assert.Equal(150m, cuenta.SaldoDisponible);
        Assert.Equal(50m, movimiento.Valor);
        Assert.Equal(150m, movimiento.Saldo);
    }

    [Fact]
    public void RegisterMovement_Retiro_DecreasesSaldoDisponible()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        var movimiento = cuenta.RegisterMovement(TipoMovimiento.Retiro, 40m);

        Assert.Equal(60m, cuenta.SaldoDisponible);
        Assert.Equal(-40m, movimiento.Valor);
    }

    [Fact]
    public void RegisterMovement_RetiroMayorQueSaldo_ThrowsSaldoNoDisponibleException()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        Assert.Throws<SaldoNoDisponibleException>(() => cuenta.RegisterMovement(TipoMovimiento.Retiro, 150m));
    }

    [Fact]
    public void RegisterMovement_CuentaDesactivada_ThrowsCuentaInactivaException()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        cuenta.Deactivate();

        Assert.Throws<CuentaInactivaException>(() => cuenta.RegisterMovement(TipoMovimiento.Deposito, 10m));
    }

    [Fact]
    public void RegisterMovement_ValorCero_Throws()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        Assert.Throws<ArgumentException>(() => cuenta.RegisterMovement(TipoMovimiento.Deposito, 0m));
    }

    [Fact]
    public void Deactivate_SetsEstadoFalse()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());

        cuenta.Deactivate();

        Assert.False(cuenta.Estado);
    }

    [Fact]
    public void Activate_SetsEstadoTrue()
    {
        var cuenta = new Cuenta("001-001", TipoCuenta.Ahorro, 100m, Guid.NewGuid());
        cuenta.Deactivate();

        cuenta.Activate();

        Assert.True(cuenta.Estado);
    }
}
