namespace Client.Domain.Repository;

public interface IClienteRepository
{
    Task<Guid?> CreateCliente(Cliente cliente);

    Task<Cliente?> GetClienteById(Guid id);

    Task<Cliente?> GetClienteByClienteId(string clienteId);

    Task<bool> ExisteClienteConIdentificacion(string identificacion);

    Task<bool> ExisteClienteConClienteId(string clienteId);

    Task<List<Cliente>> GetAllClientes();

    Task<bool> UpdateCliente(Cliente cliente);

    Task<bool> DeleteCliente(Guid id);
}
