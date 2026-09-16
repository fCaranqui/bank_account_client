using Client.Application.Common;
using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Domain.Repository;

namespace Client.Application.UseCases;

public class DeactivateClientUseCase : IUseCase<Guid, ClientDto>
{
    private readonly IClienteRepository clienteRepository;

    public DeactivateClientUseCase(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<ClientDto> Execute(Guid input)
    {
        var cliente = await this.clienteRepository.GetClientById(input);
        if (cliente == null)
        {
            throw new ClientNotFoundException();
        }

        cliente.Deactivate();
        await this.clienteRepository.UpdateClient(cliente);

        return ClientDtoFactory.CreateFromEntity(cliente);
    }
}
