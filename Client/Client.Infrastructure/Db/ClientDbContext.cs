using Client.Domain;
using Microsoft.EntityFrameworkCore;

namespace Client.Infrastructure.Db;

public class ClientDbContext : DbContext
{
    public ClientDbContext(DbContextOptions<ClientDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => this.Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
    }
}
