namespace Account.Domain.Repository;

public interface ICuentaRepository
{
    Task<Guid?> CreateAccount(Cuenta cuenta);

    Task<Cuenta?> GetAccountById(Guid id);

    Task<Cuenta?> GetAccountByNumber(string numeroCuenta);

    Task<bool> ExistsAccountWithNumber(string numeroCuenta);

    Task<List<Cuenta>> GetAccountsByClientId(Guid clienteId);

    Task<List<Cuenta>> GetAllAccounts();

    Task<bool> UpdateAccount(Cuenta cuenta);

    Task<Movimiento?> RegisterMovement(Guid cuentaId, TipoMovimiento tipoMovimiento, decimal valor);

    Task<Movimiento?> GetMovementById(Guid movimientoId);

    Task<List<Movimiento>> GetMovementsByAccountId(
        Guid cuentaId,
        DateTime? desde = null,
        DateTime? hasta = null,
        int? skip = null,
        int? take = null);
}
