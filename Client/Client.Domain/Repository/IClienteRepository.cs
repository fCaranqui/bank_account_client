namespace Client.Domain.Repository;

public interface IClienteRepository
{
    Task<Guid?> CreateClient(Cliente cliente);

    Task<Cliente?> GetClientById(Guid id);

    Task<Cliente?> GetClientByClientId(string clienteId);

    Task<bool> ExistsClientWithIdentification(string identificacion);

    Task<bool> ExistsClientWithClientId(string clienteId);

    Task<List<Cliente>> GetAllClients();

    Task<bool> UpdateClient(Cliente cliente);

    Task<bool> DeleteClient(Guid id);
}
