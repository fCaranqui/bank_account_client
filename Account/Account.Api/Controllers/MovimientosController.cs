using Account.Api.Contracts;
using Account.Application.DTO;
using Account.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

[ApiController]
[Route("movimientos")]
public class MovimientosController : ControllerBase
{
    private readonly RegisterMovementUseCase registerMovementUseCase;
    private readonly GetMovementByIdUseCase getMovementByIdUseCase;
    private readonly GetMovementsByAccountIdUseCase getMovementsByAccountIdUseCase;

    public MovimientosController(
        RegisterMovementUseCase registerMovementUseCase,
        GetMovementByIdUseCase getMovementByIdUseCase,
        GetMovementsByAccountIdUseCase getMovementsByAccountIdUseCase)
    {
        this.registerMovementUseCase = registerMovementUseCase;
        this.getMovementByIdUseCase = getMovementByIdUseCase;
        this.getMovementsByAccountIdUseCase = getMovementsByAccountIdUseCase;
    }

    /// <summary>
    /// Registra un nuevo movimiento (depósito o retiro) sobre una cuenta existente.
    /// </summary>
    /// <param name="dto">Datos del movimiento a registrar.</param>
    /// <response code="201">Movimiento registrado correctamente.</response>
    /// <response code="400">Datos del movimiento inválidos.</response>
    /// <response code="404">No existe una cuenta con el identificador indicado.</response>
    /// <response code="409">
    /// La cuenta se encuentra inactiva o no dispone de saldo suficiente para el retiro.
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(GatewayResponse<MovementDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterMovementDto dto)
    {
        var movement = await this.registerMovementUseCase.Execute(dto);
        return this.CreatedAtAction(nameof(this.GetById), new { id = movement.Id }, GatewayResponse<MovementDto>.Ok(movement));
    }

    /// <summary>
    /// Obtiene un movimiento por su identificador.
    /// </summary>
    /// <param name="id">Identificador del movimiento.</param>
    /// <response code="200">Movimiento encontrado.</response>
    /// <response code="404">No existe un movimiento con el identificador indicado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GatewayResponse<MovementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var movement = await this.getMovementByIdUseCase.Execute(id);
        return this.Ok(GatewayResponse<MovementDto>.Ok(movement));
    }

    /// <summary>
    /// Obtiene los movimientos de una cuenta, con filtro opcional por fecha y paginación.
    /// </summary>
    /// <param name="cuentaId">Identificador de la cuenta.</param>
    /// <param name="desde">Fecha inicial del rango a consultar (opcional).</param>
    /// <param name="hasta">Fecha final del rango a consultar (opcional).</param>
    /// <param name="skip">Cantidad de movimientos a omitir, para paginación.</param>
    /// <param name="take">
    /// Cantidad de movimientos a devolver. Se ajusta siempre a un valor entre 1 y 100.
    /// </param>
    /// <response code="200">Listado de movimientos de la cuenta obtenido correctamente.</response>
    [HttpGet("cuenta/{cuentaId}")]
    [ProducesResponseType(typeof(GatewayResponse<List<MovementDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAccountId(
        Guid cuentaId,
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
    {
        take = Math.Clamp(take, 1, 100);

        var movements = await this.getMovementsByAccountIdUseCase.Execute(new GetMovementsByAccountIdRequest
        {
            CuentaId = cuentaId,
            Desde = desde,
            Hasta = hasta,
            Skip = skip,
            Take = take,
        });

        return this.Ok(GatewayResponse<List<MovementDto>>.Ok(movements));
    }
}
