using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace task_manager.Migrations
{
    public partial class AddPropriedadeDataFinalizacaoCancelamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCancelamento",
                table: "Tarefa",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFinalizacao",
                table: "Tarefa",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCancelamento",
                table: "Tarefa");

            migrationBuilder.DropColumn(
                name: "DataFinalizacao",
                table: "Tarefa");
        }
    }
}
