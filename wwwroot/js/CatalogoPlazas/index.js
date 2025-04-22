async function obtenerPlazaPorId(idPlaza) {
    try {
        const response = await axiosRequest({
            url: `/CatalogoPlazas/Editar/${idPlaza}`,
            funcionCallback: mostrarModalGenerico,
        });
        return response;
    } catch (error) {
        console.error('Error al obtener la plaza por ID:', error);
        throw error;
    }
} 


async function obtenerAreaPorId(idArea) {
    try {
        await axiosRequest({
            url: `/CatalogoAreas/Editar/${idArea}`,
            funcionCallback: mostrarModalGenerico,
        });
        
    } catch (error) {
        console.error('Error al obtener la Area por ID:', error);
        throw error;
    }
}

async function desactivarArea(idArea) {
    try {
        await axiosRequest({
            url: `/CatalogoAreas/CambiarEstado/${idArea}`,
            funcionCallback: function (datos) {
                $("#areaEstado_" + idArea).text(datos);
                if (datos == "Activo") { $("#botonEstadoArea_" + idArea).html('<i class="ti ti-square-x"></i>'); } else { $("#botonEstadoArea_" + idArea).html('<i class="ti ti-square-check"></i>'); }
            }
        });
    } catch (error) {
        console.error('Error al obtener la Area por ID:', error);
        throw error;
    }

}

async function desactivarPlaza(idPlaza) {
    try {
        await axiosRequest({
            url: `/CatalogoPlazas/CambiarEstado/${idPlaza}`,
            funcionCallback: function (datos) {
                $("#plazaEstado_" + idPlaza).text(datos);
                if (datos == "Activo") { $("#botonEstadoPlaza_" + idPlaza).html('<i class="ti ti-square-x"></i>'); } else { $("#botonEstadoPlaza_" + idPlaza).html('<i class="ti ti-square-check"></i>'); }
            }
        });
    } catch (error) {
        console.error('Error al obtener la Plaza por ID:', error);
        throw error;
    }

}

async function nuevoRegistro() {
    try {
        await axiosRequest({
            url: "/CatalogoPlazas/Crear",
            funcionCallback: mostrarModalGenerico
        });
    } catch (error) {
        console.error('Error al obtener la Plaza por ID:', error);
        throw error;
    }
}