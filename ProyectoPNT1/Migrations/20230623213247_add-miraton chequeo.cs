using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoPNT1.Migrations
{
    /// <inheritdoc />
    public partial class addmiratonchequeo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Roles");
        }
    }
}
