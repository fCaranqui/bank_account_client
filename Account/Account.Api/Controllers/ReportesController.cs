using Account.Api.Contracts;
using Account.Application.DTO;
using Account.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

[ApiController]
[Route("reportes")]
public class ReportesController : ControllerBase
{
    private readonly GenerateAccountStatementReportUseCase generateAccountStatementReportUseCase;

    public ReportesController(GenerateAccountStatementReportUseCase generateAccountStatementReportUseCase)
    {
        this.generateAccountStatementReportUseCase = generateAccountStatementReportUseCase;
    }

    /// <summary>
    /// Genera el estado de cuenta de un cliente para un rango de fechas.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="desde">Fecha inicial del rango.</param>
    /// <param name="hasta">Fecha final del rango.</param>
    /// <response code="200">Reporte generado correctamente.</response>
    /// <response code="400">Rango de fechas inválido.</response>
    [HttpGet]
    [ProducesResponseType(typeof(GatewayResponse<AccountStatementReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GatewayResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStatement([FromQuery] Guid clienteId, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        var reporte = await this.generateAccountStatementReportUseCase.Execute(new GenerateAccountStatementReportRequest
        {
            ClienteId = clienteId,
            Desde = desde,
            Hasta = hasta,
        });

        return this.Ok(GatewayResponse<AccountStatementReportDto>.Ok(reporte));
    }
}
