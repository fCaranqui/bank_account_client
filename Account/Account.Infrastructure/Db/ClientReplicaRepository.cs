using Account.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Account.Infrastructure.Db;

public class ClientReplicaRepository : IClientReplicaRepository
{
    private readonly AccountDbContext dbContext;

    public ClientReplicaRepository(AccountDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Upsert(Guid clientId, DateTime createdAt)
    {
        try
        {
            var replica = await this.dbContext.ClientReplicas.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (replica == null)
            {
                await this.dbContext.ClientReplicas.AddAsync(new ClientReplica { ClientId = clientId, CreatedAt = createdAt });
            }
            else
            {
                replica.CreatedAt = createdAt;
            }

            await this.dbContext.SaveChangesAsync();

            Log.Information("ClientReplica upserted for ClientId: {ClientId}", clientId);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error upserting ClientReplica for ClientId: {ClientId}", clientId);
        }
    }

    public async Task<bool> Exists(Guid clientId)
    {
        return await this.dbContext.ClientReplicas.AnyAsync(x => x.ClientId == clientId);
    }
}
