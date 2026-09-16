using Account.Domain;

namespace Account.Application.DTO;

public static class AccountDtoFactory
{
    public static AccountDto CreateFromEntity(Cuenta cuenta)
    {
        return new AccountDto
        {
            Id = cuenta.Id,
            NumeroCuenta = cuenta.NumeroCuenta,
            TipoCuenta = cuenta.TipoCuenta,
            SaldoInicial = cuenta.SaldoInicial,
            SaldoDisponible = cuenta.SaldoDisponible,
            Estado = cuenta.Estado,
            ClienteId = cuenta.ClienteId,
            CreatedAt = cuenta.CreatedAt,
            UpdatedAt = cuenta.UpdatedAt,
        };
    }
}
