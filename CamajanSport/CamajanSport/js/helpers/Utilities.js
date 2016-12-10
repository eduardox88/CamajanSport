
function MostrarAlerta(titulo, tipoAlerta, mensaje)
{
    
    swal({
        title: titulo,
        text: mensaje,
        type: tipoAlerta,
        confirmButtonText: "Cerrar"
    });
};


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
};

function AjaxCall(url, data,idContenedor ,callBackFunction, IsAsync)
{
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: IsAsync,
        success: function (data) {
            //Llamada a la funcion para el callback
            if (callBackFunction == undefined) {
                //MostrarDialogo('ajaxSuccessModal', "Mensaje informativo", "La acción se realizó exitosamente.");
                MostrarAlerta("¡Enhorabuena!", 'success', 'La acción se realizó exitosamente.');
            } else if (idContenedor != "") {
                callBackFunction(idContenedor, data);
            } else {
                callBackFunction(data);
            }
        }, error: function (jqXHR, textStatus, errorThrown) {
            MostrarAlerta("Error", 'error', 'Ha ocurrido un error al realizar el llamado.' + url);
        }
    });
    

};
//Reinicia el dropdown de direcciones por el id.
function restartDropDown(id,value,text) {
    $('#' + id).empty();
    $('#' + id).append($('<option value="'+value+'" selected="selected">'+text+'</option>'));
};

function CargarDropDown(idDropDown, options) {
    var drp = $('#' + idDropDown);
    var html = '<option value="">Seleccione</option>';
    if (options.length > 0) {
        for (var i = 0; i < options.length; i++) {
            html += '<option value="' + options[i].Value + '">' + options[i].DisplayText + '</option>';
        }

        drp.html(html);
    }
};

function CargarDropDownDeportes(idDropDown, options) {
    if (options.length > 0) {
        for (var i = 0; i < options.length; i++) {
            $('#' + idDropDown).append($('<option value="' + options[i].IdDeporte + '">' + options[i].Nombre + '</option>'));
        }
    }
};

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
    
};

function formatFecha(milisegundos) {

    var milli = milisegundos
    var result = new Date(parseInt(milli.substr(6)));

    var date = new Date(Date.parse(result));

    return ((date.getUTCDate()) + "/" + (date.getMonth() + 1) + "/" + date.getFullYear());

};

function _arrayBufferToBase64(bytes) {
    var binary = '';
    var len = bytes.length;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
};

function limpiarTodosCampos(id) {
    $('#' + id + ' input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(':checkbox, :radio').prop('checked', false);

    $('#' + id).find('textarea').val('')
};

function Init_SingleSelect2($elem) {
    $elem.select2({
        placeholder: "Seleccione",
        allowClear: true
    });
};

function CrearObjeto(idContenedor) {

    var objeto = new Object();

    $('.objeto', '#' + idContenedor + '').each(function () {

        var elem = $(this);

        if (elem.is(':text')) {
            objeto[elem.attr('id')] = elem.val();
        }
        else if (elem.is(':hidden')) {
            objeto[elem.attr('id')] = ((elem.val() == '') ? 0 : elem.val());
        }
        else if (elem.is(':checkbox')) {
            objeto[elem.attr('id')] = elem.is(':checked');
        }
        else if (elem.is('select')) {
            objeto[elem.attr('id')] = elem.find(":selected").val()
        }
        else if (elem.is('textarea')) {
            objeto[elem.attr('id')] = elem.val()
        }
        else if (elem.attr('type') == 'password') {
            objeto[elem.attr('id')] = elem.val()
        }
    })

    return objeto;
};

function LlenarFormulario(obj) {

    var elem;

    for (var key in obj) {
        if (obj.hasOwnProperty(key)) {

            elem = $('#' + key + '');
            value = obj[key];

            if (elem.is(':checkbox')) {
                if (value == true) {
                    elem.prop('checked', true);
                }
            }
            else if (elem.is('select')) {
                
                elem.val(value).change();
            }
            else {
                elem.val(value);
            }
            //console.log(key + " -> " + obj[key]);
        }
    }
};

function FechaUtility(milisecs)
{

    this.date = new Date(parseInt(milisecs.substr(6)));

    this.GetDate = function () {        
        return ((this.date.getDate()) + "/" + (this.date.getMonth() + 1) + "/" + this.date.getFullYear());
    };
    this.GetDateTime = function()
    {
        return this.GetDate() + ' '+this.GetTime();
    };
    this.GetTime = function (sec) {
        var tiempo = this.date.toLocaleTimeString().split(':');
        var hora = tiempo[0].substr(1, tiempo[0].length-1);
        var result = ((parseInt(hora) < 10) ? ('0' + hora) : hora) + ':' + tiempo[1];
        var am = tiempo[2].split(' ')[1];
        if (sec) {
            result += ':'+tiempo[2];
        }
        result += ' '+am;
        return result;
    };

    this.GetMonthName = function()
    {
        switch (this.date.getMonth() + 1) {
            case 1:
                return 'Ene';
                break;
            case 2:
                return 'Feb';
                break;
            case 3:
                return 'Mar';
                break;
            case 4:
                return 'Abr';
                break;
            case 5:
                return 'May';
                break;
            case 6:
                return 'Jun';
                break;
            case 7:
                return 'Jul';
                break;
            case 8:
                return 'Ago';
                break;
            case 9:
                return 'Sep';
                break;
            case 10:
                return 'Oct';
                break;
            case 11:
                return 'Nov';
                break;
            case 12:
                return 'Dic';
                break;

            default:

        }

    };
    this.GetDay = function () {
        return this.date.getDate();
         

    }
    this.GetDayName = function()
    {
        switch (this.date.getDay()) {
            case 1:
                return 'Lun';
                break;
            case 2:
                return 'Mar';
                break;
            case 3:
                return 'Mie';
                break;
            case 4:
                return 'Jue';
                break;
            case 5:
                return 'Vie';
                break;
            case 6:
                return 'Sab';
                break;
            case 0:
                return 'Dom';
                break;
            default:

        }

    }


}



