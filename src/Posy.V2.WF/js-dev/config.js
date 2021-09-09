var V_TIME_MESSAGE = 8000;

function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = defaults.SiteRootUrl + url.substring(2);
    }
    return url;
}

function fnLocationHref(p_url) {
    window.document.location.href = p_url;
}

function fnIniTooltipSubMenu() {
    $(".tooltip-dropmenu").tipsy({
        trigger: 'click',
        gravity: 'n',
        html: true,
        title: function () {
            return $(this).find(".tooltip-dropmenu-html-container").html();
        }
    });
}

function fnErrorGlobal(err) {
    var errorMsg;

    if (err.status === 401) {
        errorMsg = err.responseJSON.Message;

    } else if (err.status === 414) {
        errorMsg = err.statusText;

    } else if (err.responseJSON.d === undefined) {
        errorMsg = JSON.parse(err.responseText).Message;

    } else {
        errorMsg = err.responseJSON.d.Result.ReasonPhrase;
    }

    fnNotificacao("warning", errorMsg, V_TIME_MESSAGE);
}

function fnWsError(xhr, msg, e) {
    if (xhr.responseText == "" && xhr.statusText == "error") {
        fnNotificacao("error", "Você esta sem acesso a internet.", V_TIME_MESSAGE);
    } else if (xhr.status == 500) {
        var err = JSON.parse(xhr.responseText);
        if (err === undefined)
            fnNotificacao("error", "Ocorreu um erro interno de servidor.", V_TIME_MESSAGE);
        else
            fnNotificacao("error", err.Message, V_TIME_MESSAGE);
    } else if (xhr.status === 414) {
        fnNotificacao("error", xhr.statusText, V_TIME_MESSAGE);
    } else {
        var err = JSON.parse(xhr.responseText);
        fnNotificacao("error", err.Message, V_TIME_MESSAGE);
    }
}

function fnVerificaCampos(p_form) {
    var v_vazio = 'n';

    $(p_form + " .obrigatorio").each(function () {
        if ($(this).val() == "" || $(this).val() == "0") {
            $(this).closest(".half-width").addClass('has-error');
            v_vazio = 's';
        }
    });

    return v_vazio;
}

function fnCarregarDropAno(p_drop) {
    var v_dataatual = new Date();
    var v_options = '';

    for (i = 1900; i <= v_dataatual.getFullYear() ; i++)
        v_options += "<option value='" + i + "'>" + i + "</option>";

    p_drop.html(v_options);
}

function fnCarregarDropMes(p_drop) {

    var v_options = '';
    v_options += '<option value="1">Janeiro</option>';
    v_options += '<option value="2">Fevereiro</option>';
    v_options += '<option value="3">Março</option>';
    v_options += '<option value="4">Abril</option>';
    v_options += '<option value="5">Maio</option>';
    v_options += '<option value="6">Junho</option>';
    v_options += '<option value="7">Julho</option>';
    v_options += '<option value="8">Agosto</option>';
    v_options += '<option value="9">Setembro</option>';
    v_options += '<option value="10">Outubro</option>';
    v_options += '<option value="11">Novembro</option>';
    v_options += '<option value="12">Dezembro</option>';

    p_drop.html(v_options);
}

function fnCarregarDropDia(p_drop) {
    var v_options = '';

    for (i = 1; i <= 31; i++)
        v_options += "<option value='" + i + "'>" + i + "</option>";

    p_drop.html(v_options);
}

function GetBrowserInfo() {
    var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;

    var isFirefox = typeof InstallTrigger !== 'undefined';   // Firefox 1.0+
    var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;

    var isChrome = !!window.chrome && !isOpera;              // Chrome 1+
    var isIE = false || !!document.documentMode;   // At least IE6
    if (isOpera) {
        return 1;
    }
    else if (isFirefox) {
        return 2;
    }
    else if (isChrome) {
        return 3;
    }
    else if (isSafari) {
        return 4;
    }
    else if (isIE) {
        return 5;
    }
    else {
        return 0;
    }
}

var login = {

    baseWsUrl: defaults.SiteRootUrl,
    metodo: "",
    data: "",

    ajaxWs: function (callback) {
        $.ajax({
            type: "POST",
            url: this.baseWsUrl + this.metodo,
            data: this.data,
            dataType: "json",
            success: function (response) {
                callback.call(this, response);
            },
            error: function (xhr, msg, e) {
                fnWsError(xhr, msg, e);
            }
        });
    },

    getMetodo: function () { return this.metodo; },
    getData: function () { return this.data; }

};

var perfil = {

    baseWsUrl: defaults.SiteRootUrl + "ws/wsSite.asmx/",
    metodo: "",
    data: "",

    ajaxWs: function (callback) {
        $.ajax({
            type: "POST",
            url: this.baseWsUrl + this.metodo,
            data: this.data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response, status) {
                callback.call(this, response, status);
            },
            error: function (response, status, e) {
                callback.call(this, response, status);
                fnWsError(response, status, e);
            }
        });
    },

    getMetodo: function () { return this.metodo; },
    getData: function () { return this.data; }

};

var objWsPub = {

    baseWsUrl: defaults.SiteRootUrl + "ws/wsPublico.asmx/",
    metodo: "",
    data: "",
    msgError: 1,

    ajaxWs: function (callback) {
        $.ajax({
            type: "POST",
            url: this.baseWsUrl + this.metodo,
            data: this.data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response, status) {
                /* objResult = JSON.parse(response.d); */
                /* objResult = eval('(' + response.d + ')'); */
                /* objResult = $.parseJSON(response.d); */

                var objResult = JSON.parse(response.d);
                callback.call(this, objResult, status);
            },
            error: function (response, status, e) {
                callback.call(this, response, status);
                if (this.msgError == 1)
                    fnWsError(response, status, e);
            }
        });
    },

    getMetodo: function () { return this.metodo; },
    getData: function () { return this.data; }

};

var _ws = {

    baseWsUrl: defaults.SiteRootUrl,
    service: "",
    metodo: "",
    data: "",
    async: true,
    type: "POST",
    cache: false,

    ajaxWs: function (callback_success, callback_error) {

        $.ajax({
            type: this.type,
            url: this.baseWsUrl + "WebServices/" + this.service + ".asmx/" + this.metodo,
            data: this.data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: this.async,
            cache: this.cache,
            success: function (response, status) {
                callback_success.call(this, response, status);
            },
            error: function (response, status) {
                callback_error.call(this, response, status);
            }
        });
    },

    clean: function () {
        this.baseWsUrl = defaults.SiteRootUrl;
        this.service = "";
        this.metodo = "";
        this.data = "";
        this.async = true;
        this.type = "POST";
        this.cache = false;
    },

    getService: function () { return this.service; },
    getMetodo: function () { return this.metodo; },
    getData: function () { return this.data; }
};


/* RETORNA DATA (/Date(1224043200000)/) NO FORMATO Date()*/
function fnDataJsonToDate(p_data) {

    var date = new Date(parseInt(p_data.substr(6)));
    return date;
}