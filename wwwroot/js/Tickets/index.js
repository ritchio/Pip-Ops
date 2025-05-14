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
