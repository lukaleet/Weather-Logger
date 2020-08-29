using Microsoft.EntityFrameworkCore.Migrations;

namespace esp8266Temp.Migrations
{
    public partial class TemperatureModelUpdateVol2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Temperatures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Temperatures");
        }
    }
}
