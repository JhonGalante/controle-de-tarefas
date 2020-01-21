$(document).ready(function () {
    function enviarAtividade() {
        var enviarAtividadeLink = '/Home/EnviarAtividade';
        var descricao = $("#campo-atividade").val();
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var data = {
            IdTarefa: idTarefa,
            Descricao: descricao
        };
        $("#partial-lista-tarefas").load(enviarAtividadeLink, data);
    }

    function deletarAtividade(atividadeSelecionada) {
        debugger;
        var deletarAtividadeLink = '/Home/DeletarAtividade';
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var idAtividade = atividadeSelecionada.attributes[0].textContent;
        var data = {
            IdTarefa: idTarefa,
            IdAtividade: idAtividade
        };
        $("#partial-lista-tarefas").load(deletarAtividadeLink, data);
    }

    $("#btn-enviar-atividade").on("click", function () {
        enviarAtividade();
        $("#campo-atividade").val("");
    });

    $(".apagar-atividade").on("click", function () {
        deletarAtividade(this.parentElement);
    });
});