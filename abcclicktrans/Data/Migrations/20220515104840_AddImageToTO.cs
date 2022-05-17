using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abcclicktrans.Data.Migrations
{
    public partial class AddImageToTO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportOrders_AspNetUsers_ApplicationUserId",
                table: "TransportOrders");

            migrationBuilder.DropIndex(
                name: "IX_TransportOrders_ApplicationUserId",
                table: "TransportOrders");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "TransportOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "TransportOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "TransportOrders");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "TransportOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
    }
}
