using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Application.UseCases;
using Client.Domain;

namespace Client.Tests.Application;

public class UpdatePasswordUseCaseTests
{
    private static async Task<Cliente> SeedClienteWithPassword(Client.Infrastructure.Db.PgClienteRepository repository, string plainPassword)
    {
        var cliente = new Cliente(
            "Juan Perez",
            Genero.Masculino,
            30,
            "1234567890",
            "Calle 1",
            "0999999999",
            BCrypt.Net.BCrypt.HashPassword(plainPassword));

        await repository.CreateClient(cliente);
        return cliente;
    }

    [Fact]
    public async Task Execute_CorrectCurrentPassword_UpdatesHashAndReturnsTrue()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var cliente = await SeedClienteWithPassword(repository, "OldPassword1");
        var useCase = new UpdatePasswordUseCase(repository);
        var request = new UpdatePasswordRequest
        {
            ClienteId = cliente.Id,
            Dto = new UpdatePasswordDto { ContrasenaActual = "OldPassword1", ContrasenaNueva = "NewPassword2" },
        };

        var result = await useCase.Execute(request);

        Assert.True(result);
        var updated = await repository.GetClientById(cliente.Id);
        Assert.True(BCrypt.Net.BCrypt.Verify("NewPassword2", updated!.ContrasenaHash));
    }

    [Fact]
    public async Task Execute_WrongCurrentPassword_ThrowsIncorrectPasswordException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var cliente = await SeedClienteWithPassword(repository, "OldPassword1");
        var useCase = new UpdatePasswordUseCase(repository);
        var request = new UpdatePasswordRequest
        {
            ClienteId = cliente.Id,
            Dto = new UpdatePasswordDto { ContrasenaActual = "WrongPassword", ContrasenaNueva = "NewPassword2" },
        };

        await Assert.ThrowsAsync<IncorrectPasswordException>(() => useCase.Execute(request));
    }

    [Fact]
    public async Task Execute_UnknownClient_ThrowsClientNotFoundException()
    {
        var repository = ClientRepositoryTestHelper.CreateRepository();
        var useCase = new UpdatePasswordUseCase(repository);
        var request = new UpdatePasswordRequest
        {
            ClienteId = Guid.NewGuid(),
            Dto = new UpdatePasswordDto { ContrasenaActual = "OldPassword1", ContrasenaNueva = "NewPassword2" },
        };

        await Assert.ThrowsAsync<ClientNotFoundException>(() => useCase.Execute(request));
    }
}
