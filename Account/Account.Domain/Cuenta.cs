using Account.Domain.Exceptions;

namespace Account.Domain;

public class Cuenta
{
    private readonly List<Movimiento> movimientos = new();

    public Guid Id { get; private set; }

    public string NumeroCuenta { get; private set; } = null!;

    public TipoCuenta TipoCuenta { get; private set; }

    public decimal SaldoInicial { get; private set; }

    public decimal SaldoDisponible { get; private set; }

    public bool Estado { get; private set; }

    public string ClienteId { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public IReadOnlyCollection<Movimiento> Movimientos => this.movimientos.AsReadOnly();

    protected Cuenta()
    {
    }

    public Cuenta(string numeroCuenta, TipoCuenta tipoCuenta, decimal saldoInicial, string clienteId)
    {
        if (string.IsNullOrWhiteSpace(numeroCuenta))
        {
            throw new ArgumentException("El número de cuenta es obligatorio.", nameof(numeroCuenta));
        }

        if (saldoInicial < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(saldoInicial), "El saldo inicial no puede ser negativo.");
        }

        if (string.IsNullOrWhiteSpace(clienteId))
        {
            throw new ArgumentException("ClienteId es obligatorio.", nameof(clienteId));
        }

        Id = Guid.NewGuid();
        NumeroCuenta = numeroCuenta;
        TipoCuenta = tipoCuenta;
        SaldoInicial = saldoInicial;
        SaldoDisponible = saldoInicial;
        Estado = true;
        ClienteId = clienteId;
    }

    public Movimiento RegisterMovement(TipoMovimiento tipoMovimiento, decimal valor)
    {
        if (!this.Estado)
        {
            throw new CuentaInactivaException();
        }

        if (valor == 0)
        {
            throw new ArgumentException("El valor del movimiento no puede ser cero.", nameof(valor));
        }

        var esRetiro = tipoMovimiento == TipoMovimiento.Retiro;
        var montoAplicado = esRetiro ? -Math.Abs(valor) : Math.Abs(valor);

        if (esRetiro && this.SaldoDisponible + montoAplicado < 0)
        {
            throw new SaldoNoDisponibleException();
        }

        this.SaldoDisponible += montoAplicado;

        var movimiento = new Movimiento(this.Id, tipoMovimiento, montoAplicado, this.SaldoDisponible);
        this.movimientos.Add(movimiento);
        return movimiento;
    }

    public void Deactivate() => this.Estado = false;

    public void Activate() => this.Estado = true;
}
