using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pry20220181_data_layer.Migrations
{
    public partial class AddDistrictInUbigeo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Ubigeo",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Ubigeo");
        }
    }
}
