async function axiosRequest({
    url,
    metodo = 'GET',
    parametros = null, //Opcional si se desea mandar parámetros
    headers = null, //Opcional para agregar headers personalizados
    funcionCallback = null, //Opcional función callback
    parametrosCallback = null,//Opcional []
    errorCallback = null, //Opcional función callback en caso de error
    keyCache = null, //Opcional si se desea usar sessionStorage
    mensajeEspera = null, //Opcional si se desea un mensaje personalizado en el loader
    mostrarLoader = 1,//Opcional 0 si no se desea mostrar loader
}) {
    // Ponemos el loader a chambear
    if (mostrarLoader == 1) {
        if (mensajeEspera == null) {
            mensajeEspera = 'Obteniendo información. <br/>Un momento, por favor...'
        }
        $("#mensajeEspera").html(mensajeEspera);
        $("#cargando").show();
    }
    

    // Si se provee una clave para cachear los datos
    if (keyCache) {
        let cacheData = sessionStorage.getItem(keyCache);
        if (cacheData) {

            let datos = JSON.parse(cacheData);
            $("#cargando").hide();
            // Si existe una función de callback, la ejecutamos
            if (funcionCallback && typeof funcionCallback === 'function') {
                if (parametrosCallback != null) {
                    parametrosCallback['datos'] = datos;
                    funcionCallback(parametrosCallback);
                } else {
                    funcionCallback(datos);
                }
                
            }
            //De todos modos regresamos los datos

            return datos;
        }
    }

    try {
        // Determinamos el método y la configuración de axios
        let config = {
            method: metodo,
            url: url,
            //headers: {
            //    ...getHeaders(), 
            //    ...(headers || {})
            //},
        };

        // Si el método es GET, agregamos los parámetros como query string
        if (metodo.toUpperCase() === 'GET' && parametros) {
            const queryString = new URLSearchParams(parametros).toString();
            config.url += `?${queryString}`;
        }

        // Si el método es POST, PUT, PATCH, añadimos los datos al cuerpo de la solicitud
        if (['POST', 'PUT', 'PATCH'].includes(metodo.toUpperCase()) && parametros) {
            config.data = parametros;
        }

        let response = await axios(config);
        let resp = response.data;

        // Verificamos la estructura de la respuesta
        if (resp.redireccion != null) {
            mostrarNotificacion(resp);
            //Pa que se lea el mensaje
            setTimeout(() => {
                window.location.href = resp.redireccionar;
            }, 3000);
        } else if (!resp.exito) {

            if (typeof resp.datos == 'undefined') {
                //sin genericResponse
                return resp;
            } else {
                mostrarNotificacion(resp);
               
                return resp.datos;
            }
            
            
        } else {
            // Si se provee una clave de cache, almacenamos los datos
            if (keyCache) {
                sessionStorage.setItem(keyCache, JSON.stringify(resp.datos));
            }

            // Si existe una función de callback, la ejecutamos
            if (funcionCallback && typeof funcionCallback === 'function') {
                if (parametrosCallback != null) {
                    parametrosCallback['datos'] = resp.datos;
                    funcionCallback(parametrosCallback);
                } else {
                    //console.log(resp.datos);
                    funcionCallback(resp.datos);
                }
            }

            //Si hay mensaje lo mostramos
            if (resp.mensaje != null) {
                mostrarNotificacion(resp);
            }
            
            // Retornamos los datos obtenidos
            if (mostrarLoader == 1) {
                $("#cargando").hide();
            }
            
            return resp.datos;
        }
    } catch (error) {
        
        if (error.response) {
            //toastr.options = { "positionClass": "toast-top-center" };
            if (error.response.status === 401) {
                // Redirigir al logout automáticamente
                window.location.href = `${UrlServer}Login/LogOut`;
                return;
            } else if (error.response.status === 400) {
               // mostrarNotificacion(resp);
                //toastr["error"](
                //    `Error ${error.response.status}: ${error.response.data?.msj || ' Asegúrese de haber capturado o seleccionado los campos correctamente.'}`,
                //    "Error"
                //);
            } else {
                console.log(error);
                
                if (errorCallback && typeof errorCallback === 'function') {
                    errorCallback();
                    
                } else {
                    //mostrarNotificacion(resp);
                    //toastr["error"](
                    //    `Algo salió mal, el servidor devolvió un error.`,
                    //    "Error"
                    //);
                }
                
                
            }
            
            
        } else {
            console.log("algo salio mal"+error);
            //mostrarNotificacion(resp);
            //toastr.options = { "positionClass": "toast-top-center" };
            //toastr["error"](
            //    `Algo salió mal, no se pudo enviar la petición. ${error.message || ''}`,
            //    "Error"
            //);
        }
        if (mostrarLoader == 1) {
            $("#cargando").hide();
        }
        return [];
    } finally {
        if (mostrarLoader == 1) {
            $("#cargando").hide();
        }

    }

}

