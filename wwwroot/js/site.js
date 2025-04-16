
function mostrarNotificacion(respuesta) {
    if (typeof respuesta === "string") {
        try {
            respuesta = JSON.parse(respuesta);
        } catch (e) {
            console.error("No se pudo parsear la notificación:", e);
            return;
        }
    }


    if (respuesta.Exito) {
        $("#toastMensaje").text(respuesta.Mensaje || 'Operación exitosa');
    } else {
        $("#toastMensaje").text(respuesta.Mensaje || 'Ocurrió un error');
    }

    // Detalles
    if (respuesta.Datos) {
        $("#toastMensaje").text(respuesta.datos);
    }

    const toastEl = document.getElementById('toastExito');
    const toast = new bootstrap.Toast(toastEl); 
    toast.show(); //

    // Redirigir si hay URL
    if (respuesta.Redireccion) {
        setTimeout(() => {
            window.location.href = respuesta.Redireccion;
        }, 2000);
    }
}