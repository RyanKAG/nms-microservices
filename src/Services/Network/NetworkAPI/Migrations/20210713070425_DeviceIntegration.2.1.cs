using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetworkAPI.Migrations
{
    public partial class DeviceIntegration21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Networks_NetworkId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_NetworkId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "NetworkId",
                table: "Devices");

            migrationBuilder.CreateTable(
                name: "DeviceNetwork",
                columns: table => new
                {
                    ConnectedDevicesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NetworksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceNetwork", x => new { x.ConnectedDevicesId, x.NetworksId });
                    table.ForeignKey(
                        name: "FK_DeviceNetwork_Devices_ConnectedDevicesId",
                        column: x => x.ConnectedDevicesId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceNetwork_Networks_NetworksId",
                        column: x => x.NetworksId,
                        principalTable: "Networks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceNetwork_NetworksId",
                table: "DeviceNetwork",
                column: "NetworksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceNetwork");

            migrationBuilder.AddColumn<Guid>(
                name: "NetworkId",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_NetworkId",
                table: "Devices",
                column: "NetworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Networks_NetworkId",
                table: "Devices",
                column: "NetworkId",
                principalTable: "Networks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
