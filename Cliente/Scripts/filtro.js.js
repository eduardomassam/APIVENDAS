$(document).ready(function () {
    $('#myTable').DataTable({
        "serverSide": true,
        "ajax": {
            "url": "/Pedidos/ListarPedidos",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.nomeCliente = $('#nomeCliente').val();
                d.produto = $('#produto').val();
            }
        },
        "columns": [
            { "data": "Cod", "name": "Cod" },
            { "data": "NomeCliente", "name": "NomeCliente" },
            { "data": "Produto", "name": "Produto" },
            { "data": "Quantidade", "name": "Quantidade" },
            { "data": "Status", "name": "Status" },
            {
                "render": function (data, type, full, meta) {
                    if (full.Status == 2) {
                        return '<button type="button" class="btn btn-success">' +
                            '<a href="/Pedidos/ConfirmarPedidoEntregue/' + full.Cod + '" class="text-white">Confirmar a Entrega</a>' +
                            '</button>';
                    }
                    if (full.Status == 1) {
                        return '<button type="button" class="btn btn-danger">' +
                            '<a href="/Pedidos/CancelarPedidoNaoEnviado/' + full.Cod + '" class="text-white">Cancelar Entrega</a>' +
                            '</button>';
                    }
                    return '';
                }
            },
            {
                "render": function (data, type, full, meta) {
                    if (full.Status == 2 && full.IsEnviar == true) {
                        return '<button class="btn btn-warning" type="button" onclick="location.href=\'/Pedidos/DevolverPedido/' + full.Cod + '\'">Devolver Pedido</button>';
                    }
                    return '';
                }
            }
        ]
    });

    $('#btnFiltrar').click(function () {
        $('#myTable').DataTable().draw();
    });

    $('#btnLimparFiltro').click(function () {
        $('#nomeCliente').val('');
        $('#produto').val('');
        $('#myTable').DataTable().draw();
    });
});