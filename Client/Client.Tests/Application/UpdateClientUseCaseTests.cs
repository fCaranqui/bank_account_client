using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Application.UseCases;
using Client.Domain;

namespace Client.Tests.Application;

public class UpdateClientUseCaseTests
{
    [Fact]
    public async Task Execute_ExistingClient_UpdatesAndReturnsDto()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var cliente = await ClientRepositoryTestHelper.SeedCliente(repository);
        var useCase = new UpdateClientUseCase(repository);
        var request = new UpdateClientRequest
        {
            Id = cliente.Id,
            Dto = new UpdateClientDto
            {
                Nombre = "Nuevo Nombre",
                Genero = Genero.Otro,
                Edad = 40,
                Direccion = "Nueva Calle",
                Telefono = "0988888888",
            },
        };

        var result = await useCase.Execute(request);

        Assert.Equal("Nuevo Nombre", result.Nombre);
        Assert.Equal(40, result.Edad);
    }

    [Fact]
    public async Task Execute_UnknownId_ThrowsClientNotFoundException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new UpdateClientUseCase(repository);
        var request = new UpdateClientRequest
        {
            Id = Guid.NewGuid(),
            Dto = new UpdateClientDto
            {
                Nombre = "Nuevo Nombre",
                Genero = Genero.Otro,
                Edad = 40,
                Direccion = "Nueva Calle",
                Telefono = "0988888888",
            },
        };

        await Assert.ThrowsAsync<ClientNotFoundException>(() => useCase.Execute(request));
    }
}
