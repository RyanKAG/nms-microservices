using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationManagement.API.Migrations
{
    public partial class NetworkIntegration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgNetworks_Organizations_OrganizationId",
                table: "OrgNetworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgNetworks",
                table: "OrgNetworks");

            migrationBuilder.RenameTable(
                name: "OrgNetworks",
                newName: "OrganizationNetworks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationNetworks",
                table: "OrganizationNetworks",
                columns: new[] { "OrganizationId", "NetworkId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationNetworks_Organizations_OrganizationId",
                table: "OrganizationNetworks",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationNetworks_Organizations_OrganizationId",
                table: "OrganizationNetworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationNetworks",
                table: "OrganizationNetworks");

            migrationBuilder.RenameTable(
                name: "OrganizationNetworks",
                newName: "OrgNetworks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgNetworks",
                table: "OrgNetworks",
                columns: new[] { "OrganizationId", "NetworkId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrgNetworks_Organizations_OrganizationId",
                table: "OrgNetworks",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
