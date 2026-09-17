using Account.Application.Events;
using Account.Domain.Repository;
using MassTransit;

namespace Account.Infrastructure.Events;

public class ClientCreatedEventConsumer : IConsumer<ClientCreatedEvent>
{
    private readonly IClientReplicaRepository clientReplicaRepository;

    public ClientCreatedEventConsumer(IClientReplicaRepository clientReplicaRepository)
    {
        this.clientReplicaRepository = clientReplicaRepository;
    }

    public async Task Consume(ConsumeContext<ClientCreatedEvent> context)
    {
        await this.clientReplicaRepository.Upsert(context.Message.ClientId, DateTime.UtcNow);
    }
}
