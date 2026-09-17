namespace Account.Domain.Repository;

public interface IClientReplicaRepository
{
    Task Upsert(Guid clientId, DateTime createdAt);

    Task<bool> Exists(Guid clientId);
}
