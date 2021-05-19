using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orders.Migrations
{
    public partial class UpdateOrderproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "OrderProduct",
                newName: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Status",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrderProduct",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderProduct");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderProduct",
                newName: "Amount");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Status",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "StatusId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct",
                column: "ProductId");
        }
    }
}
