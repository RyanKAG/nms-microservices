using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetworkAPI.Migrations
{
    public partial class DeviceIntegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetworkDevice",
                columns: table => new
                {
                    NetworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkDevice", x => new { x.NetworkId, x.DeviceId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NetworkDevice");
        }
    }
}
