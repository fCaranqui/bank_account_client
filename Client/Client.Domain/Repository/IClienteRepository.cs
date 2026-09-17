namespace Client.Domain.Repository;

public interface IClienteRepository
{
    Task<Guid?> CreateClient(Cliente cliente);

    Task<Cliente?> GetClientById(Guid id);

    Task<bool> ExistsClientWithIdentification(string identificacion);

    Task<List<Cliente>> GetAllClients();

    Task<bool> UpdateClient(Cliente cliente);

    Task<bool> DeleteClient(Guid id);
}
