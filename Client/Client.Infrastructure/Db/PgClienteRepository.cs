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

    public async Task<Guid?> CreateClient(Cliente cliente)
    {
        try
        {
            Log.Information("Creating cliente with Identificacion: {Identificacion}", cliente.Identificacion);

            var entry = await this.dbContext.Clientes.AddAsync(cliente);
            await this.dbContext.SaveChangesAsync();

            Log.Information("Cliente created with Id: {Id}", entry.Entity.Id);
            return entry.Entity.Id;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error creating cliente with Identificacion: {Identificacion}", cliente.Identificacion);
            return null;
        }
    }

    public async Task<Cliente?> GetClientById(Guid id)
    {
        return await this.dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task<bool> ExistsClientWithIdentification(string identificacion)
    {
        return await this.dbContext.Clientes.AnyAsync(c => c.Identificacion == identificacion && c.DeletedAt == null);
    }

    public async Task<List<Cliente>> GetAllClients()
    {
        return await this.dbContext.Clientes.Where(c => c.DeletedAt == null).ToListAsync();
    }

    public async Task<bool> UpdateClient(Cliente cliente)
    {
        try
        {
            this.dbContext.Clientes.Update(cliente);
            await this.dbContext.SaveChangesAsync();

            Log.Information("Cliente with Id {Id} updated successfully", cliente.Id);
            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error updating cliente with Id {Id}", cliente.Id);
            return false;
        }
    }

    public async Task<bool> DeleteClient(Guid id)
    {
        var cliente = await this.dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente == null)
        {
            Log.Warning("Cliente with Id {Id} not found for deletion", id);
            return false;
        }

        cliente.Delete();
        return await this.UpdateClient(cliente);
    }
}
