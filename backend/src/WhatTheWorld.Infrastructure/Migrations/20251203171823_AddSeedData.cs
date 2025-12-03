using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhatTheWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Code", "Capital", "FlagEmoji", "Lat", "Lon", "Name" },
                values: new object[,]
                {
                    { "DE", "Berlin", "🇩🇪", 52.520000000000003, 13.404999999999999, "Germany" },
                    { "FR", "Paris", "🇫🇷", 48.8566, 2.3521999999999998, "France" },
                    { "GB", "London", "🇬🇧", 51.507399999999997, -0.1278, "United Kingdom" },
                    { "US", "Washington D.C.", "🇺🇸", 38.895099999999999, -77.0364, "United States" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Code",
                keyValue: "DE");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Code",
                keyValue: "FR");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Code",
                keyValue: "GB");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Code",
                keyValue: "US");
        }
    }
}
