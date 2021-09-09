$(document).ready(function () {

    fnView();

    $(document).on("click", ".tooltip-dropmenu", function () {
        $(this).tipsy("show");
    });

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "AE")
            fnExcluirAmigo($(this));

    });

    function fnExcluirAmigo(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[codperfil=" + objBtn.attr("codperfil") + "]");

        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.UsuarioIdParaExcluir = objBtn.attr("codperfil");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirAmigo";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $("li[codperfil=" + objBtn.attr("codperfil") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            var objTitleCnt = $(".ms-tit-blc-btn"),
                    totalItens = (objTitleCnt.text() === undefined || objTitleCnt.text() == "") ? 0 : parseInt(objTitleCnt.text());

            totalItens = ((totalItens - 1) < 0) ? 0 : totalItens;
            objTitleCnt.text((totalItens - 1));

            $(fnGetIDContainerAmigos() + " .gl-cnt-zoom .gl-zoom[codperfil=" + objBtn.attr("codperfil") + "]").fadeOut(300, function () {
                $(this).remove();
            });

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
        _ws.metodo = "getPerfilAmigosView";
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