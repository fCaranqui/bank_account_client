using Client.Domain;
using Client.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Client.Tests.Application;

public static class ClientRepositoryTestHelper
{
    public static PgClienteRepository CreateRepository()
    {
        var options = new DbContextOptionsBuilder<ClientDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PgClienteRepository(new ClientDbContext(options));
    }

    public static async Task<Cliente> SeedCliente(PgClienteRepository repository, string identificacion = "1234567890")
    {
        var cliente = new Cliente(
            "Juan Perez",
            Genero.Masculino,
            30,
            identificacion,
            "Calle 1",
            "0999999999",
            "hashed-password");

        await repository.CreateClient(cliente);
        return cliente;
    }
}
