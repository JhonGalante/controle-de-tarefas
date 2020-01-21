$(document).ready(function () {

    var table = initTable();

    function initTable() {
        debugger;
        return $('#tabtarefas').DataTable({
            "language": {
                "lengthMenu": "_MENU_",
                "zeroRecords": "Nenhuma tarefa encontrada!",
                "info": "",
                "search": "Buscar:",
                "infoEmpty": "",
                "infoFiltered": "(filtrado a partir de _MAX_ tarefas)",
                "oPaginate": {
                    "sFirst": "Primeiro",
                    "sPrevious": "Anterior",
                    "sNext": "Seguinte",
                    "sLast": "Último"
                }
            },
            "retrieve": true
        });
    }

    function limparCampos() {
        $("#campo-titulo").val("");
        $("#campo-descricao").val("");
        $("#campo-responsavel").val("0");
        $("#campo-urgencia").val("0");
        $("#campo-prazo").val("");
        $(".selecionado").removeClass("selecionado");
    }

    function enviarTarefa() {
        var salvarTarefaLink = '/Home/SalvarTarefa';
        var titulo = $("#campo-titulo").val();
        var descricao = $("#campo-descricao").val();
        var responsavel = $("#campo-responsavel").val();
        var urgencia = $("#campo-urgencia").val();
        var prazo = $("#campo-prazo").val() + "T00:00:00";
        console.log(prazo);
        var data = {
            Titulo: titulo,
            Descricao: descricao,
            ResponsavelId: responsavel,
            NivelUrgencia: urgencia,
            Prazo: prazo
        };
        $("#tabela-tarefas-view").load(salvarTarefaLink, data);
    }

    function atualizarTarefa() {
        var atualizarTarefaLink = '/Home/AtualizarTarefa';
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var titulo = $("#detalhe-titulo").val();
        var descricao = $("#detalhe-descricao").val();
        var responsavel = $("#detalhe-responsavel").val();
        var urgencia = $("#detalhe-urgencia").val();
        var prazo = $("#detalhe-prazo").val() + "T00:00:00";
        debugger;
        var data = {
            Id: idTarefa,
            Titulo: titulo,
            Descricao: descricao,
            ResponsavelId: responsavel,
            NivelUrgencia: urgencia,
            Prazo: prazo
        };
        $("#tabela-tarefas-view").load(atualizarTarefaLink, data);
    }

    function carregarAtividades() {
        var carregarAtividadesLink = '/Home/CarregarAtividades';
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var data = {
            IdTarefa: idTarefa
        };
        $("#partial-lista-tarefas").load(carregarAtividadesLink, data);
    }

    function finalizarTarefa() {
        var finalizarTarefaLink = '/Home/FinalizarTarefa';
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var data = {
            Id: idTarefa,
        };
        $("#tabela-tarefas-view").load(finalizarTarefaLink, data);
    }

    function finalizarTarefa2() {
        var finalizarTarefaLink = '/Home/FinalizarTarefa';
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var data = {
            Id: idTarefa,
        };
        $("#tabela-tarefas-usuario-view").load(finalizarTarefaLink, data);
    }

    function cancelarTarefa() {
        var cancelarTarefaLink = '/Home/CancelarTarefa';
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var data = {
            Id: idTarefa,
        };
        $("#tabela-tarefas-view").load(cancelarTarefaLink, data);
    }

    function deletarTarefa() {
        var deletarTarefaLink = '/Home/DeletarTarefa';
        var idTarefa = $(".selecionado")[0].attributes[0].textContent;
        var data = {
            Id: idTarefa,
        };
        $("#tabela-tarefas-view").load(deletarTarefaLink, data);
    }

    $('#btn-salvar').on('click', function () {
        enviarTarefa();
        limparCampos();
    });
    $('#btn-atualizar').on('click', function () {
        atualizarTarefa();
        limparCampos();
    });
    $('#btn-fechar').on('click', function () {
        limparCampos();
    });
    $('#btn-fechar-detalhe').on('click', function () {
        limparCampos();
    });

    $('#close').on('click', function () {
        limparCampos();
    });

    $('#close-detalhe').on('click', function () {
        limparCampos();
    });

    $("#btn-finish").on("click", function () {
        finalizarTarefa();
        limparCampos();
    });

    $("#btn-finish2").on("click", function () {
        debugger;
        finalizarTarefa2();
        limparCampos();
    });

    $("#btn-cancel").on("click", function () {
        cancelarTarefa();
        limparCampos();
    });

    $("#btn-delete").on("click", function () {
        deletarTarefa();
        limparCampos();
    });

    $('#tabtarefas tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selecionado')) {
            $(this).removeClass('selecionado');
        }
        else {
            table.$('tr.selecionado').removeClass('selecionado');
            $(this).addClass('selecionado');
        }
    });

    function preencherCampos() {
        var tarefaSelecionada = $(".selecionado")[0];
        $("#detalhe-titulo").val(tarefaSelecionada.children[0].innerHTML);
        $("#detalhe-descricao").val(tarefaSelecionada.children[1].innerHTML);
        $("#detalhe-responsavel").val(tarefaSelecionada.children[4].attributes[0].textContent);
        $("#detalhe-urgencia").val(tarefaSelecionada.children[2].attributes[0].textContent);
        $("#detalhe-prazo").val(FormataStringData(tarefaSelecionada.children[3].innerHTML));
    }

    $('#tabtarefas tbody').on('click', 'tr', function () {
        preencherCampos();
        carregarAtividades();
        $('#modalDetalheTarefa').modal({
            backdrop: "static"
        });
    });

    $('#add-tarefa').on('click', function () {
        $('#modalAdicionarTarefa').modal({
            backdrop: "static"
        });
    });

    function FormataStringData(data) {
        var dia = data.split("/")[0];
        var mes = data.split("/")[1];
        var ano = data.split("/")[2];

        return ano + '-' + ("0" + mes).slice(-2) + '-' + ("0" + dia).slice(-2);
        // Utilizo o .slice(-2) para garantir o formato com 2 digitos.
    }
});