using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Infrastructure.Db.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClienteId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clientes_ClienteId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Clientes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClienteId",
                table: "Clientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ClienteId",
                table: "Clientes",
                column: "ClienteId",
                unique: true);
        }
    }
}
