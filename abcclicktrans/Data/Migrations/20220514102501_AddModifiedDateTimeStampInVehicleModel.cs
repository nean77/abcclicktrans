using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abcclicktrans.Data.Migrations
{
    public partial class AddModifiedDateTimeStampInVehicleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "Vehicles",
                type: "datetime2",
                nullable: true);
            }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Vehicles");
        }
    }
}
