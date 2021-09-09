$(document).ready(function () {

    fnView();

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewCmm();

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "getCmmView";
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