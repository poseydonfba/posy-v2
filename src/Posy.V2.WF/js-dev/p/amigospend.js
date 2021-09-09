$(document).ready(function () {

    var TOTAL_ITEM_BLOCO = 6;

    fnView();

    $(document).on("click", ".tooltip-dropmenu", function () {
        $(this).tipsy("show");
    });

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "AA")
            fnAceitarAmigo($(this));

        else if ($(this).attr("opcao") == "AR")
            fnRecusarAmigo($(this));
    });

    function fnAceitarAmigo(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cppend=" + objBtn.attr("cppend") + "]");

        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.UsuarioIdAceitar = objBtn.attr("cppend");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "AceitarAmigo";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var response = res.d.Result.Result;

            $("li[cppend=" + objBtn.attr("cppend") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            var objSpanCntAmigo = $(fnGetIDMenuVertical()).find("a[item=amigo] span"),
                totalAmiPend = parseInt(objSpanCntAmigo.text()),
                objTitleCntAmigo = $(".ms-tit-blc-btn"),
                totalAmigos = (objTitleCntAmigo.text() === undefined || objTitleCntAmigo.text() == "") ? 0 : parseInt(objTitleCntAmigo.text());

            if ((totalAmiPend - 1) == 0) {
                objSpanCntAmigo.fadeOut(300, function () {
                    $(this).remove();
                });
            } else {
                objSpanCntAmigo.text((totalAmiPend - 1));
            }

            if ((totalAmigos - 1) == 0)
                objTitleCntAmigo.text("0");
            else
                objTitleCntAmigo.text((totalAmigos - 1));

            $(response[0]).hide().prependTo("#containerAmigosAdicionados ul").fadeIn("slow");

            if ($(fnGetIDContainerAmigos() + " .gl-cnt-zoom .gl-zoom").length == TOTAL_ITEM_BLOCO) {
                $(fnGetIDContainerAmigos() + " .gl-cnt-zoom .gl-zoom:last").remove();
            }
            $(response[1]).hide().prependTo(fnGetIDContainerAmigos() + " .gl-cnt-zoom").fadeIn("slow");

            $(".tooltip-dropmenu").tipsy({
                trigger: 'click',
                gravity: 'n',
                html: true,
                title: function () {
                    return $(this).find(".tooltip-dropmenu-html-container").html();
                }
            });

            $(".tooltip2").tipsy({ gravity: 's' });

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnRecusarAmigo(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cppend=" + objBtn.attr("cppend") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.UsuarioIdRecusar = objBtn.attr("cppend");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "RecusarAmigo";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $("li[cppend=" + objBtn.attr("cppend") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            var objSpanCntAmigo = $(fnGetIDMenuVertical()).find("a[item=amigo] span"),
                totalAmiPend = parseInt(objSpanCntAmigo.text()),
                objTitleCntAmigo = $(".ms-tit-blc-btn"),
                totalAmigos = (objTitleCntAmigo.text() === undefined || objTitleCntAmigo.text() == "") ? 0 : parseInt(objTitleCntAmigo.text());

            if ((totalAmiPend - 1) == 0) {
                objSpanCntAmigo.fadeOut(300, function () {
                    $(this).remove();
                });
            } else {
                objSpanCntAmigo.text((totalAmiPend - 1));
            }

            if ((totalAmigos - 1) == 0)
                objTitleCntAmigo.text("0");
            else
                objTitleCntAmigo.text((totalAmigos - 1));

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnView() {

        $(".mv-loading").buttonLoader('start2');

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getPerfilAmigosPendenteView";
        _ws.type = "GET";
        _ws.ajaxWs(function (res) {

            $(".mv-loading").buttonLoader('stop2');

            var response = res.d.Result.Result;

            var array = response,
                perfil = array[0],
                amigos = array[1],
                cmm = array[2],
                menuVertical = array[3],
                visitantes = array[4],
                view = array[5];

            fnShowViewPerfil(perfil, amigos, cmm, menuVertical, visitantes);
            fnShowViewPerfilData(view);

            fnInicializaViewPerfil();

        }, function (err) {
            $(".mv-loading").buttonLoader('stop2');
            fnErrorGlobal(err);
        });
    }
});