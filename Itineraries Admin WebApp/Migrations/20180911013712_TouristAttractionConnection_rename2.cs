using Microsoft.EntityFrameworkCore.Migrations;

namespace ItinerariesAdminWebApp.Migrations
{
    public partial class TouristAttractionConnection_rename2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TouristAttractionConnection_Cities_CityId",
                table: "TouristAttractionConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_TouristAttractionConnection_TouristAttractions_DestinationId",
                table: "TouristAttractionConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_TouristAttractionConnection_TouristAttractions_OriginId",
                table: "TouristAttractionConnection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TouristAttractionConnection",
                table: "TouristAttractionConnection");

            migrationBuilder.RenameTable(
                name: "TouristAttractionConnection",
                newName: "TouristAttractionConnections");

            migrationBuilder.RenameIndex(
                name: "IX_TouristAttractionConnection_OriginId",
                table: "TouristAttractionConnections",
                newName: "IX_TouristAttractionConnections_OriginId");

            migrationBuilder.RenameIndex(
                name: "IX_TouristAttractionConnection_DestinationId",
                table: "TouristAttractionConnections",
                newName: "IX_TouristAttractionConnections_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TouristAttractionConnections",
                table: "TouristAttractionConnections",
                columns: new[] { "CityId", "OriginId", "DestinationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TouristAttractionConnections_Cities_CityId",
                table: "TouristAttractionConnections",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TouristAttractionConnections_TouristAttractions_DestinationId",
                table: "TouristAttractionConnections",
                column: "DestinationId",
                principalTable: "TouristAttractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TouristAttractionConnections_TouristAttractions_OriginId",
                table: "TouristAttractionConnections",
                column: "OriginId",
                principalTable: "TouristAttractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TouristAttractionConnections_Cities_CityId",
                table: "TouristAttractionConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_TouristAttractionConnections_TouristAttractions_DestinationId",
                table: "TouristAttractionConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_TouristAttractionConnections_TouristAttractions_OriginId",
                table: "TouristAttractionConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TouristAttractionConnections",
                table: "TouristAttractionConnections");

            migrationBuilder.RenameTable(
                name: "TouristAttractionConnections",
                newName: "TouristAttractionConnection");

            migrationBuilder.RenameIndex(
                name: "IX_TouristAttractionConnections_OriginId",
                table: "TouristAttractionConnection",
                newName: "IX_TouristAttractionConnection_OriginId");

            migrationBuilder.RenameIndex(
                name: "IX_TouristAttractionConnections_DestinationId",
                table: "TouristAttractionConnection",
                newName: "IX_TouristAttractionConnection_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TouristAttractionConnection",
                table: "TouristAttractionConnection",
                columns: new[] { "CityId", "OriginId", "DestinationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TouristAttractionConnection_Cities_CityId",
                table: "TouristAttractionConnection",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TouristAttractionConnection_TouristAttractions_DestinationId",
                table: "TouristAttractionConnection",
                column: "DestinationId",
                principalTable: "TouristAttractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TouristAttractionConnection_TouristAttractions_OriginId",
                table: "TouristAttractionConnection",
                column: "OriginId",
                principalTable: "TouristAttractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
