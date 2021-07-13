using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetworkAPI.Migrations
{
    public partial class AddedNetworkForiegnKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetworkNetworkDevice",
                columns: table => new
                {
                    NetworksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NetworksNetworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NetworksDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkNetworkDevice", x => new { x.NetworksId, x.NetworksNetworkId, x.NetworksDeviceId });
                    table.ForeignKey(
                        name: "FK_NetworkNetworkDevice_NetworkDevice_NetworksNetworkId_NetworksDeviceId",
                        columns: x => new { x.NetworksNetworkId, x.NetworksDeviceId },
                        principalTable: "NetworkDevice",
                        principalColumns: new[] { "NetworkId", "DeviceId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetworkNetworkDevice_Networks_NetworksId",
                        column: x => x.NetworksId,
                        principalTable: "Networks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NetworkNetworkDevice_NetworksNetworkId_NetworksDeviceId",
                table: "NetworkNetworkDevice",
                columns: new[] { "NetworksNetworkId", "NetworksDeviceId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NetworkNetworkDevice");
        }
    }
}
