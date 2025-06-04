async function muestraCrearAutorizacion() {
    await axiosRequest({
        url: BASE_PATH + "/CatalogoAutorizaciones/Crear",
        funcionCallback: mostrarModalGenerico
    });
}

async function muestraEditarAutorizacion(idAutorizacion) {
    await axiosRequest({
        url: BASE_PATH + "/CatalogoAutorizaciones/Editar",
        funcionCallback: mostrarModalGenerico,
        parametros: { idAutorizacion: idAutorizacion }
    });
}