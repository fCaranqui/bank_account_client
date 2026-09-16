using Account.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Db;

public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
{
    public void Configure(EntityTypeBuilder<Cuenta> builder)
    {
        builder.ToTable("Cuentas");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.NumeroCuenta)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(c => c.NumeroCuenta).IsUnique();

        builder.Property(c => c.TipoCuenta)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.SaldoInicial).HasPrecision(18, 2);

        builder.Property(c => c.SaldoDisponible).HasPrecision(18, 2);

        builder.Property(c => c.ClienteId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.DeletedAt);

        builder.Navigation(c => c.Movimientos)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
