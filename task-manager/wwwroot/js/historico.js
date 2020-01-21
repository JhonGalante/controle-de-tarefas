$(document).ready(function () {

    var table = initTable();

    function initTable() {
        debugger;
        return $('#tabtarefashistorico').DataTable({
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

    function carregarAtividades() {
        var carregarAtividadesLink = '/Home/CarregarAtividadesHistorico';
        var idTarefa = $(".selecionado-historico")[0].attributes[0].textContent;
        var data = {
            IdTarefa: idTarefa
        };
        $("#partial-lista-tarefas-historico").load(carregarAtividadesLink, data);
    }

    function deletarTarefa() {
        var deletarTarefaLink = '/Home/DeletarTarefa';
        var idTarefa = $(".selecionado-historico")[0].attributes[0].textContent;
        var data = {
            Id: idTarefa,
        };
        $("#tabela-historico-view").load(deletarTarefaLink, data);
    }

    $('#btn-fechar-detalhe-historico').on('click', function () {
        limparCampos();
    });

    $('#btn-apagar-detalhe-historico').on('click', function () {
        deletarTarefa();
        limparCampos();
    });

    $('#close-historico').on('click', function () {
        limparCampos();
    });

    $('#tabtarefashistorico tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selecionado-historico')) {
            $(this).removeClass('selecionado-historico');
        }
        else {
            table.$('tr.selecionado-historico').removeClass('selecionado-historico');
            $(this).addClass('selecionado-historico');
        }
    });

    function preencherCampos() {
        var tarefaSelecionada = $(".selecionado-historico")[0];
        $("#detalhe-titulo-historico").val(tarefaSelecionada.children[0].innerHTML);
        $("#detalhe-descricao-historico").val(tarefaSelecionada.children[1].innerHTML);
        $("#detalhe-responsavel-historico").val(tarefaSelecionada.children[4].attributes[0].textContent);
        $("#detalhe-urgencia-historico").val(tarefaSelecionada.children[2].attributes[0].textContent);
        $("#detalhe-prazo-historico").val(FormataStringData(tarefaSelecionada.children[3].innerHTML));
    }

    function limparCampos() {
        $("#detalhe-titulo-historico").val("");
        $("#detalhe-descricao-historico").val("");
        $("#detalhe-responsavel-historico").val("0");
        $("#detalhe-urgencia-historico").val("0");
        $("#detalhe-prazo-historico").val("");
        $(".selecionado-historico").removeClass("selecionado-historico");
    }

    $('#tabtarefashistorico tbody').on('click', 'tr', function () {
        preencherCampos();
        carregarAtividades();
        $('#modalDetalheTarefaHistico').modal({
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