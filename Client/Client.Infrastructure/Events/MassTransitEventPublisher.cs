using Client.Application.Common;
using Client.Application.Events;
using MassTransit;

namespace Client.Infrastructure.Events;

public class MassTransitEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    public async Task PublishClientCreatedAsync(Guid clientId)
    {
        await this.publishEndpoint.Publish(new ClientCreatedEvent { ClientId = clientId });
    }
}
