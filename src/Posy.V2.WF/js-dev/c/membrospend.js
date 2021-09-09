$(document).ready(function () {

    fnView();

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "A")
            fnAceitarMembro($(this));

        else if ($(this).attr("opcao") == "R")
            fnRecusarMembro($(this));

    });

    function fnAceitarMembro(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cppend=" + objBtn.attr("cppend") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.MembroId = objBtn.attr("cppend");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "AceitarMembro";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var arrRetorno = res.d.Result.Result;

            $("li[cppend=" + objBtn.attr("cppend") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            var objSpanCntMembro = $(fnGetIDMenuVertical()).find("a[item=membro] span"),
                    totalAmiPend = parseInt(objSpanCntMembro.text()),
                    objTitleCntMembro = $(".ms-tit-blc-btn"),
                    totalAmigos = (objTitleCntMembro.text() === undefined || objTitleCntMembro.text() == "") ? 0 : parseInt(objTitleCntMembro.text());

            if ((totalAmiPend - 1) == 0) {
                objSpanCntMembro.fadeOut(300, function () {
                    $(this).remove();
                });
            } else {
                objSpanCntMembro.text((totalAmiPend - 1));
            }

            objTitleCntMembro.text((totalAmigos - 1));

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnRecusarMembro(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cppend=" + objBtn.attr("cppend") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.MembroId = objBtn.attr("cppend");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "RecusarMembro";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $("li[cppend=" + objBtn.attr("cppend") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            var objSpanCntMembro = $(fnGetIDMenuVertical()).find("a[item=membro] span"),
                    totalAmiPend = parseInt(objSpanCntMembro.text()),
                    objTitleCntMembro = $(".ms-tit-blc-btn"),
                    totalAmigos = (objTitleCntMembro.text() === undefined || objTitleCntMembro.text() == "") ? 0 : parseInt(objTitleCntMembro.text());

            if ((totalAmiPend - 1) == 0) {
                objSpanCntMembro.fadeOut(300, function () {
                    $(this).remove();
                });
            } else {
                objSpanCntMembro.text((totalAmiPend - 1));
            }

            objTitleCntMembro.text((totalAmigos - 1));

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewCmm();

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "getCmmMembrosPendView";
        _ws.type = "GET";
        _ws.ajaxWs(function (res) {

            objLoader.stop();

            var response = res.d.Result.Result;

            var array = response,
                perfil = array[0],
                menuVertical = array[1],
                membros = array[2],
                cmmrel = array[3],
                view = array[4];

            fnShowViewCmm(perfil, menuVertical, membros, view);

            fnInicializaViewCmm();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }
});
