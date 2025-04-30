async function nuevoUsuario() {
    try {
        await axiosRequest({
            url: "/CatalogoUsuarios/Crear",
            funcionCallback: mostrarModalGenerico,
            parametrosCallback: { tamanio: "extra" }
        });
    } catch (error) {
        console.error('Error al obtener la Plaza por ID:', error);
        throw error;
    }
}

async function editarUsuario(id) {
    try {
        await axiosRequest({
            url: "/CatalogoUsuarios/Editar",
            funcionCallback: mostrarModalGenerico,
            parametrosCallback: { tamanio: "extra" },
            parametros: { id:id }
        });
    } catch (error) {
        console.error('Error al obtener el Usuario por ID:', error);
        throw error;
    }
}

async function cambiarEstadoUsuario(id) {
    try {
        await axiosRequest({
            url: "/CatalogoUsuarios/CambiarEstado",
            parametros: { id: id },
            funcionCallback: mostrarCambioEstado,
            parametrosCallback: {id:id}
        });
    } catch (error) {
        console.error('Error al obtener el Usuario por ID:', error);
        throw error;
    }
}

function mostrarCambioEstado({ datos, id }) {
    let boton = (datos.estado == "activado") ? '<i class="ti ti-circle-x"></i>' : '<i class="ti ti-circle-check"></i>';
    $(".estadoUsuario_" +id).html(boton);
    $(".fechaInactivacion_" + id).html(datos.fechaInactivacion);
}