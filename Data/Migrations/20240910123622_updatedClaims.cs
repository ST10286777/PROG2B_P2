using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG_P1.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEarnings",
                table: "Claims");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalEarnings",
                table: "Claims",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
