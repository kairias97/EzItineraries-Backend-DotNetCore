using Microsoft.EntityFrameworkCore.Migrations;

namespace ItinerariesAdminWebApp.Migrations
{
    public partial class Latitude_Longitude_Decimal_To_Double : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "TouristAttractionSuggestions",
                type: "float(53)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(9,6)");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "TouristAttractionSuggestions",
                type: "float(53)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(8,6)");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "TouristAttractions",
                type: "float(53)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(9,6)");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "TouristAttractions",
                type: "float(53)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(8,6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "TouristAttractionSuggestions",
                type: "DECIMAL(9,6)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(53)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "TouristAttractionSuggestions",
                type: "DECIMAL(8,6)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(53)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "TouristAttractions",
                type: "DECIMAL(9,6)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(53)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "TouristAttractions",
                type: "DECIMAL(8,6)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(53)");
        }
    }
}
