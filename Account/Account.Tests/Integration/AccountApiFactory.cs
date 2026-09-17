using Account.Infrastructure.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Tests.Integration;

public class AccountApiFactory : WebApplicationFactory<Program>
{
    private readonly string databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // AddDbContext registers both a DbContextOptions<T> singleton and an
            // IDbContextOptionsConfiguration<T> action for the original UseNpgsql call. Removing
            // only the former leaves the Npgsql configuration action in place, so it still runs
            // alongside the InMemory one added below and EF Core rejects having two providers.
            var descriptorsToRemove = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<AccountDbContext>)
                    || (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextOptionsConfiguration<>)))
                .ToList();
            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AccountDbContext>(options => options.UseInMemoryDatabase(this.databaseName));
        });
    }
}
