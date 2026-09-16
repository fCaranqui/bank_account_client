using Client.Application.Common;
using Client.Application.Exceptions;
using Client.Domain.Repository;

namespace Client.Application.UseCases;

public class DeleteClientUseCase : IUseCase<Guid, bool>
{
    private readonly IClienteRepository clienteRepository;

    public DeleteClientUseCase(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<bool> Execute(Guid input)
    {
        var cliente = await this.clienteRepository.GetClientById(input);
        if (cliente == null)
        {
            throw new ClientNotFoundException();
        }

        return await this.clienteRepository.DeleteClient(input);
    }
}
