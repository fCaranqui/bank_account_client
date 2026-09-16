using Client.Application.Exceptions;
using Client.Application.UseCases;

namespace Client.Tests.Application;

public class ActivateClientUseCaseTests
{
    [Fact]
    public async Task Execute_DeactivatedClient_ActivatesAndReturnsDto()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var cliente = await ClientRepositoryTestHelper.SeedCliente(repository);
        cliente.Deactivate();
        await repository.UpdateClient(cliente);
        var useCase = new ActivateClientUseCase(repository);

        var result = await useCase.Execute(cliente.Id);

        Assert.True(result.Estado);
    }

    [Fact]
    public async Task Execute_UnknownId_ThrowsClientNotFoundException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new ActivateClientUseCase(repository);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => useCase.Execute(Guid.NewGuid()));
    }
}
