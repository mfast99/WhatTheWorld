using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatTheWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllCountries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Capital = table.Column<string>(type: "TEXT", nullable: false),
                    FlagEmoji = table.Column<string>(type: "TEXT", nullable: false),
                    Lat = table.Column<double>(type: "REAL", nullable: false),
                    Lon = table.Column<double>(type: "REAL", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    Subregion = table.Column<string>(type: "TEXT", nullable: false),
                    Population = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    AreaKm2 = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Timezones = table.Column<string>(type: "TEXT", nullable: false),
                    Currencies = table.Column<string>(type: "TEXT", nullable: false),
                    Languages = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Summary = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TempCelsius = table.Column<double>(type: "REAL", nullable: false),
                    TempFahrenheit = table.Column<double>(type: "REAL", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weather_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_CountryId",
                table: "News",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Weather_CountryId",
                table: "Weather",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Weather");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
