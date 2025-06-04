async function obtenerRegistros() {
    let fechaInicio = $("#fechaInicioReporte").val();
    let fechaFinal = $("#fechaFinalReporte").val();
    await axiosRequest({
        url: BASE_PATH + '/Reportes/Registros',
        parametros: { fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        funcionCallback: cargarTablaRegistros
    });

    obtenerDetallesRegistros();
}

function cargarTablaRegistros(datos) {
    if (datos.length > 0) {
        let divRegistro = $("#registrosAtendidos");
        Object.keys(datos).forEach((key) => {
            let porcentaje = (100 / datos[key].total) * datos[key].atendido;
            divRegistro.append(
                '<div class="row mb-2">'
                + `<div class="col-4">${datos[key].nombreAgente}</div>`
                + `<div class="col-7"><div class="progress ">`
                + `<div class="progress-bar bg-info" role="progressbar" style="width: ${porcentaje}%" aria-valuenow="${datos[key].atendido}" aria-valuemin="0" aria-valuemax="${datos[key].total}"></div></div></div>`
                + `<div class="col-1 text-right">${datos[key].atendido} / ${datos[key].total}</div></div>`

            );
        });
    }
}

async function obtenerDetallesRegistros() {
    let fechaInicio = $("#fechaInicioReporte").val();
    let fechaFinal = $("#fechaFinalReporte").val();
    await axiosRequest({
        url: BASE_PATH + '/Reportes/Detalles',
        parametros: { fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        funcionCallback: cargarTablaDetalles
    });

    
}

function cargarTablaDetalles(datos) {
    $("#tablaReporteDetalles").DataTable({
        data: datos,
        columns: [
            { data:"idTicket", title:"TICKET"},
            {data:"tipoSoporte", title:"TIPO SOPORTE"},
            {data:"plaza", title:"PLAZA"},
            {data:"modulo", title:"MÓDULO"},
            {data:"nombreSolicitante", title:"NOMBRE USUARIO"},
            {data:"fechaCreacion", title:"FECHA Y HORA CREACION"},
            {data:"asunto", title:"ASUNTO"},
            {data:"cuerpo", title:"DESCRIPCIÓN (Incidencia)"},
            {data:"nombreAgente", title:"AGENTE"},
            {data:"fechaAsignacion", title:"FECHA Y HORA TOMA TICKET"},
            {data:"fechaCerrado", title:"FECHA Y HORA CIERRE TICKET"},
            {data:"fechaValidado", title:"FECHA Y HORA VALIDACION"},
            {data:"estadoTicket", title:"ESTATUS TICKET"},
        ],
        buttons: [
            {
                extend: 'excelHtml5',
                text: "<i class='ti ti-file-type-xls mr-2'></i>&nbsp;Generar reporte detallado",
                className: "mr-1 rounded btn-primary",
                attr: {
                    title: "Exportar a Excel",
                    'data-toggle': "tooltip",
                    'data-placement': "top"
                },
                exportOptions: {
                    columns: ':visible'
                },

                filename: "ReporteDetallado"
            }
        ]
    }).buttons().container().appendTo('#botonesHeader');


}

