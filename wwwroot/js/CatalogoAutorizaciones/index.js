async function muestraCrearAutorizacion() {
    await axiosRequest({
        url: "/CatalogoAutorizaciones/Crear",
        funcionCallback: mostrarModalGenerico
    });
}

async function muestraEditarAutorizacion(idAutorizacion) {
    await axiosRequest({
        url: "/CatalogoAutorizaciones/Editar",
        funcionCallback: mostrarModalGenerico,
        parametros: { idAutorizacion: idAutorizacion }
    });
}