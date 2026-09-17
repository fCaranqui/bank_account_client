using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Application.UseCases;
using Client.Domain;

namespace Client.Tests.Application;

public class CreateClientUseCaseTests
{
    private static CreateClientDto ValidDto() => new()
    {
        Nombre = "Juan Perez",
        Genero = Genero.Masculino,
        Edad = 30,
        Identificacion = "1234567890",
        Direccion = "Calle 1",
        Telefono = "0999999999",
        Contrasena = "SuperSecret123",
    };

    [Fact]
    public async Task Execute_ValidData_CreatesClientWithHashedPassword()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new CreateClientUseCase(repository);

        var result = await useCase.Execute(ValidDto());

        Assert.NotEqual(Guid.Empty, result.Id);

        var stored = await repository.GetClientById(result.Id);
        Assert.NotNull(stored);
        Assert.NotEqual("SuperSecret123", stored!.ContrasenaHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("SuperSecret123", stored.ContrasenaHash));
    }

    [Fact]
    public async Task Execute_DuplicateIdentificacion_ThrowsDuplicateIdentificationException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        await ClientRepositoryTestHelper.SeedCliente(repository, "1234567890");
        var useCase = new CreateClientUseCase(repository);

        await Assert.ThrowsAsync<DuplicateIdentificationException>(() => useCase.Execute(ValidDto()));
    }
}
