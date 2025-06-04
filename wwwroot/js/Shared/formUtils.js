
function rellenarSelect({
    datos,
    idDestino,
    seleccionado = null,
    llave = null,
    valor = null,
    opcionCero = false
}) {
    let $select = $("#" + idDestino);
    $select.find('option').remove();

    console.log(datos);

    if (datos != null && Object.keys(datos).length > 0) {

        //console.log(datos);
        //0 por defecto
        if (opcionCero) {
            $select.append('<option value="0">Seleccione una opción</option>');
        }

        //si se especifica el par llave valor
        if (llave != null) {
            $.each(datos, function (key, value) {
                if (seleccionado != null && seleccionado == value[llave]) {
                    $select.append(`<option value="${value[llave]}" selected>${value[valor]}</option>`);
                } else {
                    $select.append(`<option value="${value[llave]}" >${value[valor]}</option>`);
                }
            });
        } else {
            $.each(datos, function (key, value) {
                if (seleccionado != null && seleccionado == key) {
                    $select.append(`<option value="${key}" selected >${value}</option>`);
                } else {
                    $select.append(`<option value="${key}" >${value}</option>`);
                }
            });
        }

    } else {
        $select.append('<option value="0">No hay información disponible</option>');
    }
    
    $select.trigger("change");
}

function rellenarTomSelect({
    datos,
    idDestino,
    seleccionado = null,
    llave = null,
    valor = null,
    opcionCero = false
}) {
    // Obtener la instancia de Tom Select
    let selectElement = document.getElementById(idDestino);
    let tomSelectInstance = selectElement.tomselect;

    if (!tomSelectInstance) {
        console.error(`No se encontró instancia de Tom Select para #${idDestino}`);
        return;
    }

    // Limpiar opciones existentes
    tomSelectInstance.clearOptions();

    // Si hay datos, los agregamos
    if (datos && Object.keys(datos).length > 0) {
        let opciones = [];

        // Opción cero si aplica
        if (opcionCero) {
            opciones.push({ value: "0", text: "Seleccione una opción" });
        }

        if (llave !== null) {
            datos.forEach(item => {
                opciones.push({
                    value: item[llave],
                    text: item[valor]
                });
            });
        } else {
            for (let key in datos) {
                opciones.push({
                    value: key,
                    text: datos[key]
                });
            }
        }

        // Agregar las opciones y refrescar
        opciones.forEach(opcion => {
            tomSelectInstance.addOption(opcion);
        });

        tomSelectInstance.refreshOptions(false);

        // Seleccionar si se indicó
        if (seleccionado !== null) {
            tomSelectInstance.setValue(seleccionado.toString());
        } else {
            tomSelectInstance.clear(true);
        }

    } else {
        
        tomSelectInstance.clear(true);
        tomSelectInstance.clearOptions();

    }
}
