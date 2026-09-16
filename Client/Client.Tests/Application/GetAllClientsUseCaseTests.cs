using Client.Application.Common;
using Client.Application.UseCases;

namespace Client.Tests.Application;

public class GetAllClientsUseCaseTests
{
    [Fact]
    public async Task Execute_NoClients_ReturnsEmptyList()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new GetAllClientsUseCase(repository);

        var result = await useCase.Execute(Unit.Value);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Execute_MultipleClients_ReturnsAll()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        await ClientRepositoryTestHelper.SeedCliente(repository, "cli-001", "1111111111");
        await ClientRepositoryTestHelper.SeedCliente(repository, "cli-002", "2222222222");
        var useCase = new GetAllClientsUseCase(repository);

        var result = await useCase.Execute(Unit.Value);

        Assert.Equal(2, result.Count);
    }
}
