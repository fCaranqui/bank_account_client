using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Application.UseCases;
using Account.Domain;
using Account.Domain.Repository;

namespace Account.Tests.Application;

public class CreateAccountUseCaseTests
{
    [Fact]
    public async Task Execute_ValidData_CreatesAndReturnsAccountDto()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new CreateAccountUseCase(repository, new StubClientReplicaRepository(true));

        var dto = new CreateAccountDto
        {
            NumeroCuenta = "001-001",
            TipoCuenta = TipoCuenta.Ahorro,
            SaldoInicial = 500m,
            ClienteId = Guid.NewGuid(),
        };

        var result = await useCase.Execute(dto);

        Assert.Equal("001-001", result.NumeroCuenta);
        Assert.Equal(500m, result.SaldoDisponible);
        Assert.True(result.Estado);
    }

    [Fact]
    public async Task Execute_DuplicateNumeroCuenta_ThrowsDuplicateAccountNumberException()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new CreateAccountUseCase(repository, new StubClientReplicaRepository(true));
        var dto = new CreateAccountDto
        {
            NumeroCuenta = "001-001",
            TipoCuenta = TipoCuenta.Ahorro,
            SaldoInicial = 500m,
            ClienteId = Guid.NewGuid(),
        };
        await useCase.Execute(dto);

        var duplicate = new CreateAccountDto
        {
            NumeroCuenta = "001-001",
            TipoCuenta = TipoCuenta.Corriente,
            SaldoInicial = 100m,
            ClienteId = Guid.NewGuid(),
        };

        await Assert.ThrowsAsync<DuplicateAccountNumberException>(() => useCase.Execute(duplicate));
    }

    [Fact]
    public async Task Execute_ClientReplicaDoesNotExist_ThrowsClientNotFoundException()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new CreateAccountUseCase(repository, new StubClientReplicaRepository(false));
        var dto = new CreateAccountDto
        {
            NumeroCuenta = "001-002",
            TipoCuenta = TipoCuenta.Ahorro,
            SaldoInicial = 500m,
            ClienteId = Guid.NewGuid(),
        };

        await Assert.ThrowsAsync<ClientNotFoundException>(() => useCase.Execute(dto));
    }

    private sealed class StubClientReplicaRepository : IClientReplicaRepository
    {
        private readonly bool exists;

        public StubClientReplicaRepository(bool exists)
        {
            this.exists = exists;
        }

        public Task Upsert(Guid clientId, DateTime createdAt) => Task.CompletedTask;

        public Task<bool> Exists(Guid clientId) => Task.FromResult(this.exists);
    }
}
