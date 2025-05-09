$(function () {
    $("#tablaAccesos").DataTable({
        language: lenguajeTablaGenerica,
        buttons: [
            {
                extend: 'excelHtml5',
                text: "<i class='ti ti-file-type-xls'></i>&nbsp;Exportar",
                attr: {
                    title: "Exportar a Excel",
                    'data-toggle': "tooltip",
                    'data-placement': "top"

                },
                exportOptions: {
                    columns: ':visible'
                },

                filename: formatearFecha(new Date())
            }
        ]
    }).buttons().container().appendTo('#botonesAcceso');

    $("#tablaTicketsActividades").DataTable({
        language: lenguajeTablaGenerica,
         buttons: [
                {
                    extend: 'excelHtml5',
                    text: "<i class='ti ti-file-type-xls'></i>&nbsp;Exportar",
                    attr: {
                        title: "Exportar a Excel",
                        'data-toggle': "tooltip",
                        'data-placement': "top"

                    },
                    exportOptions: {
                        columns: ':visible'
                    },

                    filename: formatearFecha(new Date())
                }
            ]
        }).buttons().container().appendTo('#botonesTicketsActividades');
});


