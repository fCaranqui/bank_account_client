using Client.Domain;
using Client.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Client.Infrastructure.Db;

public class PgClienteRepository : IClienteRepository
{
    private readonly ClientDbContext dbContext;

    public PgClienteRepository(ClientDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Guid?> CreateCliente(Cliente cliente)
    {
        var transaction = await this.dbContext.Database.BeginTransactionAsync();
        try
        {
            Log.Information("Creating cliente with ClienteId: {ClienteId}", cliente.ClienteId);

            var entry = await this.dbContext.Clientes.AddAsync(cliente);
            await this.dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            Log.Information("Cliente created with Id: {Id}", entry.Entity.Id);
            return entry.Entity.Id;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Log.Error(e, "Error creating cliente with ClienteId: {ClienteId}", cliente.ClienteId);
            return null;
        }
    }

    public async Task<Cliente?> GetClienteById(Guid id)
    {
        return await this.dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task<Cliente?> GetClienteByClienteId(string clienteId)
    {
        return await this.dbContext.Clientes.FirstOrDefaultAsync(c => c.ClienteId == clienteId && c.DeletedAt == null);
    }

    public async Task<bool> ExisteClienteConIdentificacion(string identificacion)
    {
        return await this.dbContext.Clientes.AnyAsync(c => c.Identificacion == identificacion && c.DeletedAt == null);
    }

    public async Task<bool> ExisteClienteConClienteId(string clienteId)
    {
        return await this.dbContext.Clientes.AnyAsync(c => c.ClienteId == clienteId && c.DeletedAt == null);
    }

    public async Task<List<Cliente>> GetAllClientes()
    {
        return await this.dbContext.Clientes.Where(c => c.DeletedAt == null).ToListAsync();
    }

    public async Task<bool> UpdateCliente(Cliente cliente)
    {
        var transaction = await this.dbContext.Database.BeginTransactionAsync();
        try
        {
            this.dbContext.Clientes.Update(cliente);
            await this.dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            Log.Information("Cliente with Id {Id} updated successfully", cliente.Id);
            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Log.Error(e, "Error updating cliente with Id {Id}", cliente.Id);
            return false;
        }
    }

    public async Task<bool> DeleteCliente(Guid id)
    {
        var cliente = await this.dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente == null)
        {
            Log.Warning("Cliente with Id {Id} not found for deletion", id);
            return false;
        }

        cliente.Desactivar();
        return await this.UpdateCliente(cliente);
    }
}
