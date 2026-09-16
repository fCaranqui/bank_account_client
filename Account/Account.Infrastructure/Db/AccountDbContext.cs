using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Db;

public class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
    }
}
