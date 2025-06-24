function guardarEncuesta() {
    let datos = {
        Nombre: $('#Nombre').val(),
        Apellido: $('#Apellido').val(),
        Pais: $('#Pais').val(),
        Rol: $('#Rol').val(),
        DestinoPrimario: $('#DestinoPrimario').val(),
        DestinoSecundario: $('#DestinoSecundario').val()
    };

    if (!datos.Nombre || !datos.Apellido || !datos.Pais || !datos.Rol || !datos.DestinoPrimario || !datos.DestinoSecundario) {
        $('#mensaje').text("Todos los campos son obligatorios.");
        return;
    }
    $.ajax({
        type: "POST",
        url: '/Home/GuardarAjax',
        data: datos,
        success: function (response) {
            if (response.success) {
                alert("Se guardó la Encuesta");
                window.location.href = '/Home/Index';
            } else {
                $('#mensaje').text(response.message);
            }
        }
    });
}

function cargarResumen() {
    $.ajax({
        url: '/Home/ObtenerResumen',
        type: 'GET',
        success: function (data) {
            let html = "<table><thead><tr><th>#</th><th>Destino</th><th>%</th><th>Diferencia</th></tr></thead><tbody>";
            for (let i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "<td>" + (i + 1) + "</td>";
                html += "<td>" + data[i].Nombre + "</td>";
                html += "<td>" + data[i].Porcentaje.toFixed(2) + "%</td>";
                html += "<td>" + (i === 0 ? "-" : data[i].Diferencia.toFixed(2) + "%") + "</td>";
                html += "</tr>";
            }
            html += "</tbody></table>";
            $('#tablaResumen').html(html);
        }
    });
}