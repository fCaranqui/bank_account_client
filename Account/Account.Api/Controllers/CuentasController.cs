using Account.Api.Contracts;
using Account.Application.Common;
using Account.Application.DTO;
using Account.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

[ApiController]
[Route("cuentas")]
public class CuentasController : ControllerBase
{
    private readonly CreateAccountUseCase createAccountUseCase;
    private readonly GetAccountByIdUseCase getAccountByIdUseCase;
    private readonly GetAllAccountsUseCase getAllAccountsUseCase;
    private readonly GetAccountsByClientIdUseCase getAccountsByClientIdUseCase;
    private readonly UpdateAccountUseCase updateAccountUseCase;

    public CuentasController(
        CreateAccountUseCase createAccountUseCase,
        GetAccountByIdUseCase getAccountByIdUseCase,
        GetAllAccountsUseCase getAllAccountsUseCase,
        GetAccountsByClientIdUseCase getAccountsByClientIdUseCase,
        UpdateAccountUseCase updateAccountUseCase)
    {
        this.createAccountUseCase = createAccountUseCase;
        this.getAccountByIdUseCase = getAccountByIdUseCase;
        this.getAllAccountsUseCase = getAllAccountsUseCase;
        this.getAccountsByClientIdUseCase = getAccountsByClientIdUseCase;
        this.updateAccountUseCase = updateAccountUseCase;
    }

    /// <summary>
    /// Crea una nueva cuenta.
    /// </summary>
    /// <param name="dto">Datos de la cuenta a crear.</param>
    /// <response code="201">Cuenta creada correctamente.</response>
    /// <response code="400">Datos de la cuenta inválidos.</response>
    /// <response code="409">Ya existe una cuenta con el mismo número de cuenta.</response>
    [HttpPost]
    [ProducesResponseType(typeof(GatewayResponse<AccountDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
    {
        var account = await this.createAccountUseCase.Execute(dto);
        return this.CreatedAtAction(nameof(this.GetById), new { id = account.Id }, GatewayResponse<AccountDto>.Ok(account));
    }

    /// <summary>
    /// Obtiene una cuenta por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <response code="200">Cuenta encontrada.</response>
    /// <response code="404">No existe una cuenta con el identificador indicado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GatewayResponse<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await this.getAccountByIdUseCase.Execute(id);
        return this.Ok(GatewayResponse<AccountDto>.Ok(account));
    }

    /// <summary>
    /// Obtiene todas las cuentas registradas.
    /// </summary>
    /// <response code="200">Listado de cuentas obtenido correctamente.</response>
    [HttpGet]
    [ProducesResponseType(typeof(GatewayResponse<List<AccountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await this.getAllAccountsUseCase.Execute(Unit.Value);
        return this.Ok(GatewayResponse<List<AccountDto>>.Ok(accounts));
    }

    /// <summary>
    /// Obtiene todas las cuentas asociadas a un cliente.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <response code="200">Listado de cuentas del cliente obtenido correctamente.</response>
    [HttpGet("cliente/{clienteId}")]
    [ProducesResponseType(typeof(GatewayResponse<List<AccountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClientId(Guid clienteId)
    {
        var accounts = await this.getAccountsByClientIdUseCase.Execute(clienteId);
        return this.Ok(GatewayResponse<List<AccountDto>>.Ok(accounts));
    }

    /// <summary>
    /// Actualiza el estado de una cuenta existente.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="dto">Datos de actualización de la cuenta.</param>
    /// <response code="200">Cuenta actualizada correctamente.</response>
    /// <response code="400">Datos de actualización inválidos.</response>
    /// <response code="404">No existe una cuenta con el identificador indicado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GatewayResponse<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountDto dto)
    {
        var account = await this.updateAccountUseCase.Execute(new UpdateAccountRequest { Id = id, Dto = dto });
        return this.Ok(GatewayResponse<AccountDto>.Ok(account));
    }
}
