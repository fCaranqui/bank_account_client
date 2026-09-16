using Account.Domain;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Db;

public class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cuenta> Cuentas => this.Set<Cuenta>();

    public DbSet<Movimiento> Movimientos => this.Set<Movimiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CuentaConfiguration());
        modelBuilder.ApplyConfiguration(new MovimientoConfiguration());
    }
}
