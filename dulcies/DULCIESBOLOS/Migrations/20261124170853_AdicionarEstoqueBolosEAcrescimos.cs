using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULCIESBOLOS.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEstoqueBolosEAcrescimos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estoque",
                table: "Bolo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Estoque",
                table: "Acrescimo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estoque",
                table: "Bolo");

            migrationBuilder.DropColumn(
                name: "Estoque",
                table: "Acrescimo");
        }
    }
}
