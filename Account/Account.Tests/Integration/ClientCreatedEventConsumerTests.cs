using Account.Application.Events;
using Account.Domain.Repository;
using Account.Infrastructure.Db;
using Account.Infrastructure.Events;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Tests.Integration;

public class ClientCreatedEventConsumerTests
{
    [Fact]
    public async Task Consume_ClientCreatedEvent_UpsertsClientReplica()
    {
        var databaseName = Guid.NewGuid().ToString();

        var services = new ServiceCollection();
        services.AddDbContext<AccountDbContext>(options => options.UseInMemoryDatabase(databaseName));
        services.AddScoped<IClientReplicaRepository, ClientReplicaRepository>();
        services.AddMassTransitTestHarness(x =>
        {
            x.AddConsumer<ClientCreatedEventConsumer>();
        });

        await using var provider = services.BuildServiceProvider(true);
        var harness = provider.GetTestHarness();

        await harness.Start();

        try
        {
            var clientId = Guid.NewGuid();

            await harness.Bus.Publish(new ClientCreatedEvent { ClientId = clientId });

            Assert.True(await harness.Consumed.Any<ClientCreatedEvent>());

            using var scope = provider.CreateScope();
            var clientReplicaRepository = scope.ServiceProvider.GetRequiredService<IClientReplicaRepository>();
            Assert.True(await clientReplicaRepository.Exists(clientId));
        }
        finally
        {
            await harness.Stop();
        }
    }
}
