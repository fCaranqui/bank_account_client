using Client.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Client.Infrastructure.Db;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.Nombre)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Genero)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Identificacion)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Direccion)
            .HasMaxLength(300);

        builder.Property(c => c.Telefono)
            .HasMaxLength(20);

        builder.Property(c => c.ContrasenaHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.DeletedAt);
    }
}
