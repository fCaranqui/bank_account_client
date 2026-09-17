using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Client.Api.Contracts;
using Client.Application.DTO;
using Client.Domain;

namespace Client.Tests.Integration;

public class ClientesControllerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private static CreateClientDto ValidCreateDto(string? identificacion = null) => new()
    {
        Nombre = "Juan Perez",
        Genero = Genero.Masculino,
        Edad = 30,
        Identificacion = identificacion ?? Guid.NewGuid().ToString("N")[..10],
        Direccion = "Calle 1",
        Telefono = "0999999999",
        Contrasena = "secret123",
    };

    [Fact]
    public async Task Create_ValidData_Returns201WithCreatedClient()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-create-1"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);
        Assert.NotNull(body);
        Assert.True(body!.Success);
        Assert.Equal("it-create-1", body.Data!.Identificacion);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_DuplicateIdentificacion_Returns409()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-dup-1"));

        var response = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-dup-1"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.False(body!.Success);
        Assert.Equal("conflict", body.Errors![0].Code);
    }

    [Fact]
    public async Task Create_InvalidData_Returns400()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var dto = ValidCreateDto("it-invalid-1");
        dto.Nombre = string.Empty;

        var response = await client.PostAsJsonAsync("/clientes", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.False(body!.Success);
        Assert.Equal("invalid_argument", body.Errors![0].Code);
    }

    [Fact]
    public async Task GetById_ExistingClient_Returns200()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-getbyid-1"));
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);

        var response = await client.GetAsync($"/clientes/{created!.Data!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);
        Assert.Equal("it-getbyid-1", body!.Data!.Identificacion);
    }

    [Fact]
    public async Task GetById_MissingClient_Returns404()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/clientes/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("not_found", body!.Errors![0].Code);
    }

    [Fact]
    public async Task GetAll_ReturnsCreatedClients()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-getall-1"));

        var response = await client.GetAsync("/clientes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<List<ClientDto>>>(JsonOptions);
        Assert.Contains(body!.Data!, c => c.Identificacion == "it-getall-1");
    }

    [Fact]
    public async Task Update_ExistingClient_Returns200WithUpdatedData()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-update-1"));
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);

        var response = await client.PutAsJsonAsync(
            $"/clientes/{created!.Data!.Id}",
            new UpdateClientDto { Nombre = "Juan Actualizado", Genero = Genero.Otro, Edad = 40, Direccion = "Calle 2", Telefono = "0988888888" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);
        Assert.Equal("Juan Actualizado", body!.Data!.Nombre);
    }

    [Fact]
    public async Task Update_MissingClient_Returns404()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/clientes/{Guid.NewGuid()}",
            new UpdateClientDto { Nombre = "X", Genero = Genero.Otro, Edad = 40, Direccion = "Calle 2", Telefono = "0988888888" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingClient_Returns204()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-delete-1"));
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);

        var response = await client.DeleteAsync($"/clientes/{created!.Data!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Activate_ExistingClient_Returns200WithEstadoTrue()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-activate-1"));
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);
        await client.PatchAsync($"/clientes/{created!.Data!.Id}/desactivar", content: null);

        var response = await client.PatchAsync($"/clientes/{created.Data!.Id}/activar", content: null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);
        Assert.True(body!.Data!.Estado);
    }

    [Fact]
    public async Task Deactivate_ExistingClient_Returns200WithEstadoFalse()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-deactivate-1"));
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);

        var response = await client.PatchAsync($"/clientes/{created!.Data!.Id}/desactivar", content: null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);
        Assert.False(body!.Data!.Estado);
    }

    [Fact]
    public async Task UpdatePassword_CorrectCurrentPassword_Returns200()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-pwd-1"));
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);

        var response = await client.PatchAsJsonAsync(
            $"/clientes/{created!.Data!.Id}/contrasena",
            new UpdatePasswordDto { ContrasenaActual = "secret123", ContrasenaNueva = "newSecret456" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePassword_WrongCurrentPassword_Returns401()
    {
        using var factory = new ClientApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync("/clientes", ValidCreateDto("it-pwd-2"));
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<ClientDto>>(JsonOptions);

        var response = await client.PatchAsJsonAsync(
            $"/clientes/{created!.Data!.Id}/contrasena",
            new UpdatePasswordDto { ContrasenaActual = "wrong-password", ContrasenaNueva = "newSecret456" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("unauthorized", body!.Errors![0].Code);
    }
}
