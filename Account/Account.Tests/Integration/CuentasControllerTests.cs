using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Api.Contracts;
using Account.Application.DTO;
using Account.Domain;
using Account.Infrastructure.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Tests.Integration;

public class CuentasControllerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private static CreateAccountDto ValidCreateDto(string numeroCuenta) => new()
    {
        NumeroCuenta = numeroCuenta,
        TipoCuenta = TipoCuenta.Ahorro,
        SaldoInicial = 100m,
        ClienteId = Guid.NewGuid(),
    };

    private static async Task SeedClientReplicaAsync(AccountApiFactory factory, Guid clientId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
        dbContext.ClientReplicas.Add(new ClientReplica { ClientId = clientId, CreatedAt = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Create_ValidData_Returns201WithCreatedAccount()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var dto = ValidCreateDto("it-cta-1");
        await SeedClientReplicaAsync(factory, dto.ClienteId);

        var response = await client.PostAsJsonAsync("/cuentas", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<AccountDto>>(JsonOptions);
        Assert.True(body!.Success);
        Assert.Equal("it-cta-1", body.Data!.NumeroCuenta);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_DuplicateNumeroCuenta_Returns409()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var firstDto = ValidCreateDto("it-cta-dup");
        await SeedClientReplicaAsync(factory, firstDto.ClienteId);
        await client.PostAsJsonAsync("/cuentas", firstDto);

        var response = await client.PostAsJsonAsync("/cuentas", ValidCreateDto("it-cta-dup"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("conflict", body!.Errors![0].Code);
    }

    [Fact]
    public async Task Create_ClientReplicaMissing_Returns404()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/cuentas", ValidCreateDto("it-cta-noclient"));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("not_found", body!.Errors![0].Code);
    }

    [Fact]
    public async Task Create_InvalidData_Returns400()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var dto = ValidCreateDto(string.Empty);
        await SeedClientReplicaAsync(factory, dto.ClienteId);

        var response = await client.PostAsJsonAsync("/cuentas", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("invalid_argument", body!.Errors![0].Code);
    }

    [Fact]
    public async Task GetById_ExistingAccount_Returns200()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var dto = ValidCreateDto("it-cta-getbyid");
        await SeedClientReplicaAsync(factory, dto.ClienteId);
        var createResponse = await client.PostAsJsonAsync("/cuentas", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<AccountDto>>(JsonOptions);

        var response = await client.GetAsync($"/cuentas/{created!.Data!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<AccountDto>>(JsonOptions);
        Assert.Equal("it-cta-getbyid", body!.Data!.NumeroCuenta);
    }

    [Fact]
    public async Task GetById_MissingAccount_Returns404()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/cuentas/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("not_found", body!.Errors![0].Code);
    }

    [Fact]
    public async Task GetAll_ReturnsCreatedAccounts()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var dto = ValidCreateDto("it-cta-getall");
        await SeedClientReplicaAsync(factory, dto.ClienteId);
        await client.PostAsJsonAsync("/cuentas", dto);

        var response = await client.GetAsync("/cuentas");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<List<AccountDto>>>(JsonOptions);
        Assert.Contains(body!.Data!, a => a.NumeroCuenta == "it-cta-getall");
    }

    [Fact]
    public async Task GetByClientId_ReturnsAccountsForThatClient()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var clienteId = Guid.NewGuid();
        var dto = ValidCreateDto("it-cta-byclient");
        dto.ClienteId = clienteId;
        await SeedClientReplicaAsync(factory, clienteId);
        await client.PostAsJsonAsync("/cuentas", dto);

        var response = await client.GetAsync($"/cuentas/cliente/{clienteId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<List<AccountDto>>>(JsonOptions);
        Assert.Contains(body!.Data!, a => a.NumeroCuenta == "it-cta-byclient");
    }

    [Fact]
    public async Task Update_ExistingAccount_Returns200WithUpdatedEstado()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var dto = ValidCreateDto("it-cta-update");
        await SeedClientReplicaAsync(factory, dto.ClienteId);
        var createResponse = await client.PostAsJsonAsync("/cuentas", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<AccountDto>>(JsonOptions);

        var response = await client.PutAsJsonAsync($"/cuentas/{created!.Data!.Id}", new UpdateAccountDto { Estado = false });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<AccountDto>>(JsonOptions);
        Assert.False(body!.Data!.Estado);
    }

    [Fact]
    public async Task Update_MissingAccount_Returns404()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync($"/cuentas/{Guid.NewGuid()}", new UpdateAccountDto { Estado = false });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
