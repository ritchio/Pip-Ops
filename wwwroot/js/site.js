
function mostrarNotificacion(respuesta) {
    if (typeof respuesta === "string") {
        try {
            respuesta = JSON.parse(respuesta);
        } catch (e) {
            console.error("No se pudo parsear la notificación:", e);
            return;
        }
    }


    if (respuesta.exito) {
        $("#notificacionMensaje").text(respuesta.mensaje || 'Operación exitosa');
    } else {
        $("#notificacionMensaje").text(respuesta.mensaje || 'Ocurrió un error');
    }

    // Detalles
    if (respuesta.datos) {
        $("#notificaionMensaje").text(respuesta.datos);
    }

    const toastEl = document.getElementById('notificacionToast');
    const toast = new bootstrap.Toast(toastEl); 
    toast.show(); //

    // Redirigir si hay URL
    if (respuesta.redireccion) {
        setTimeout(() => {
            window.location.href = respuesta.redireccion;
        }, 2000);
    }
}

function mostrarModalGenerico(html) {
    if (html != null) {
        $("#modalGenericoCuerpo").html(html);
        $("#modalGenerico").modal("show");
    }
    
}
// tooltips
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})