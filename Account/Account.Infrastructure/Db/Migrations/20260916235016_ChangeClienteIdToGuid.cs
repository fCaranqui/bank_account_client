using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Infrastructure.Db.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClienteIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Postgres has no implicit/assignment cast from character varying to uuid, so the
            // plain AlterColumn EF scaffolds fails with "column cannot be cast automatically" —
            // an explicit USING clause is required even though this column has no invalid data.
            migrationBuilder.Sql(
                "ALTER TABLE \"Cuentas\" ALTER COLUMN \"ClienteId\" TYPE uuid USING \"ClienteId\"::uuid;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Cuentas\" ALTER COLUMN \"ClienteId\" TYPE character varying(50) USING \"ClienteId\"::text;");
        }
    }
}
