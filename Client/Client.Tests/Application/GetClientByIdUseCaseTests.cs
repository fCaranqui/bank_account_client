using Client.Application.Exceptions;
using Client.Application.UseCases;

namespace Client.Tests.Application;

public class GetClientByIdUseCaseTests
{
    [Fact]
    public async Task Execute_ExistingId_ReturnsClientDto()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var cliente = await ClientRepositoryTestHelper.SeedCliente(repository);
        var useCase = new GetClientByIdUseCase(repository);

        var result = await useCase.Execute(cliente.Id);

        Assert.Equal(cliente.Id, result.Id);
        Assert.Equal(cliente.ClienteId, result.ClienteId);
    }

    [Fact]
    public async Task Execute_UnknownId_ThrowsClientNotFoundException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new GetClientByIdUseCase(repository);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => useCase.Execute(Guid.NewGuid()));
    }
}
