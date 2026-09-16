using Client.Application.Exceptions;
using Client.Application.UseCases;

namespace Client.Tests.Application;

public class DeleteClientUseCaseTests
{
    [Fact]
    public async Task Execute_ExistingClient_SoftDeletesAndReturnsTrue()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var cliente = await ClientRepositoryTestHelper.SeedCliente(repository);
        var useCase = new DeleteClientUseCase(repository);

        var result = await useCase.Execute(cliente.Id);

        Assert.True(result);
        Assert.Null(await repository.GetClientById(cliente.Id));
    }

    [Fact]
    public async Task Execute_UnknownId_ThrowsClientNotFoundException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new DeleteClientUseCase(repository);

        await Assert.ThrowsAsync<ClientNotFoundException>(() => useCase.Execute(Guid.NewGuid()));
    }
}
