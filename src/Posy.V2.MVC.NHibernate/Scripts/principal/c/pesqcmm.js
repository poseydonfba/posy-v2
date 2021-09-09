$(document).ready(function () {

    $("#liPesqCmm").addClass("mn-horiz-active");

    fnView();

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "getCmmPesquisaView";
        _ws.type = "GET";
        _ws.ajaxWs(function (res) {

            objLoader.stop();

            var response = res.d.Result.Result;

            var array = response,
                view = array[0];

            $("#containerPesqCmmView").html(view[0]).hide().fadeIn("slow");

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }

});