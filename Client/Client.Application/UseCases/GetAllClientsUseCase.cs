using Client.Application.Common;
using Client.Application.DTO;
using Client.Domain.Repository;

namespace Client.Application.UseCases;

public class GetAllClientsUseCase : IUseCase<Unit, List<ClientDto>>
{
    private readonly IClienteRepository clienteRepository;

    public GetAllClientsUseCase(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<List<ClientDto>> Execute(Unit input)
    {
        var clientes = await this.clienteRepository.GetAllClients();
        return clientes.Select(ClientDtoFactory.CreateFromEntity).ToList();
    }
}
