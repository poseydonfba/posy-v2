$(document).ready(function () {

    fnView();

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "E")
            fnExcluirMembro($(this));

    });

    function fnExcluirMembro(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cp=" + objBtn.attr("cp") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.UsuarioMembroId = objBtn.attr("cp");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "ExcluirMembro";
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $("li[cp=" + objBtn.attr("cp") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            var objTitle = $(".ms-tit-blc-btn"),
                totalTit = (objTitle.text() === undefined || objTitle.text() == "") ? 0 : parseInt(objTitle.text());

            objTitle.text((totalTit - 1));

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
        _ws.metodo = "getCmmMembrosView";
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
