using Microsoft.EntityFrameworkCore.Migrations;

namespace DeviceManagement.API.Migrations
{
    public partial class PcTableNewOsCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OS",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OS",
                table: "Devices");
        }
    }
}
