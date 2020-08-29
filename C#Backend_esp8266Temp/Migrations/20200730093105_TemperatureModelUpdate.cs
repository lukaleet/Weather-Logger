using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace esp8266Temp.Migrations
{
    public partial class TemperatureModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MeasuredAt",
                table: "Temperatures",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasuredAt",
                table: "Temperatures");
        }
    }
}
