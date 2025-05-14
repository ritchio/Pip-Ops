
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
    //$select.select2({
    //    theme: 'bootstrap4',
    //});

    $select.trigger("change");
}