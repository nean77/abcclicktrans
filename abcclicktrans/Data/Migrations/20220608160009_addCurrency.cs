using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace abcclicktrans.Data.Migrations
{
    public partial class addCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportOrders_TransportAddress_DeliveryAddressId",
                table: "TransportOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportOrders_TransportAddress_PickUpAddressId",
                table: "TransportOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "PickUpAddressId",
                table: "TransportOrders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeliveryAddressId",
                table: "TransportOrders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "TransportOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Postal",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportOrders_TransportAddress_DeliveryAddressId",
                table: "TransportOrders",
                column: "DeliveryAddressId",
                principalTable: "TransportAddress",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportOrders_TransportAddress_PickUpAddressId",
                table: "TransportOrders",
                column: "PickUpAddressId",
                principalTable: "TransportAddress",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportOrders_TransportAddress_DeliveryAddressId",
                table: "TransportOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportOrders_TransportAddress_PickUpAddressId",
                table: "TransportOrders");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "TransportOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "PickUpAddressId",
                table: "TransportOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DeliveryAddressId",
                table: "TransportOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postal",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "TransportAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportOrders_TransportAddress_DeliveryAddressId",
                table: "TransportOrders",
                column: "DeliveryAddressId",
                principalTable: "TransportAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportOrders_TransportAddress_PickUpAddressId",
                table: "TransportOrders",
                column: "PickUpAddressId",
                principalTable: "TransportAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
