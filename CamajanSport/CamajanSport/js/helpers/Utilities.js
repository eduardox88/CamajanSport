function MostrarAlerta(tipoAlerta,mensaje)
{
    var height = window.screen.availHeight
    var width = window.screen.availWidth
    $('<div class="alert alert-'+tipoAlerta+' alert-dismissable" style="z-index:9000;position:absolute;top:10%;left:40%"><button type = "button" class = "close" data-dismiss = "alert" aria-hidden = "true">&times;</button>'+mensaje+'</div>').appendTo($('form'));
}

function MostrarDialogo(id, titulo, mensaje, showBtnCerrar, botones, showTopCloseBtn, backdropClose) {
    showBtnCerrar = (showBtnCerrar == undefined) ? true : showBtnCerrar;
    if (backdropClose == undefined || backdropClose == false ) {
        backdropClose = false;
    } else {
        backdropClose = true;
    }
    var btnX = $('<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>').click(function () {
        $('#'+id).modal('hide');
    });
    var modal = $('<!-- Modal -->' +
        '<div id="' + id + '" class="myCustomModal modal fade" tabindex="-1" role="dialog" data-backdrop="' + backdropClose + '">' +
        '<div class="modal-dialog">' +
        '<!-- Modal content-->' +
        '<div class="modal-content">' +
        '<div class="modal-header">' +
        '<h4 class="modal-title">'+titulo+'</h4>' +
        '</div>' +
        '<div class="modal-body">' +
        '<p>'+mensaje+'</p>' +
        '</div>' +
        '<div class="modal-footer">'+
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>');

    modal.appendTo($('body'));
    if (showTopCloseBtn == undefined || showTopCloseBtn) {
        $('#' + id).find('.modal-header').append(btnX);
    }
    
    if (botones != undefined && botones.length > 0) {
        for (var i = 0; i < botones.length; i++)
        {
            $('#' + id).find('.modal-footer').append(botones[i]);
        }
    }
    if (showBtnCerrar) {
        $('#' + id).find('.modal-footer').append($('<button type="button" id="btnModalClose" class="btn btn-default" data-dismiss="modal">Cerrar</button>').click(function () {
            $('#' + id).modal('hide');

        }));
    }
    
    $('#' + id).on('hidden.bs.modal', function (event) {
        $(this).remove();
    });
    $('#' + id).modal();
}

function AjaxCall(url, data,idContenedor ,callBackFunction)
{    
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //Llamada a la funcion para el callback
            if (callBackFunction == undefined) {
                MostrarDialogo('ajaxSuccessModal',"Mensaje informativo","La acción se realizó exitosamente.");
            } else if(idContenedor != ""){
                callBackFunction(idContenedor, data);
            } else {
                callBackFunction(data);
            }
        }, error: function (jqXHR, textStatus, errorThrown) {
            MostrarDialogo('ajaxErrorModal', "Error", textStatus);
        }
    });
}
//Reinicia el dropdown de direcciones por el id.
function restartDropDown(id,value,text) {
    $('#' + id).empty();
    $('#' + id).append($('<option value="'+value+'" selected="selected">'+text+'</option>'));
}

function CargarDropDown(idDropDown, options) {
    if (options.d.length > 0) {
        for (var i = 0; i < options.d.length; i++) {
            $('#' + idDropDown).append($('<option value="' + options.d[i].Value + '">' + options.d[i].DisplayText + '</option>'));
        }
    }
    
}

function LimpiarRecibirEnvioModal() {

    $('#hdfIdEnvio').val('');
    $('#txtAlbaran').val('');
    $('#lblRemitente').text('');
    $('#lblDirRemitente').text('');
    $('#lblDestinatario').text('');
    $('#lblDirDestinatario').text('');
    $('#lblPuertoOrigen').text('');
    $('#lblPuertoDestino').text('');
    $('#lblFechaEnvio').text('');
    $('#tblListaPaquetes tbody tr').empty();
    $('#divDetalles').addClass('hidden');
    $('#pnlRecepcion').addClass('hidden');
    $('#divRecepcionEnvio').appendTo('body');
    
}