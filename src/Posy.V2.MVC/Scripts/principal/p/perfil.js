$(document).ready(function () {

    $("#liPerfil").addClass("mn-horiz-active");

    fnVisitarPerfil();
    fnView();

    function fnVisitarPerfil() {

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "VisitarPerfil";
        _ws.ajaxWs(function (res) {

        }, function (err) {
            fnErrorGlobal(err);
        });

    }

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getPerfilView";
        _ws.type = "GET";
        _ws.ajaxWs(function (res) {

            objLoader.stop();

            var response = res.d.Result;

            var array = response.Result,
                perfil = array[0],
                amigos = array[1],
                cmm = array[2],
                menuVertical = array[3],
                visitantes = array[4],
                view = array[5],
                view_depo = array[6];

            fnShowViewPerfil(perfil, amigos, cmm, menuVertical, visitantes);
            fnShowViewPerfilData(view);

            if (view_depo[1] != "") {
                $(".view-page-dep").html(view_depo[1]).hide().fadeIn("slow");
                $(".ms-cnt-depoimentos").fadeIn("slow");
            }

            fnInicializaViewPerfil();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });

    }
});