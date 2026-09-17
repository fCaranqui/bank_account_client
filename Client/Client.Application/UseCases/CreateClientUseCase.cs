using Client.Application.Common;
using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Domain;
using Client.Domain.Repository;

namespace Client.Application.UseCases;

public class CreateClientUseCase : IUseCase<CreateClientDto, ClientDto>
{
    private readonly IClienteRepository clienteRepository;

    public CreateClientUseCase(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
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

        return ClientDtoFactory.CreateFromEntity(cliente);
    }
}
