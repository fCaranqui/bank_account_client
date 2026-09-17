using Account.Application.Common;
using Account.Application.DTO;
using Account.Domain.Repository;

namespace Account.Application.UseCases;

public class GenerateAccountStatementReportUseCase : IUseCase<GenerateAccountStatementReportRequest, AccountStatementReportDto>
{
    private readonly ICuentaRepository cuentaRepository;

    public GenerateAccountStatementReportUseCase(ICuentaRepository cuentaRepository)
    {
        this.cuentaRepository = cuentaRepository;
    }

    public async Task<AccountStatementReportDto> Execute(GenerateAccountStatementReportRequest input)
    {
        if (Guid.Empty.Equals(input.ClienteId))
        {
            throw new ArgumentException("El identificador del cliente no puede estar vacío.");
        }

        input.Desde = DateTime.SpecifyKind(input.Desde, DateTimeKind.Utc);
        input.Hasta = DateTime.SpecifyKind(input.Hasta, DateTimeKind.Utc);

        if (input.Desde.Equals(default(DateTime)) || input.Hasta.Equals(default(DateTime)))
        {
            throw new ArgumentException("Las fechas 'desde' y 'hasta' no pueden ser nulas.");
        }

        if (input.Desde > input.Hasta)
        {
            throw new ArgumentException("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
        }

        var cuentas = await this.cuentaRepository.GetAccountsByClientId(input.ClienteId);

        var estadosCuenta = new List<AccountStatementDto>();
        foreach (var cuenta in cuentas)
        {
            var movimientos = await this.cuentaRepository.GetMovementsByAccountId(
                cuenta.Id,
                input.Desde,
                input.Hasta,
                skip: null,
                take: null);

            estadosCuenta.Add(new AccountStatementDto
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                TipoCuenta = cuenta.TipoCuenta,
                SaldoDisponible = cuenta.SaldoDisponible,
                Movimientos = movimientos.Select(MovementDtoFactory.CreateFromEntity).ToList(),
            });
        }

        return new AccountStatementReportDto
        {
            ClienteId = input.ClienteId,
            Desde = input.Desde,
            Hasta = input.Hasta,
            Cuentas = estadosCuenta,
        };
    }
}
