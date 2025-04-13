using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTodoModelsAndEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TodoItems",
                newName: "ItemName");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "TodoList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TodoList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Days",
                table: "TodoList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TodoList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TodoItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "TodoItems",
                newName: "Name");
        }
    }
}
