using Account.Application.DTO;
using Account.Application.Exceptions;
using Account.Application.UseCases;
using Account.Domain;

namespace Account.Tests.Application;

public class CreateAccountUseCaseTests
{
    [Fact]
    public async Task Execute_ValidData_CreatesAndReturnsAccountDto()
    {
        var repository = TestDbContextFactory.CreateRepository();
        var useCase = new CreateAccountUseCase(repository);

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
        var useCase = new CreateAccountUseCase(repository);
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
}
