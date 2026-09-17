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

public class MovimientosControllerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private static async Task<Guid> CreateAccountAsync(AccountApiFactory factory, HttpClient client, string numeroCuenta, decimal saldoInicial = 100m)
    {
        var clienteId = Guid.NewGuid();
        await SeedClientReplicaAsync(factory, clienteId);

        var response = await client.PostAsJsonAsync("/cuentas", new CreateAccountDto
        {
            NumeroCuenta = numeroCuenta,
            TipoCuenta = TipoCuenta.Ahorro,
            SaldoInicial = saldoInicial,
            ClienteId = clienteId,
        });
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<AccountDto>>(JsonOptions);
        return body!.Data!.Id;
    }

    private static async Task SeedClientReplicaAsync(AccountApiFactory factory, Guid clientId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
        dbContext.ClientReplicas.Add(new ClientReplica { ClientId = clientId, CreatedAt = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Register_ValidDeposito_Returns201WithUpdatedSaldo()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var cuentaId = await CreateAccountAsync(factory, client, "it-mov-deposito");

        var response = await client.PostAsJsonAsync("/movimientos", new RegisterMovementDto
        {
            CuentaId = cuentaId,
            TipoMovimiento = TipoMovimiento.Deposito,
            Valor = 50m,
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<MovementDto>>(JsonOptions);
        Assert.Equal(150m, body!.Data!.Saldo);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Register_RetiroMayorQueSaldo_Returns409()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var cuentaId = await CreateAccountAsync(factory, client, "it-mov-sinsaldo", saldoInicial: 10m);

        var response = await client.PostAsJsonAsync("/movimientos", new RegisterMovementDto
        {
            CuentaId = cuentaId,
            TipoMovimiento = TipoMovimiento.Retiro,
            Valor = 999m,
        });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("conflict", body!.Errors![0].Code);
    }

    [Fact]
    public async Task Register_CuentaDesactivada_Returns409()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var cuentaId = await CreateAccountAsync(factory, client, "it-mov-inactiva");
        await client.PutAsJsonAsync($"/cuentas/{cuentaId}", new UpdateAccountDto { Estado = false });

        var response = await client.PostAsJsonAsync("/movimientos", new RegisterMovementDto
        {
            CuentaId = cuentaId,
            TipoMovimiento = TipoMovimiento.Deposito,
            Valor = 10m,
        });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("conflict", body!.Errors![0].Code);
    }

    [Fact]
    public async Task Register_MissingCuenta_Returns404()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/movimientos", new RegisterMovementDto
        {
            CuentaId = Guid.NewGuid(),
            TipoMovimiento = TipoMovimiento.Deposito,
            Valor = 10m,
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ExistingMovement_Returns200()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var cuentaId = await CreateAccountAsync(factory, client, "it-mov-getbyid");
        var createResponse = await client.PostAsJsonAsync("/movimientos", new RegisterMovementDto
        {
            CuentaId = cuentaId,
            TipoMovimiento = TipoMovimiento.Deposito,
            Valor = 25m,
        });
        var created = await createResponse.Content.ReadFromJsonAsync<GatewayResponse<MovementDto>>(JsonOptions);

        var response = await client.GetAsync($"/movimientos/{created!.Data!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<MovementDto>>(JsonOptions);
        Assert.Equal(25m, body!.Data!.Valor);
    }

    [Fact]
    public async Task GetById_MissingMovement_Returns404()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/movimientos/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("not_found", body!.Errors![0].Code);
    }

    [Fact]
    public async Task GetByAccountId_NoTakeSpecified_StillReturnsResults()
    {
        // Regression test for the pagination bug fixed in this phase: an unspecified `take`
        // must not silently produce an always-empty result.
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var cuentaId = await CreateAccountAsync(factory, client, "it-mov-paginacion");
        await client.PostAsJsonAsync("/movimientos", new RegisterMovementDto { CuentaId = cuentaId, TipoMovimiento = TipoMovimiento.Deposito, Valor = 15m });

        var response = await client.GetAsync($"/movimientos/cuenta/{cuentaId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<List<MovementDto>>>(JsonOptions);
        Assert.Single(body!.Data!);
    }

    [Fact]
    public async Task GetByAccountId_TakeAboveMax_IsClampedTo100()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var cuentaId = await CreateAccountAsync(factory, client, "it-mov-clamp");
        await client.PostAsJsonAsync("/movimientos", new RegisterMovementDto { CuentaId = cuentaId, TipoMovimiento = TipoMovimiento.Deposito, Valor = 15m });

        var response = await client.GetAsync($"/movimientos/cuenta/{cuentaId}?take=99999");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<List<MovementDto>>>(JsonOptions);
        Assert.Single(body!.Data!);
    }
}
