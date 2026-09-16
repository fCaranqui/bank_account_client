namespace Account.Domain.Repository;

public interface ICuentaRepository
{
    Task<Guid?> CreateCuenta(Cuenta cuenta);

    Task<Cuenta?> GetCuentaById(Guid id);

    Task<Cuenta?> GetCuentaByNumeroCuenta(string numeroCuenta);

    Task<bool> ExisteCuentaConNumeroCuenta(string numeroCuenta);

    Task<List<Cuenta>> GetCuentasByClienteId(string clienteId);

    Task<List<Cuenta>> GetAllCuentas();

    Task<bool> UpdateCuenta(Cuenta cuenta);

    Task<Movimiento?> RegistrarMovimiento(Guid cuentaId, TipoMovimiento tipoMovimiento, decimal valor);

    Task<Movimiento?> GetMovimientoById(Guid movimientoId);

    Task<List<Movimiento>> GetMovimientosByCuentaId(
        Guid cuentaId,
        DateTime? desde = null,
        DateTime? hasta = null,
        int? skip = null,
        int? take = null);
}
