using Client.Application.Common;
using Client.Application.DTO;
using Client.Application.Exceptions;
using Client.Domain.Repository;

namespace Client.Application.UseCases;

public class UpdatePasswordUseCase : IUseCase<UpdatePasswordRequest, bool>
{
    private readonly IClienteRepository clienteRepository;

    public UpdatePasswordUseCase(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<bool> Execute(UpdatePasswordRequest input)
    {
        var cliente = await this.clienteRepository.GetClientById(input.ClienteId);
        if (cliente == null)
        {
            throw new ClientNotFoundException();
        }

        if (!BCrypt.Net.BCrypt.Verify(input.Dto.ContrasenaActual, cliente.ContrasenaHash))
        {
            throw new IncorrectPasswordException();
        }

        var nuevoHash = BCrypt.Net.BCrypt.HashPassword(input.Dto.ContrasenaNueva);
        cliente.SetPasswordHash(nuevoHash);

        return await this.clienteRepository.UpdateClient(cliente);
    }
}
