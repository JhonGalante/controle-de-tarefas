using Microsoft.EntityFrameworkCore.Migrations;

namespace task_manager.Migrations
{
    public partial class Correcao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "prazo",
                table: "Tarefa",
                newName: "Prazo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prazo",
                table: "Tarefa",
                newName: "prazo");
        }
    }
}
