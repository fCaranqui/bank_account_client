using Client.Application.Exceptions;
using Client.Application.UseCases;

namespace Client.Tests.Application;

public class DeactivateClientUseCaseTests
{
    [Fact]
    public async Task Execute_ActiveClient_DeactivatesAndReturnsDto()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var cliente = await ClientRepositoryTestHelper.SeedCliente(repository);
        var useCase = new DeactivateClientUseCase(repository);

        var result = await useCase.Execute(cliente.Id);

        Assert.False(result.Estado);
    }

    [Fact]
    public async Task Execute_UnknownId_ThrowsClientNotFoundException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new DeactivateClientUseCase(repository);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => useCase.Execute(Guid.NewGuid()));
    }
}
