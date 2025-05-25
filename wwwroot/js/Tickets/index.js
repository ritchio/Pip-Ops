async function muestraCrearTicket() {
    await axiosRequest({
        url: '/Tickets/Crear',
        funcionCallback: mostrarModalGenerico,
        parametrosCallback: {tamanio:"extra"}
    });
}

async function obtenerModulosClasificaciones(idTipoSoporte) {
    await axiosRequest({
        url: '/CatalogoModulos/Obtener',
        parametros: { idTipoSoporte: idTipoSoporte, activo: true },
        keyCache: 'modulosClasificaciones_' + idTipoSoporte,
        funcionCallback: rellenarSelect,
        parametrosCallback: { idDestino: "IdModuloClasificacion", llave: "idModuloClasificacion", valor: "nombre" }
    });

}

async function obtenerAutorizacion(idModulo) {
    await axiosRequest({
        url: "/CatalogoAutorizaciones/ObtenerPorModulo",
        parametros: { idModulo: idModulo },
        funcionCallback: function (datos) { $("#requisitos").text(datos.requisitos); console.log(idModulo + " - datos:"); console.log(datos); },
        errorCallback: function(){ $("#requisitos").text("n/a") },
        keyCache: "autorizacion_"+idModulo
    })
}

async function mostrarDetallesTicket(idTicket) {

    await axiosRequest({
        url: "/Tickets/Detalles/" + idTicket,
        funcionCallback: mostrarModalGenerico,
        parametrosCallback: {tamanio:"extra"}
    });
}

async function validarTicket(idTicket) {
    if (confirm("¿En realidad desea validar la solución de este ticket?")) {
        await axiosRequest({
            url: "/Tickets/Validar/" + idTicket,
            funcionCallback: function () { setTimeout(location.reload(), 1000); }
        });
    }
    
}

async function obtenerCantidadesPorEstado() {
    await axiosRequest({
        url: "/Tickets/CantidadesPorEstado",
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
        window.location.href = `/Tickets/Asignar/${idTicket}/${idUsuario}`;
    }
}

async function mostrarAsignarAgente(idTicket) {
    await axiosRequest({
        url: `/TicketsSoporte/AsignarAgente/${idTicket}`,
        funcionCallback: mostrarModalGenerico,
        keyCache : "asignarAgente"
    });
}