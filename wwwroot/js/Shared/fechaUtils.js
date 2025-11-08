//La idea es centralizar y estandarizar las utilidades más usadas

function formatearFecha(cadenaFecha, tipo = "dd/mm/YYYY") {
    if (!cadenaFecha) return ""; //Fecha no válida

    let fecha = new Date(cadenaFecha);
    if (isNaN(fecha.getTime())) {
        //tampoco es válida pero logeamos
        console.log(cadenaFecha, " no es una fecha válida.");
        return "";
    }

    let formato = null;

    switch (tipo) {
        case "dd/mm/YYYY hh:ii A": // 12 horas con AM/PM
            formato = new Intl.DateTimeFormat("es-MX", {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
                hour: "2-digit",
                minute: "2-digit",
                hour12: true,
            });
            break;

        case "dd/mm/YYYY HH:ii:ss": // 24 horas
            formato = new Intl.DateTimeFormat("es-MX", {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
                hour: "2-digit",
                minute: "2-digit",
                second: "2-digit",
                hour12: false, 
            });
            break;
        //Agregar los que se necesiten
        case "dd/mm/YYYY":
        default:
            formato = new Intl.DateTimeFormat("es-MX", {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
            });

            break;
    }

    return formato.format(fecha);
}