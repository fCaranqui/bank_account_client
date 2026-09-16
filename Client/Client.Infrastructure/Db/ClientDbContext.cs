using Microsoft.EntityFrameworkCore;

namespace Client.Infrastructure.Db;

public class ClientDbContext : DbContext
{
    public ClientDbContext(DbContextOptions<ClientDbContext> options)
        : base(options)
    {
    }
}
