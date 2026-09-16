using Account.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Db;

public class MovimientoConfiguration : IEntityTypeConfiguration<Movimiento>
{
    public void Configure(EntityTypeBuilder<Movimiento> builder)
    {
        builder.ToTable("Movimientos");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(m => m.Fecha)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(m => m.TipoMovimiento)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.Valor).HasPrecision(18, 2);

        builder.Property(m => m.Saldo).HasPrecision(18, 2);

        builder.Property(m => m.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(m => m.UpdatedAt);

        builder.Property(m => m.DeletedAt);

        builder
            .HasOne<Cuenta>()
            .WithMany(c => c.Movimientos)
            .HasForeignKey(m => m.CuentaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
