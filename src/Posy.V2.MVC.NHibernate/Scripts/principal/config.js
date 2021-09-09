var V_TIME_MESSAGE = 8000;

$(document).ready(function () {

    $(document).on("click", ".tabs-tab-label", function () {
        $(".tabs-tab-content").hide();
        $("." + $(this).attr("idcontent")).show();
        $('.tabs-tab-label').removeClass("tabs-tab-label-selected");
        $(this).addClass("tabs-tab-label-selected");
    });

    //$.ajaxSetup({
    //    beforeSend: function (xhr, settings) {
    //        //var element = settings.context.element;
    //        //var event = settings.context.event;

    //        var objLoader = $(".mv-loading").boxLoader();
    //    }
    //});
    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        //fnErrorGlobal(event, jqxhr, settings, thrownError);
        fnErrorGlobal(event, jqxhr, settings, thrownError);
    });

    $(document).on("click", fnGetIDMenuVertical() + " .btnViewPerfil", function () {

        var objMenuLi, v_codperfil_view, objLoader;
        var model = new Object();

        if ($(this).attr("load") === "add") {

            objMenuLi = $(this).closest(".menu-li");
            v_codperfil_view = $(this).attr("cp");

            model.UsuarioIdSolicitado = v_codperfil_view;

            objLoader = $(".mv-loading").boxLoader();

            $.post("/ajax/amigo", { model: model }, function (response) {

                objMenuLi.fadeOut(300, function () {
                    $(this).remove();
                    $(response).hide().prependTo(fnGetIDMenuVertical() + " .mn-vert").fadeIn("slow");
                });

            }).always(function () {
                objLoader.stop();
            });

        } else if ($(this).attr("load") === "del") {

            objMenuLi = $(this).closest(".menu-li");
            v_codperfil_view = $(this).attr("cp"),
                v_codperfil_log = $(this).attr("cpl");

            model.UsuarioIdParaExcluir = v_codperfil_view;

            objLoader = $(".mv-loading").boxLoader();

            $.post("/ajax/amigo/excluir", { model: model }, function (response) {

                objMenuLi.fadeOut(300, function () {
                    $(this).remove();
                    $(response).hide().prependTo(fnGetIDMenuVertical() + " .mn-vert").fadeIn("slow");
                });

                $(".ms-cnt-blc-vis-recente").hide();
                $(fnGetIDContainerVisitante()).html("");
                $(fnGetIDContainerAmigos()).html("");
                $(fnGetIDContainerCmm()).html("");

            }).always(function () {
                objLoader.stop();
            });
        }
    });

    $(document).on("click", fnGetIDMenuVertical() + " .btnViewCmm", function () {

        if ($(this).attr("load") == "add") {

            var objMenuLi = $(this).closest(".menu-li"),
                v_codcmm_view = $(this).attr("cc");

            var objLoader = $(".mv-loading").boxLoader();

            $.post("/ajax/comunidade/entrar", function (response) {

                objMenuLi.fadeOut(300, function () {
                    $(this).remove();
                    $("<li class='menu-li'><div class='icon-cmm'></div><a cc='" + v_codcmm_view + "' href='javascript:void(0);'>Aguardando pedido de entrada</a></li>").hide().prependTo(fnGetIDMenuVertical() + " .mn-vert").fadeIn("slow");
                });

            }).always(function () {
                objLoader.stop();
            });
        }
    });

});

function fnGetIDMenuVertical() { return ".mn-cnt-vert"; }
function fnGetIDContainerVisitante() { return "#containerVisitante"; }
function fnGetIDContainerAmigos() { return "#containerAmigos"; }
function fnGetIDContainerCmm() { return "#containerCmm"; }
function fnGetIDContainerPerfilAmigosView() { return "#containerPerfilAmigosView"; }

function ResolveUrl(url) {
    if (url.indexOf("~/") === 0) {
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

function fnError(errorMsg) {
    fnNotificacao("warning", errorMsg, V_TIME_MESSAGE);
}

function fnErrorGlobal(event, jqxhr, settings, thrownError) {

    console.log(event);

    var errorMsg;
    var err = jqxhr;

    // USADO COM [ExceptionHandler]
    switch (err.status) {
        case 404:
            errorMsg = err.statusText;
            break;
        default:
            errorMsg = err.responseJSON.message;
    }

    // USADO COM return new HttpStatusCodeResult(500, "Erro http 500");
    //switch (err.status) {
    //    case 400:
    //        errorMsg = err.statusText;
    //        break;
    //    case 401:
    //        errorMsg = err.statusText;
    //        break;
    //    case 500:
    //        errorMsg = err.statusText; //'Internel Server Error.'
    //        break;
    //    default:
    //        errorMsg = "Error";
    //}

    fnNotificacao("warning", errorMsg, V_TIME_MESSAGE);
}

function fnVerificaCampos(p_form) {
    var v_vazio = 'n';

    $(p_form + " .obrigatorio").each(function () {
        if ($(this).val() === "" || $(this).val() === "0") {
            $(this).closest(".half-width").addClass('has-error');
            v_vazio = 's';
        }
    });

    return v_vazio;
}

function fnCarregarDropAno(p_drop) {
    var v_dataatual = new Date();
    var v_options = '';

    for (i = 1900; i <= v_dataatual.getFullYear(); i++)
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


/* RETORNA DATA (/Date(1224043200000)/) NO FORMATO Date()*/
function fnDataJsonToDate(p_data) {

    var date = new Date(parseInt(p_data.substr(6)));
    return date;
}

function fnShowViewPerfilData(view) {

    $(".view-titulo").html(view[0]);

    $(".view-page").html(view[1]).hide().fadeIn("slow");

    if (view[2] !== "")
        $(".view-pager").html(view[2]);
}

function fnInicializaViewPerfil() {

    fnIniTooltipSubMenu();

    $(".tooltip2").tipsy({ gravity: 's' });
}

function fnInicializaViewCmm() {

    fnIniTooltipSubMenu();

    $(".tooltip2").tipsy({ gravity: 's' });
}

// https://docs.microsoft.com/pt-br/aspnet/signalr/overview/performance/signalr-performance
// Código de JavaScript do lado do cliente que remapeia reduzido nomes de propriedade para nomes legíveis
function reMap(smallObject, contract) {
    var largeObject = {};
    for (var smallProperty in contract) {
        largeObject[contract[smallProperty]] = smallObject[smallProperty];
    }
    return largeObject;
}

// https://docs.microsoft.com/pt-br/aspnet/signalr/overview/security/introduction-to-security
// Ou, o status de autenticação do usuário poderá ser alterado se o seu site usa a expiração
// deslizante com autenticação de formulários, e não há nenhuma atividade para manter o cookie de
// autenticação válido.Nesse caso, o usuário será desconectado e o nome de usuário não corresponderá
// ao nome de usuário no token de conexão.Você pode corrigir esse problema adicionando um script que
// periodicamente solicita um recurso no servidor web para manter o cookie de autenticação válido.
// O exemplo a seguir mostra como solicitar um recurso a cada 30 minutos.
function fnPing() {
    setInterval(function () {
        $.ajax({
            url: "Ping.aspx",
            cache: false
        });
    }, 1800000);
}