using Client.Api.Contracts;
using Client.Application.Common;
using Client.Application.DTO;
using Client.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Client.Api.Controllers;

[ApiController]
[Route("clientes")]
public class ClientesController : ControllerBase
{
    private readonly CreateClientUseCase createClientUseCase;
    private readonly GetClientByIdUseCase getClientByIdUseCase;
    private readonly GetAllClientsUseCase getAllClientsUseCase;
    private readonly UpdateClientUseCase updateClientUseCase;
    private readonly DeleteClientUseCase deleteClientUseCase;
    private readonly ActivateClientUseCase activateClientUseCase;
    private readonly DeactivateClientUseCase deactivateClientUseCase;
    private readonly UpdatePasswordUseCase updatePasswordUseCase;

    public ClientesController(
        CreateClientUseCase createClientUseCase,
        GetClientByIdUseCase getClientByIdUseCase,
        GetAllClientsUseCase getAllClientsUseCase,
        UpdateClientUseCase updateClientUseCase,
        DeleteClientUseCase deleteClientUseCase,
        ActivateClientUseCase activateClientUseCase,
        DeactivateClientUseCase deactivateClientUseCase,
        UpdatePasswordUseCase updatePasswordUseCase)
    {
        this.createClientUseCase = createClientUseCase;
        this.getClientByIdUseCase = getClientByIdUseCase;
        this.getAllClientsUseCase = getAllClientsUseCase;
        this.updateClientUseCase = updateClientUseCase;
        this.deleteClientUseCase = deleteClientUseCase;
        this.activateClientUseCase = activateClientUseCase;
        this.deactivateClientUseCase = deactivateClientUseCase;
        this.updatePasswordUseCase = updatePasswordUseCase;
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    /// <param name="dto">Datos del cliente a crear.</param>
    /// <response code="201">Cliente creado exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="409">Ya existe un cliente con la misma identificación o clienteId.</response>
    [HttpPost]
    [ProducesResponseType(typeof(GatewayResponse<ClientDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
    {
        var cliente = await this.createClientUseCase.Execute(dto);
        return this.CreatedAtAction(nameof(this.GetById), new { id = cliente.Id }, GatewayResponse<ClientDto>.Ok(cliente));
    }

    /// <summary>
    /// Obtiene un cliente por su identificador.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <response code="200">Cliente encontrado.</response>
    /// <response code="404">No existe un cliente con el identificador indicado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GatewayResponse<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cliente = await this.getClientByIdUseCase.Execute(id);
        return this.Ok(GatewayResponse<ClientDto>.Ok(cliente));
    }

    /// <summary>
    /// Obtiene la lista de todos los clientes.
    /// </summary>
    /// <response code="200">Listado de clientes obtenido exitosamente.</response>
    [HttpGet]
    [ProducesResponseType(typeof(GatewayResponse<List<ClientDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await this.getAllClientsUseCase.Execute(Unit.Value);
        return this.Ok(GatewayResponse<List<ClientDto>>.Ok(clientes));
    }

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <param name="dto">Nuevos datos del cliente.</param>
    /// <response code="200">Cliente actualizado exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="404">No existe un cliente con el identificador indicado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GatewayResponse<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientDto dto)
    {
        var cliente = await this.updateClientUseCase.Execute(new UpdateClientRequest { Id = id, Dto = dto });
        return this.Ok(GatewayResponse<ClientDto>.Ok(cliente));
    }

    /// <summary>
    /// Elimina (borrado lógico) un cliente existente.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <response code="204">Cliente eliminado exitosamente.</response>
    /// <response code="404">No existe un cliente con el identificador indicado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await this.deleteClientUseCase.Execute(id);
        return this.NoContent();
    }

    /// <summary>
    /// Activa un cliente existente.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <response code="200">Cliente activado exitosamente.</response>
    /// <response code="404">No existe un cliente con el identificador indicado.</response>
    [HttpPatch("{id}/activar")]
    [ProducesResponseType(typeof(GatewayResponse<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var cliente = await this.activateClientUseCase.Execute(id);
        return this.Ok(GatewayResponse<ClientDto>.Ok(cliente));
    }

    /// <summary>
    /// Desactiva un cliente existente.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <response code="200">Cliente desactivado exitosamente.</response>
    /// <response code="404">No existe un cliente con el identificador indicado.</response>
    [HttpPatch("{id}/desactivar")]
    [ProducesResponseType(typeof(GatewayResponse<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var cliente = await this.deactivateClientUseCase.Execute(id);
        return this.Ok(GatewayResponse<ClientDto>.Ok(cliente));
    }

    /// <summary>
    /// Actualiza la contraseña de un cliente existente.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <param name="dto">Contraseña actual y nueva contraseña.</param>
    /// <response code="200">Contraseña actualizada exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="401">La contraseña actual proporcionada es incorrecta.</response>
    /// <response code="404">No existe un cliente con el identificador indicado.</response>
    [HttpPatch("{id}/contrasena")]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdatePasswordDto dto)
    {
        var updated = await this.updatePasswordUseCase.Execute(new UpdatePasswordRequest { ClienteId = id, Dto = dto });
        return this.Ok(GatewayResponse<object>.Ok(new { updated }));
    }
}
