async function nuevoUsuario() {
    try {
        await axiosRequest({
            url: "/CatalogoUsuarios/Crear",
            funcionCallback: mostrarModalGenerico
        });
    } catch (error) {
        console.error('Error al obtener la Plaza por ID:', error);
        throw error;
    }
}