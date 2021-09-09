var _btnMenuSair;

$(document).ready(function () {

    fnUsuarioOnLine();

    /*var intervaloUsuOnLine = window.setInterval(function () {
        fnUsuarioOnLine();
    }, 2000);*/

    $(document).on("click", ".tabs-tab-label", function () {
        $(".tabs-tab-content").hide();
        $("." + $(this).attr("idcontent")).show();
        $('.tabs-tab-label').removeClass("tabs-tab-label-selected");
        $(this).addClass("tabs-tab-label-selected");
    });

    $(document).on("click", "#containerAmigos .ms-cnt-blc-lnk-tit-vt", function () {
        alert(ok);
    });

    $(document).on("click", fnGetIDMenuVertical() + " .btnViewPerfil", function () {

        if ($(this).attr("load") == "add") {

            var objMenuLi = $(this).closest(".menu-li"),
                v_codperfil_view = $(this).attr("cp");

            var model = new Object();
            model.UsuarioIdSolicitado = v_codperfil_view;

            var dto = { "model": model };

            var objLoader = $(".mv-loading").boxLoader();

            _ws.clean();
            _ws.service = "UsuarioWebService";
            _ws.metodo = "AddAmigo";
            _ws.data = JSON.stringify(dto);
            _ws.ajaxWs(function (res) {

                objLoader.stop();

                var response = res.d.Result.Result;

                objMenuLi.fadeOut(300, function () {
                    $(this).remove();
                    $(response).hide().prependTo(fnGetIDMenuVertical() + " .mn-vert").fadeIn("slow");
                });

            }, function (err) {
                objLoader.stop();
                fnErrorGlobal(err);
            });

        } else if ($(this).attr("load") == "del") {

            var objMenuLi = $(this).closest(".menu-li"),
                v_codperfil_view = $(this).attr("cp"),
                v_codperfil_log = $(this).attr("cpl");

            var model = new Object();
            model.UsuarioIdParaExcluir = v_codperfil_view;

            var dto = { "model": model };

            var objLoader = $(".mv-loading").boxLoader();

            _ws.clean();
            _ws.service = "UsuarioWebService";
            _ws.metodo = "ExcluirAmigo";
            _ws.data = JSON.stringify(dto);
            _ws.ajaxWs(function (res) {

                objLoader.stop();

                var response = res.d.Result.Result;

                objMenuLi.fadeOut(300, function () {
                    $(this).remove();
                    $(response).hide().prependTo(fnGetIDMenuVertical() + " .mn-vert").fadeIn("slow");
                });

                $(".ms-cnt-blc-vis-recente").hide();
                $(fnGetIDContainerVisitante()).html("");
                $(fnGetIDContainerAmigos()).html("");
                $(fnGetIDContainerCmm()).html("");

            }, function (err) {
                objLoader.stop();
                fnErrorGlobal(err);
            });
        }
    });

    $(document).on("click", fnGetIDMenuVertical() + " .btnViewCmm", function () {

        if ($(this).attr("load") == "add") {

            var objMenuLi = $(this).closest(".menu-li"),
                v_codcmm_view = $(this).attr("cc");

            var objLoader = $(".mv-loading").boxLoader();

            _ws.clean();
            _ws.service = "ComunidadeWebService";
            _ws.metodo = "AddCmm";
            _ws.ajaxWs(function (res) {

                objLoader.stop();

                objMenuLi.fadeOut(300, function () {
                    $(this).remove();
                    $("<li class='menu-li'><div class='icon-cmm'></div><a cc='" + v_codcmm_view + "' href='javascript:void(0);'>Aguardando pedido de entrada</a></li>").hide().prependTo(fnGetIDMenuVertical() + " .mn-vert").fadeIn("slow");
                });

            }, function (err) {
                objLoader.stop();
                fnErrorGlobal(err);
            });
        }
    });

});

function fnGetIDMenuVertical() { return ".mn-cnt-vert"; }
function fnGetIDContainerVisitante() { return "#containerVisitante"; }
function fnGetIDContainerAmigos() { return "#containerAmigos"; }
function fnGetIDContainerCmm() { return "#containerCmm"; }
function fnGetIDContainerPerfilAmigosView() { return "#containerPerfilAmigosView"; }

function fnLimparViewPerfil() {
    /*
        $(".lblNomeUsu").html("");
        $(".lblFrase").html("");
    
        $(".ms-cnt-blc-vis-recente").hide();
        $(fnGetIDMenuVertical()).html("");
        $(fnGetIDContainerVisitante()).html("");
        $(fnGetIDContainerAmigos()).html("");
        $(fnGetIDContainerCmm()).html("");
    */
}
function fnShowViewPerfil(perfil, amigos, cmm, menuVertical, visitantes) {

    $("#fotoPerfil").attr("src", perfil.Foto);
    $(".lblNome").html(perfil.Nome);
    $(".lblSobrenome").html(perfil.Sobrenome);
    $(".lblFrase").html(perfil.FraseHtml);
    $("#liPerfil a").attr("href", perfil.Alias);

    $(fnGetIDContainerAmigos()).html(amigos); //.hide().fadeIn("slow");
    $(fnGetIDContainerCmm()).html(cmm); //.hide().fadeIn("slow");
    $(fnGetIDMenuVertical()).html(menuVertical); //.hide().fadeIn("slow");
    $(fnGetIDContainerVisitante()).html(visitantes); //.hide().fadeIn("slow");

    if (visitantes != "-1")
        $(".ms-cnt-blc-vis-recente").show();
    else
        $(".ms-cnt-blc-vis-recente").hide();
}
function fnShowViewPerfilData(view) {

    $(".view-titulo").html(view[0]);

    $(".view-page").html(view[1]).hide().fadeIn("slow");

    if (view[2] != "")
        $(".view-pager").html(view[2]);
}

function fnInicializaViewPerfil() {

    fnIniTooltipSubMenu();

    $(".tooltip2").tipsy({ gravity: 's' });
}


function fnLimparViewCmm() {
    /* $(".lblNome").html(""); */
}
function fnShowViewCmm(perfil, menuVertical, membros, view) {
    $("#fotoPerfil").attr("src", perfil.Foto);
    $(".lblNome").html(perfil.Nome);

    $(fnGetIDMenuVertical()).html(menuVertical).fadeIn("slow");
    $(fnGetIDContainerAmigos()).html(membros).fadeIn("slow");

    $(".view-page").html(view[1]).fadeIn("slow");

    if (view[0] != "")
        $(".view-titulo").html(view[0]).fadeIn("slow");

    if (view[2] != "")
        $(".view-pager").html(view[2]).fadeIn("slow");
    /*
    $("#fotoPerfil").attr("src", perfil.Foto);
    $(".lblNome").html(perfil.Nome);

    $(fnGetIDMenuVertical()).html(menuVertical).hide().fadeIn("slow");
    $(fnGetIDContainerAmigos()).html(membros).hide().fadeIn("slow");

    $(".view-page").html(view[1]).hide().fadeIn("slow");

    if (view[0] != "")
        $(".view-titulo").html(view[0]).hide().fadeIn("slow");

    if (view[2] != "")
        $(".view-pager").html(view[2]).hide().fadeIn("slow");
    */
}

function fnInicializaViewCmm() {

    fnIniTooltipSubMenu();

    $(".tooltip2").tipsy({ gravity: 's' });
}

function fnUsuarioOnLine() {

    _ws.clean();
    _ws.service = "UsuarioWebService";
    _ws.metodo = "UsuariosOnLine";
    _ws.ajaxWs(function (res) {
        $("#usuOnLine").html(res.d);
    });

}

function fnAbandonSession() {

    perfil.metodo = "AbandonSession";
    perfil.data = "";
    perfil.ajaxWs(function (resposta) {
    });
}