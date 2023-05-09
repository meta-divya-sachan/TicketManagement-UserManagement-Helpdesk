function EditarDocumento(idDocumento) { 

}

function EliminarDocumento(idDocumento) {

    if (confirm('El documento seleccionado se eliminará. ¿Desea continuar?') === false) {
        return false;
    } 

    var data = {};
    data.idDocumento = idDocumento;
    $.ajax({
        type: "POST",
        url: "/tDocumento/EliminarDocumento", 
        async: false,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response.error === 0) {
                location.reload();
            }
            else {
                toastr.error(response.message, "");
            }
        },
        failure: function (response) {
            toastr.error(response, "");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            toastr.error(XMLHttpRequest.responseText, "");
        }
    });
} 