$(document).ready(function () {

    var editorPerfil;

    fnView();

    $(document).on("click", ".btnCriarTpc", function () {

        $(this).fadeOut("slow", function () {
            $(".cnt-criar-tpc").fadeIn("slow");
            $(".txtNome").focus();
        });

    });

    $(document).on("click", ".btnSalvarTpcCanc", function () {

        $(".cnt-criar-tpc").fadeOut("slow", function () {
            $(".btnCriarTpc").fadeIn("slow");
        });

    });

    $(document).on("click", ".btnSalvarTpc", function () {
        if (fnVerificaCampos("#formCriarTpc") != "n") {
            fnNotificacao("warning", "Preencha os campos corretamente.", V_TIME_MESSAGE);
            $("html, body").animate({ scrollTop: 0 }, "slow");
            return false;
        }

        fnSalvarTopico($(this));
    });

    function fnSalvarTopico(objBtn) {

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.Titulo = $(".txtNome").val();
        model.Fixo = $(".dropFixo").val();

        var _objEditor = editorPerfil.find('.the-new-com'),
            _nodeName = _objEditor.get(0).nodeName;

        if (_nodeName == "IFRAME")
            model.Descricao = _objEditor.contents().find("body").html();
        else
            model.Descricao = _objEditor.val();

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "SalvarTopico";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var response = res.d.Result.Result;

            $(response).hide().prependTo(".poseydon-forum-list-size ul").fadeIn("slow");

            var objTitle = $(".ms-tit-blc-btn"),
                totalTit = (objTitle.text() === undefined || objTitle.text() == "") ? 0 : parseInt(objTitle.text());

            objTitle.text((totalTit + 1));

            $(".cnt-criar-tpc").fadeOut("slow", function () {
                $(".btnCriarTpc").fadeIn("slow");
            });

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
        _ws.metodo = "getCmmForumView";
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

            $(".view-form").html(view[3]).hide().fadeIn("slow");

            if (view[4] == 1) {
                $(".ed-cnt-cmt-btn").show();

                editorPerfil = $(".ed-cnt-cmt").editorHtml({ buttonsBottom: false });
                editorPerfil.find('.new-com-bt').click();
            }

            fnInicializaViewCmm();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }
});