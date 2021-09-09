$(document).ready(function () {

    fnView();

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "E") 
            fnExcluirModerador($(this));

    });

    function fnExcluirModerador(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cp=" + objBtn.attr("cp") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.ModeradorId = objBtn.attr("cp");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "ExcluirModerador";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $("li[cp=" + objBtn.attr("cp") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            var objTitleCntMembro = $(".ms-tit-blc-btn"),
                    totalAmigos = (objTitleCntMembro.text() === undefined || objTitleCntMembro.text() == "") ? 0 : parseInt(objTitleCntMembro.text());

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
        _ws.metodo = "getCmmModeradoresView";
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
