namespace Account.Domain;

public class Movimiento
{
    public Guid Id { get; private set; }

    public Guid CuentaId { get; private set; }

    public DateTime Fecha { get; private set; }

    public TipoMovimiento TipoMovimiento { get; private set; }

    public decimal Valor { get; private set; }

    public decimal Saldo { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    protected Movimiento()
    {
    }

    internal Movimiento(Guid cuentaId, TipoMovimiento tipoMovimiento, decimal valor, decimal saldoResultante)
    {
        Id = Guid.NewGuid();
        CuentaId = cuentaId;
        Fecha = DateTime.UtcNow;
        TipoMovimiento = tipoMovimiento;
        Valor = valor;
        Saldo = saldoResultante;
    }
}
