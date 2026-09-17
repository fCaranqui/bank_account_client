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

public class ReportesControllerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private static async Task SeedClientReplicaAsync(AccountApiFactory factory, Guid clientId)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
        dbContext.ClientReplicas.Add(new ClientReplica { ClientId = clientId, CreatedAt = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetStatement_ValidRange_ReturnsAccountsWithMovements()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var clienteId = Guid.NewGuid();
        await SeedClientReplicaAsync(factory, clienteId);
        await client.PostAsJsonAsync("/cuentas", new CreateAccountDto
        {
            NumeroCuenta = "it-rep-1",
            TipoCuenta = TipoCuenta.Ahorro,
            SaldoInicial = 100m,
            ClienteId = clienteId,
        });
        var desde = Uri.EscapeDataString(DateTime.UtcNow.AddMinutes(-1).ToString("O"));
        var hasta = Uri.EscapeDataString(DateTime.UtcNow.AddMinutes(1).ToString("O"));

        var response = await client.GetAsync($"/reportes?clienteId={clienteId}&desde={desde}&hasta={hasta}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<AccountStatementReportDto>>(JsonOptions);
        Assert.True(body!.Success);
        Assert.Equal(clienteId, body.Data!.ClienteId);
        Assert.Contains(body.Data!.Cuentas, c => c.NumeroCuenta == "it-rep-1");
    }

    [Fact]
    public async Task GetStatement_DesdeAfterHasta_Returns400()
    {
        using var factory = new AccountApiFactory();
        using var client = factory.CreateClient();
        var clienteId = Guid.NewGuid();
        var desde = Uri.EscapeDataString(DateTime.UtcNow.ToString("O"));
        var hasta = Uri.EscapeDataString(DateTime.UtcNow.AddDays(-1).ToString("O"));

        var response = await client.GetAsync($"/reportes?clienteId={clienteId}&desde={desde}&hasta={hasta}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<GatewayResponse<object>>(JsonOptions);
        Assert.Equal("invalid_argument", body!.Errors![0].Code);
    }
}
