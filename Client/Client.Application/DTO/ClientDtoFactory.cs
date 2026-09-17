using Client.Domain;

namespace Client.Application.DTO;

public static class ClientDtoFactory
{
    public static ClientDto CreateFromEntity(Cliente cliente)
    {
        return new ClientDto
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Genero = cliente.Genero,
            Edad = cliente.Edad,
            Identificacion = cliente.Identificacion,
            Direccion = cliente.Direccion,
            Telefono = cliente.Telefono,
            Estado = cliente.Estado,
            CreatedAt = cliente.CreatedAt,
            UpdatedAt = cliente.UpdatedAt,
        };
    }
}
