$(document).ready(function () {

    fnView();

    function fnView() {

        $(".mv-loading").buttonLoader('start2');

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getPerfilComunidadesView";
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