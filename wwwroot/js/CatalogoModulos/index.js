async function muestraCrearModulo() {
    await axiosRequest({
        url: BASE_PATH + '/CatalogoModulos/Crear',
        funcionCallback: mostrarModalGenerico
    });
}

function deshabilitaAreas(checkbox) {
    if ($(checkbox).is(":checked")) {
        $("#IdAreas-ts-control").prop({ "disabled": true });
    } else {
        $("#IdAreas-ts-control").prop({ "disabled": false });
    }
}

async function cambiarEstadoModulo(idModulo) {
    await axiosRequest({
        url: BASE_PATH + "/CatalogoModulos/CambiarEstado",
        parametros: { idModulo: idModulo },
        funcionCallback: actualizaEstadoModulo
    });
}

function actualizaEstadoModulo(datos) {
    console.log(datos);
    if (datos.activo == true) {
        $("#botonModuloEstado_"+datos.idModuloClasificacion).html('<i class="ti ti-circle-x"></i>');
    } else {
        $("#botonModuloEstado_" + datos.idModuloClasificacion).html('<i class="ti ti-circle-check"></i>');
    }

    $("#moduloEstado_" + datos.idModuloClasificacion).text(datos.estado);
}

async function mostrarEditarModulo(idModulo) {
    axiosRequest({
        url: BASE_PATH + '/CatalogoModulos/Editar',
        parametros: { idModulo: idModulo },
        funcionCallback: mostrarModalGenerico
    });
}