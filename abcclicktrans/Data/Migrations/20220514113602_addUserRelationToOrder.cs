using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abcclicktrans.Data.Migrations
{
    public partial class addUserRelationToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TransportOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TransportOrders_ApplicationUserId",
                table: "TransportOrders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportOrders_AspNetUsers_ApplicationUserId",
                table: "TransportOrders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportOrders_AspNetUsers_ApplicationUserId",
                table: "TransportOrders");

            migrationBuilder.DropIndex(
                name: "IX_TransportOrders_ApplicationUserId",
                table: "TransportOrders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TransportOrders");
        }
    }
}
