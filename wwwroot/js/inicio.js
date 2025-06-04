$(function () {
    //cargar la carga que necesite ser cargada cuando cargue la pagina ya logueada
    //para lo demás está site.js

    //Tickets
    obtenerCantidadesPorEstado();

    //notificaciones 
    document.addEventListener("DOMContentLoaded", async () => {
        if ("Notification" in window) {
            let permission = await Notification.requestPermission();
        if (permission === "granted") {
            console.log("Permiso de notificación concedido");
            }
        }
    });

    if ('serviceWorker' in navigator) {
            navigator.serviceWorker.register(BASE_PATH+'/js/notificacionWorker.js')
                .then(reg => console.log('Service Worker registrado'))
                .catch(err => console.error('Fallo el registro del SW', err));
    }


    if (Notification.permission !== "granted") {
        Notification.requestPermission();
    }


    // Crear conexión
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(BASE_PATH + "/notificacionesHub") // Ruta al hub
        .withAutomaticReconnect()
        .build();

    // Método que se ejecuta cuando llega una notificación
    connection.on("RecibirNotificacion", (titulo, mensaje) => {
        console.log("Notificación recibida:", titulo, mensaje);

        // Muestra la notificación local
        if (Notification.permission === "granted") {
            new Notification(titulo, {
                body: mensaje,
                //icon: '/icono.png' // Opcional: ícono de tu app
            });
        }
    });

    // Iniciar la conexión
    connection.start().then(() => {
        console.log("Conectado a SignalR");
    }).catch(err => console.error("Error de conexión:", err));

    //mostrarNotificacion("Estado actualizadooooo", "Tu ticket ha sido respondido.");

});
//fin, acabó, ya no hay más, nada que ver aquí, por favor, siga su camino, no mire atrás, no mire a los lados, no mire al frente, no mire nada, solo siga su camino y no se detenga, por favor, siga su camino y no mire atrás, no mire a los lados, no mire al frente, no mire nada, solo siga su camino y no se detenga, por favor, siga su camino y no mire atrás, no mire a los lados, no mire al frente, no mire nada, solo siga su camino y no se detenga. 