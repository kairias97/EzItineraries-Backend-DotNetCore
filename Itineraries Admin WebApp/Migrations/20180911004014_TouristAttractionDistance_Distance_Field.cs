using Microsoft.EntityFrameworkCore.Migrations;

namespace ItinerariesAdminWebApp.Migrations
{
    public partial class TouristAttractionDistance_Distance_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "TouristAttractionDistances",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "TouristAttractionDistances");
        }
    }
}
