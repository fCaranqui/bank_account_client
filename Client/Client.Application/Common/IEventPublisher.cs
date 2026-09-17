namespace Client.Application.Common;

public interface IEventPublisher
{
    Task PublishClientCreatedAsync(Guid clientId);
}
