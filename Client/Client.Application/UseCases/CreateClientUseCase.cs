using Client.Application.Common;
using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Domain;
using Client.Domain.Repository;

namespace Client.Application.UseCases;

public class CreateClientUseCase : IUseCase<CreateClientDto, ClientDto>
{
    private readonly IClienteRepository clienteRepository;
    private readonly IEventPublisher eventPublisher;

    public CreateClientUseCase(IClienteRepository clienteRepository, IEventPublisher eventPublisher)
    {
        this.clienteRepository = clienteRepository;
        this.eventPublisher = eventPublisher;
    }

    public async Task<ClientDto> Execute(CreateClientDto input)
    {
        if (await this.clienteRepository.ExistsClientWithIdentification(input.Identificacion))
        {
            throw new DuplicateIdentificationException();
        }

        var contrasenaHash = BCrypt.Net.BCrypt.HashPassword(input.Contrasena);

        var cliente = new Cliente(
            input.Nombre,
            input.Genero,
            input.Edad,
            input.Identificacion,
            input.Direccion,
            input.Telefono,
            contrasenaHash);

        await this.clienteRepository.CreateClient(cliente);

        // Published right after the DB write with no outbox: a broker outage at this exact
        // instant creates the client but loses the event. Accepted trade-off, not a bug.
        await this.eventPublisher.PublishClientCreatedAsync(cliente.Id);

        return ClientDtoFactory.CreateFromEntity(cliente);
    }
}
