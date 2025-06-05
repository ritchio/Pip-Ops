async function muestraCrearTicket() {
    await axiosRequest({
        url: BASE_PATH + '/Tickets/Crear',
        funcionCallback: mostrarModalGenerico,
        parametrosCallback: {tamanio:"extra"}
    });
}

async function obtenerModulosClasificaciones(idTipoSoporte) {
    await axiosRequest({
        url: BASE_PATH + '/CatalogoModulos/Obtener',
        parametros: { idTipoSoporte: idTipoSoporte, activo: true },
        keyCache: 'modulosClasificaciones_' + idTipoSoporte,
        funcionCallback: rellenarSelect,
        parametrosCallback: { idDestino: "IdModuloClasificacion", llave: "idModuloClasificacion", valor: "nombre" }
    });

}

async function obtenerAutorizacion(idModulo) {
    await axiosRequest({
        url: BASE_PATH + "/CatalogoAutorizaciones/ObtenerPorModulo",
        parametros: { idModulo: idModulo },
        funcionCallback: function (datos) { $("#requisitos").text(datos.requisitos); console.log(idModulo + " - datos:"); console.log(datos); },
        errorCallback: function(){ $("#requisitos").text("n/a") },
        keyCache: "autorizacion_"+idModulo
    })
}

async function mostrarDetallesTicket(idTicket) {

    await axiosRequest({
        url: BASE_PATH + "/Tickets/Detalles/" + idTicket,
        funcionCallback: mostrarModalGenerico,
        parametrosCallback: {tamanio:"extra"}
    });
}

async function validarTicket(idTicket) {
    if (confirm("¿En realidad desea validar la solución de este ticket?")) {
        await axiosRequest({
            url: BASE_PATH + "/Tickets/Validar/" + idTicket,
            funcionCallback: function () { setTimeout(location.reload(), 1000); }
        });
    }
    
}

async function obtenerCantidadesPorEstado() {
    await axiosRequest({
        url: BASE_PATH + "/Tickets/CantidadesPorEstado",
        funcionCallback: function (datos) {

            Object.keys(datos).forEach((item) => {

                if (datos[item].contexto == "VistaGeneral") {
                    $("#banderaTicketsSoporte" + datos[item].estado).text(`(${datos[item].cantidad})`);
                } else {
                    $("#banderaTicketsUsuario" + datos[item].estado).text(`(${datos[item].cantidad})`);
                }
                
            });
            
        }
    });
}

async function asignarTicket(idTicket,idUsuario) {
    let mensaje = (idUsuario == "auto") ? "¿Realmente desea asignarse este ticket?" : "¿Realmente desea asignar el agente a este ticket?"

    if (confirm(mensaje)) {
        window.location.href = `${BASE_PATH}/Tickets/Asignar/${idTicket}/${idUsuario}`;
    }
}

async function mostrarAsignarAgente(idTicket) {
    await axiosRequest({
        url: BASE_PATH + `/TicketsSoporte/AsignarAgente/${idTicket}`,
        funcionCallback: mostrarModalGenerico,
        keyCache : "asignarAgente"
    });
}