using Client.Application.Common;
using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Domain.Repository;

namespace Client.Application.UseCases;

public class UpdateClientUseCase : IUseCase<UpdateClientRequest, ClientDto>
{
    private readonly IClienteRepository clienteRepository;

    public UpdateClientUseCase(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<ClientDto> Execute(UpdateClientRequest input)
    {
        var cliente = await this.clienteRepository.GetClientById(input.Id);
        if (cliente == null)
        {
            throw new ClientNotFoundException();
        }

        cliente.UpdateDetails(input.Dto.Nombre, input.Dto.Genero, input.Dto.Edad, input.Dto.Direccion, input.Dto.Telefono);
        await this.clienteRepository.UpdateClient(cliente);

        return ClientDtoFactory.CreateFromEntity(cliente);
    }
}
