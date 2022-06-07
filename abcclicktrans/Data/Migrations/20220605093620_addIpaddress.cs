using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abcclicktrans.Data.Migrations
{
    public partial class addIpaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPaddress",
                table: "TransportOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPaddress",
                table: "TransportOrders");
        }
    }
}
