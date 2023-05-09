// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function OpenModal(modal) {
    try {
        var panel = "#modal" + modal;
        $(panel).modal("show");
    }
    catch (e) {
        Common.ShowToastrMessage(Common.Variable.Error, Common.Variable.Error, e.message);
    }
};

function CloseModal(modal) {
    var panel = "#modal" + modal;
    $(panel).modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
    return false;
};


// "2023-04-18T00:00:00"

function ToDateDDMMYYYY(value) {
    if (value.length === 19) {
        var day = value.substring(8, 10);
        var month = value.substring(5, 7);
        var year = value.substring(0, 4); 
        return '' + day + '/' + month + '/' + year;
    }
    else {
        return '';
    }
}

function ToDateYYYYMMDD(value) {
    if (value.length === 19) {
        var day = value.substring(8, 10);
        var month = value.substring(5, 7);
        var year = value.substring(0, 4);
        return '' + year + '-' + month + '-' + day;
    }
    else {
        return '';
    }
}
