using Microsoft.EntityFrameworkCore.Migrations;

namespace ItinerariesAdminWebApp.Migrations
{
    public partial class TouristAttractionConnection_rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TouristAttractionDistances");

            migrationBuilder.CreateTable(
                name: "TouristAttractionConnection",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    Distance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristAttractionConnection", x => new { x.CityId, x.OriginId, x.DestinationId });
                    table.ForeignKey(
                        name: "FK_TouristAttractionConnection_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TouristAttractionConnection_TouristAttractions_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "TouristAttractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TouristAttractionConnection_TouristAttractions_OriginId",
                        column: x => x.OriginId,
                        principalTable: "TouristAttractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractionConnection_DestinationId",
                table: "TouristAttractionConnection",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractionConnection_OriginId",
                table: "TouristAttractionConnection",
                column: "OriginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TouristAttractionConnection");

            migrationBuilder.CreateTable(
                name: "TouristAttractionDistances",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    Distance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristAttractionDistances", x => new { x.CityId, x.OriginId, x.DestinationId });
                    table.ForeignKey(
                        name: "FK_TouristAttractionDistances_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TouristAttractionDistances_TouristAttractions_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "TouristAttractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TouristAttractionDistances_TouristAttractions_OriginId",
                        column: x => x.OriginId,
                        principalTable: "TouristAttractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractionDistances_DestinationId",
                table: "TouristAttractionDistances",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractionDistances_OriginId",
                table: "TouristAttractionDistances",
                column: "OriginId");
        }
    }
}
