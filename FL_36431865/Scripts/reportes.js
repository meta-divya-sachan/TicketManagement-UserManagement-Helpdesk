//[{ "nombreFiltro": "Tipo de Evento (Categoría)", "idFiltro": 1, "tipoFiltro": 1, "campoBDD": "ep.codigoCategoria" },
// { "nombreFiltro": "Código de Proceso", "idFiltro": 2, "tipoFiltro": 1, "campoBDD": "ep.codigoProceso" },
// { "nombreFiltro": "Línea de Negocio", "idFiltro": 3, "tipoFiltro": 1, "campoBDD": "ep.lineaNegocio" },
// { "nombreFiltro": "Area Responsable", "idFiltro": 4, "tipoFiltro": 1, "campoBDD": "ep.unidadFuncional" },
// { "nombreFiltro": "Clasificación de Causa", "idFiltro": 5, "tipoFiltro": 1, "campoBDD": "ep.codigoClasificacion" },
// { "nombreFiltro": "Año de Certificación", "idFiltro": -1, "tipoFiltro": 2, "campoBDD": "ep.anhoCertificacion" },
// { "nombreFiltro": "Fecha de Identificación", "idFiltro": -1, "tipoFiltro": 3, "campoBDD": "ep.fechaIdentificacion" }]

// Presentar html
function PresentarHtml(othis) {
    try {
        var tr = $(othis).closest('.x-tr');
        var codigoReporte = tr.find('#hdncodigoReporte').val();
        $('#hdntipoReporte').val('1');
        $('#hdncodigoReporte').val(codigoReporte);
        ConfigurarControles(codigoReporte);
        OpenModal('FILTRO');
    }
    catch (error) {
        toastr.error(error, "");
    }
};

// Descargar excel
function DescargarExcel(othis) {
    try {
        var tr = $(othis).closest('.x-tr');
        var codigoReporte = tr.find('#hdncodigoReporte').val();
        $('#hdntipoReporte').val('2');
        $('#hdncodigoReporte').val(codigoReporte);
        ConfigurarControles(codigoReporte);
        OpenModal('FILTRO');
    }
    catch (error) {
        toastr.error(error, "");
    }
};

// Descargar pdf
function DescargarPdf(othis) {
    try {
        var tr = $(othis).closest('.x-tr');
        var codigoReporte = tr.find('#hdncodigoReporte').val();
        $('#hdntipoReporte').val('3');
        $('#hdncodigoReporte').val(codigoReporte);
        ConfigurarControles(codigoReporte);
        OpenModal('FILTRO');
    }
    catch (error) {
        toastr.error(error, "");
    }
};

// Confifurar los controles
function ConfigurarControles(codigoReporte) {
    var filtros = ObtenerFiltrosDeReporte(codigoReporte);
    var html = "";
    for (var i = 0; i < filtros.length; i++) {
        var nombreFiltro = filtros[i].nombreFiltro;
        var idFiltro = filtros[i].idFiltro;
        var tipoFiltro = filtros[i].tipoFiltro; 
        var campoBDD = filtros[i].campoBDD;

        switch (tipoFiltro) {
            case 1:
            case 4:
            case 5:
                html = html + "<div class='row xr-filtro'>";
                html = html + "<div class='col-lg-12'>";
                html = html + " <span>" + nombreFiltro + "</span>";
                html = html + " <div class='form-group'>";
                html = html + "     <select id='ddlcontrol' class='form-control'>";
                html = html + "         <option value=''>Todos</option>";
                var lista = ObtenerLista(idFiltro);
                for (var j = 0; j < lista.length; j++) {
                    html = html + "     <option value='" + lista[j].valor + "'>" + lista[j].texto + "</option>";
                };
                html = html + "     </select>";
                html = html + "     <input type='hidden' id='hdntipoFiltro' value='" + tipoFiltro + "'>";
                html = html + "     <input type='hidden' id='hdncampoBDD' value='" + campoBDD + "'>";
                html = html + " </div>";
                html = html + "</div>";
                html = html + "</div>";
                break;

            case 2:
                html = html + "<div class='row xr-filtro'>";
                html = html + "<div class='col-lg-12'>";
                html = html + " <span>" + nombreFiltro + "</span>";
                html = html + " <div class='form-group'>";
                html = html + "     <input type='text' id='txtcontrol' class='form-control'>";
                html = html + "     <input type='hidden' id='hdntipoFiltro' value='" + tipoFiltro + "'>";
                html = html + "     <input type='hidden' id='hdncampoBDD' value='" + campoBDD + "'>";
                html = html + " </div>";
                html = html + "</div>";
                html = html + "</div>";
                break;

            case 3:
                html = html + "<div class='row xr-filtro'>";
                html = html + " <div class='col-lg-6'>";
                html = html + "     <span>" + nombreFiltro + " - Inicio</span>";
                html = html + "     <div class='form-group'>";
                html = html + "         <input type='date' id='txtcontrolInicio' class='form-control'>";
                html = html + "     </div>";
                html = html + " </div>";
                html = html + " <div class='col-lg-6'>";
                html = html + "     <span>Fin</span>";
                html = html + "     <div class='form-group'>";
                html = html + "         <input type='date' id='txtcontrolFin' class='form-control'>";
                html = html + "     </div>";
                html = html + " </div>";
                html = html + " <input type='hidden' id='hdntipoFiltro' value='" + tipoFiltro + "'>";
                html = html + " <input type='hidden' id='hdncampoBDD' value='" + campoBDD + "'>";
                html = html + "</div>";
                break;
        }
    }
    $('#spFiltro').html(html);
}

// Generar reporte
function GenerarReporte() {

    // Generación del filtro
    var tipoReporte = $('#hdntipoReporte').val();
    var codigoReporte = $('#hdncodigoReporte').val();

    var filtro = ' WHERE 1 = 1 ';
    var divs = $('.xr-filtro');
    for (var i = 0; i < divs.length; i++) {
        var div = divs[i];
        var tipoFiltro = $(div).find('#hdntipoFiltro').val();
        var campoBDD = $(div).find('#hdncampoBDD').val();

        switch (tipoFiltro) {
            case '1':
                var valor_1 = $(div).find('#ddlcontrol').val();
                if (valor_1 !== '') {
                    filtro = filtro + " AND " + campoBDD + " = " + "'" + valor_1 + "'";
                }
                break;

            case '2':
                var valor_2 = $(div).find('#txtcontrol').val();
                if (valor_2 !== '') {
                    filtro = filtro + " AND " + campoBDD + " = " + "'" + valor_2 + "'";
                }
                break;

            case '3':
                var valorInicio = $(div).find('#txtcontrolInicio').val();
                var valorFin = $(div).find('#txtcontrolFin').val(); 
                if (valorInicio !== '' && valorFin !== '') {
                    filtro = filtro + " AND " + campoBDD + " BETWEEN " + "'" + valorInicio + "'" + " AND " + "'" + valorFin + "'";
                }
                break;

            case '4':
                // Fecha de 6 caracteres
                var valor_4 = $(div).find('#ddlcontrol').val();
                if (valor_4 !== '') {
                    filtro = filtro + " AND " + campoBDD + " >=  CONVERT(CHAR(6), DATEADD(MONTH, -" + valor_4 + ", GETDATE()), 112)";
                }
                break;

            case '5':
                // Fecha de 8 caracteres
                var valor_5 = $(div).find('#ddlcontrol').val();
                if (valor_5 !== '') {
                    filtro = filtro + " AND " + campoBDD + " >=  CONVERT(CHAR(8), DATEADD(MONTH, -" + valor_5 + ", GETDATE()), 112)";
                }
                break;
        }
    }

    var query = ObtenerQueryDeReporte(codigoReporte);
    query = query.replace('#FILTRO', filtro);

    // Generación del reporte
    switch (tipoReporte) {
        case '1':
            break;

        case '2':
            DescargarReporteExcel(query);
            break;

        case '3':
            break;
    }

};

// Descargar reporte excel
function DescargarReporteExcel(query) {
    var data = {};
    data.query = query;
    $.ajax({
        type: "POST",
        url: "/tReportes/GenerarReporteExcel",
        async: false,
        data: data,
        dataType: "json", 
        success: function (response) {
            if (response.error === 0) {
                var fileName = response.output; 
                if (fileName !== "") { 
                    window.location.href = "/tReportes/DescargarReporteExcel/?fileName=" + fileName;
                }
            }
            else {
                throw response.message;
            }
        },
        failure: function (response) {
            throw response;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            throw XMLHttpRequest.responseText;
        }
    });
}

// Obtener los filtros
function ObtenerFiltrosDeReporte(codigoReporte) {
    var output;
    var data = {};
    data.codigoReporte = codigoReporte;
    $.ajax({
        type: "POST",
        url: "/tReportes/ObtenerFiltrosDeReporte",
        async: false,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response.error === 0) { 
                output = response.output;
            }
            else {
                throw response.message;
            }
        },
        failure: function (response) {
            throw response;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            throw XMLHttpRequest.responseText;
        }
    });
    return output;
}

// Obtener el query
function ObtenerQueryDeReporte(codigoReporte) {
    var output;
    var data = {};
    data.codigoReporte = codigoReporte;
    $.ajax({
        type: "POST",
        url: "/tReportes/ObtenerQueryDeReporte",
        async: false,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response.error === 0) {
                output = response.output;
            }
            else {
                throw response.message;
            }
        },
        failure: function (response) {
            throw response;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            throw XMLHttpRequest.responseText;
        }
    });
    return output;
}

// Obtener la lista para enlace de combos
function ObtenerLista(idFiltro) {
    var output;
    var data = {};
    data.idFiltro = idFiltro;
    $.ajax({
        type: "POST",
        url: "/tReportes/ObtenerListaPorID",
        async: false,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response.error === 0) {
                output = response.output;
            }
            else {
                throw response.message;
            }
        },
        failure: function (response) {
            throw response;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            throw XMLHttpRequest.responseText;
        }
    });
    return output;
}
