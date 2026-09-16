using Account.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account.Tests.Application;

public static class TestDbContextFactory
{
    public static PgCuentaRepository CreateRepository()
    {
        var options = new DbContextOptionsBuilder<AccountDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AccountDbContext(options);
        return new PgCuentaRepository(dbContext);
    }
}
