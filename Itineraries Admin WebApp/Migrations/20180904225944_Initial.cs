using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItinerariesAdminWebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(200)", nullable: true),
                    Password = table.Column<string>(type: "varchar(250)", nullable: true),
                    Name = table.Column<string>(type: "varchar(120)", nullable: true),
                    Lastname = table.Column<string>(type: "varchar(120)", nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    IsoNumericCode = table.Column<string>(type: "varchar(3)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Alpha2Code = table.Column<string>(type: "varchar(2)", nullable: true),
                    Alpha3Code = table.Column<string>(type: "varchar(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.IsoNumericCode);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true),
                    Token = table.Column<string>(type: "varchar(250)", nullable: true),
                    SentBy = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Administrators_SentBy",
                        column: x => x.SentBy,
                        principalTable: "Administrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", nullable: true),
                    CountryId = table.Column<string>(type: "varchar(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "IsoNumericCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TouristAttractions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "250", nullable: true),
                    Address = table.Column<string>(type: "400", nullable: true),
                    GooglePlaceId = table.Column<string>(type: "100", nullable: true),
                    PhoneNumber = table.Column<string>(type: "60", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "150", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Latitude = table.Column<decimal>(type: "DECIMAL(8,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristAttractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouristAttractions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TouristAttractions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TouristAttractions_Administrators_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Administrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TouristAttractionSuggestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true),
                    Address = table.Column<string>(type: "varchar(400)", nullable: true),
                    GooglePlaceId = table.Column<string>(type: "varchar(100)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(60)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "varchar(150)", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Approved = table.Column<bool>(nullable: true),
                    AnsweredBy = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    Latitude = table.Column<decimal>(type: "DECIMAL(8,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristAttractionSuggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouristAttractionSuggestions_Administrators_AnsweredBy",
                        column: x => x.AnsweredBy,
                        principalTable: "Administrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TouristAttractionSuggestions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TouristAttractionSuggestions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_Email",
                table: "Administrators",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SentBy",
                table: "Invitations",
                column: "SentBy");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractions_CategoryId",
                table: "TouristAttractions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractions_CityId",
                table: "TouristAttractions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractions_CreatedBy",
                table: "TouristAttractions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractions_GooglePlaceId",
                table: "TouristAttractions",
                column: "GooglePlaceId",
                unique: true,
                filter: "[GooglePlaceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractionSuggestions_AnsweredBy",
                table: "TouristAttractionSuggestions",
                column: "AnsweredBy");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractionSuggestions_CategoryId",
                table: "TouristAttractionSuggestions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttractionSuggestions_CityId",
                table: "TouristAttractionSuggestions",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "TouristAttractions");

            migrationBuilder.DropTable(
                name: "TouristAttractionSuggestions");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
