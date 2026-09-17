using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Db;

public class ClientReplicaConfiguration : IEntityTypeConfiguration<ClientReplica>
{
    public void Configure(EntityTypeBuilder<ClientReplica> builder)
    {
        builder.ToTable("ClientReplicas");

        builder.HasKey(x => x.ClientId);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamptz")
            .IsRequired();
    }
}
