using Microsoft.EntityFrameworkCore.Migrations;

namespace ItinerariesAdminWebApp.Migrations
{
    public partial class AttractionWebsiteUrl_Alter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WebsiteUrl",
                table: "TouristAttractions",
                type: "varchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WebsiteUrl",
                table: "TouristAttractions",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldNullable: true);
        }
    }
}
