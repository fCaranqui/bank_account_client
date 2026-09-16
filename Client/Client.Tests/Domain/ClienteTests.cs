using Client.Domain;

namespace Client.Tests.Domain;

public class ClienteTests
{
    private static Cliente CreateValidCliente() =>
        new(
            "Juan Perez",
            Genero.Masculino,
            30,
            "1234567890",
            "Calle 1",
            "0999999999",
            "cli-001",
            "hashed-password");

    [Fact]
    public void Constructor_ValidData_CreatesActiveCliente()
    {
        var cliente = CreateValidCliente();

        Assert.Equal("cli-001", cliente.ClienteId);
        Assert.Equal("hashed-password", cliente.ContrasenaHash);
        Assert.True(cliente.Estado);
        Assert.Null(cliente.DeletedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_InvalidClienteId_ThrowsArgumentException(string? clienteId)
    {
        Assert.Throws<ArgumentException>(() =>
            new Cliente("Juan Perez", Genero.Masculino, 30, "1234567890", "Calle 1", "0999999999", clienteId!, "hashed-password"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_InvalidContrasenaHash_ThrowsArgumentException(string? contrasenaHash)
    {
        Assert.Throws<ArgumentException>(() =>
            new Cliente("Juan Perez", Genero.Masculino, 30, "1234567890", "Calle 1", "0999999999", "cli-001", contrasenaHash!));
    }

    [Fact]
    public void Activate_SetsEstadoTrue()
    {
        var cliente = CreateValidCliente();
        cliente.Deactivate();

        cliente.Activate();

        Assert.True(cliente.Estado);
    }

    [Fact]
    public void Deactivate_SetsEstadoFalse()
    {
        var cliente = CreateValidCliente();

        cliente.Deactivate();

        Assert.False(cliente.Estado);
    }

    [Fact]
    public void Delete_SetsDeletedAt_AndKeepsEstadoUntouched()
    {
        var cliente = CreateValidCliente();

        cliente.Delete();

        Assert.NotNull(cliente.DeletedAt);
        Assert.True(cliente.Estado);
    }

    [Fact]
    public void UpdateDetails_ValidData_KeepsClienteOnlyFieldsUntouched()
    {
        var cliente = CreateValidCliente();

        cliente.UpdateDetails("Juan Actualizado", Genero.Otro, 40, "Calle 2", "0988888888");

        Assert.Equal("Juan Actualizado", cliente.Nombre);
        Assert.Equal("cli-001", cliente.ClienteId);
        Assert.Equal("hashed-password", cliente.ContrasenaHash);
        Assert.True(cliente.Estado);
    }

    [Fact]
    public void SetPasswordHash_ValidHash_UpdatesContrasenaHash()
    {
        var cliente = CreateValidCliente();

        cliente.SetPasswordHash("new-hashed-password");

        Assert.Equal("new-hashed-password", cliente.ContrasenaHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SetPasswordHash_InvalidHash_ThrowsArgumentException(string? hash)
    {
        var cliente = CreateValidCliente();

        Assert.Throws<ArgumentException>(() => cliente.SetPasswordHash(hash!));
    }
}
