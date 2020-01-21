using Microsoft.EntityFrameworkCore.Migrations;

namespace task_manager.Migrations
{
    public partial class CorrecaoRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atividade_Tarefa_TarefaId",
                table: "Atividade");

            migrationBuilder.AlterColumn<int>(
                name: "TarefaId",
                table: "Atividade",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Atividade_Tarefa_TarefaId",
                table: "Atividade",
                column: "TarefaId",
                principalTable: "Tarefa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atividade_Tarefa_TarefaId",
                table: "Atividade");

            migrationBuilder.AlterColumn<int>(
                name: "TarefaId",
                table: "Atividade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Atividade_Tarefa_TarefaId",
                table: "Atividade",
                column: "TarefaId",
                principalTable: "Tarefa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
