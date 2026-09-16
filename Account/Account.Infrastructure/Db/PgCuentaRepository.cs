using Account.Domain;
using Account.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Account.Infrastructure.Db;

public class PgCuentaRepository : ICuentaRepository
{
    private readonly AccountDbContext dbContext;

    public PgCuentaRepository(AccountDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Guid?> CreateCuenta(Cuenta cuenta)
    {
        try
        {
            Log.Information("Creating cuenta with NumeroCuenta: {NumeroCuenta}", cuenta.NumeroCuenta);

            var entry = await this.dbContext.Cuentas.AddAsync(cuenta);
            await this.dbContext.SaveChangesAsync();

            Log.Information("Cuenta created with Id: {Id}", entry.Entity.Id);
            return entry.Entity.Id;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error creating cuenta with NumeroCuenta: {NumeroCuenta}", cuenta.NumeroCuenta);
            return null;
        }
    }

    public async Task<Cuenta?> GetCuentaById(Guid id)
    {
        return await this.dbContext.Cuentas.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task<Cuenta?> GetCuentaByNumeroCuenta(string numeroCuenta)
    {
        return await this.dbContext.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta && c.DeletedAt == null);
    }

    public async Task<bool> ExisteCuentaConNumeroCuenta(string numeroCuenta)
    {
        return await this.dbContext.Cuentas.AnyAsync(c => c.NumeroCuenta == numeroCuenta && c.DeletedAt == null);
    }

    public async Task<List<Cuenta>> GetCuentasByClienteId(string clienteId)
    {
        return await this.dbContext.Cuentas.Where(c => c.ClienteId == clienteId && c.DeletedAt == null).ToListAsync();
    }

    public async Task<List<Cuenta>> GetAllCuentas()
    {
        return await this.dbContext.Cuentas.Where(c => c.DeletedAt == null).ToListAsync();
    }

    public async Task<bool> UpdateCuenta(Cuenta cuenta)
    {
        try
        {
            this.dbContext.Cuentas.Update(cuenta);
            await this.dbContext.SaveChangesAsync();

            Log.Information("Cuenta with Id {Id} updated successfully", cuenta.Id);
            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error updating cuenta with Id {Id}", cuenta.Id);
            return false;
        }
    }

    public async Task<Movimiento?> RegistrarMovimiento(Guid cuentaId, TipoMovimiento tipoMovimiento, decimal valor)
    {
        var cuenta = await this.dbContext.Cuentas.FirstOrDefaultAsync(c => c.Id == cuentaId);
        if (cuenta == null)
        {
            Log.Warning("Cuenta with Id {Id} not found for RegistrarMovimiento", cuentaId);
            return null;
        }

        // Domain invariants (F2/F3) run here. SaldoNoDisponibleException and
        // CuentaInactivaException must propagate uncaught — they are expected business-rule
        // violations for the exception middleware to map, not infrastructure failures.
        var movimiento = cuenta.RegistrarMovimiento(tipoMovimiento, valor);

        try
        {
            await this.dbContext.Movimientos.AddAsync(movimiento);
            await this.dbContext.SaveChangesAsync();

            Log.Information("Movimiento {MovimientoId} registered for Cuenta {CuentaId}", movimiento.Id, cuentaId);
            return movimiento;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error persisting movimiento for Cuenta {CuentaId}", cuentaId);
            return null;
        }
    }

    public async Task<Movimiento?> GetMovimientoById(Guid movimientoId)
    {
        return await this.dbContext.Movimientos.FirstOrDefaultAsync(m => m.Id == movimientoId && m.DeletedAt == null);
    }

    public async Task<List<Movimiento>> GetMovimientosByCuentaId(
        Guid cuentaId,
        DateTime? desde = null,
        DateTime? hasta = null,
        int? skip = null,
        int? take = null)
    {
        var query = this.dbContext.Movimientos.Where(m => m.CuentaId == cuentaId && m.DeletedAt == null);

        if (desde.HasValue)
        {
            query = query.Where(m => m.Fecha >= desde.Value);
        }

        if (hasta.HasValue)
        {
            query = query.Where(m => m.Fecha <= hasta.Value);
        }

        query = query.OrderBy(m => m.Fecha);

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }
}
