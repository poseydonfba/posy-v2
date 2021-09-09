$(document).ready(function () {

    var editorPerfil;

    fnView();

    $(document).on("click", ".ed-cnt-cmt .btnAddPost", function () {

        fnTopicoPostar($(this).closest(".ed-cnt-cmt"));

    });

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "E")
            fnExcluirPost($(this));

        else if ($(this).attr("opcao") == "M")
            fnAddModerador($(this));

    });

    function fnExcluirPost(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[codpost=" + objBtn.attr("codpost") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.PostId = objBtn.attr("codpost");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "ExcluirTopicoPost";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".list-li[codpost=" + objBtn.attr("codpost") + "]").fadeOut(300, function () {
                $(this).remove();
            });

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnAddModerador(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[codpost=" + objBtn.attr("codpost") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.UsuarioId = objBtn.attr("cp");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "AddCmmModerador";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var autorizacaoComu = "<span class='cmm-mode'>MODERADOR DA COMUNIDADE</span>";

            $(autorizacaoComu).insertAfter(".list-li[cp=" + objBtn.attr("cp") + "] .list-h3");

            $(".menuitem[cp=" + objBtn.attr("cp") + "]").closest("a").remove();

            fnIniTooltipSubMenu();

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnTopicoPostar(editorPerfil) {

        var _htmlPost;
        var _objEditor = editorPerfil.find('.the-new-com'),
            _nodeName = _objEditor.get(0).nodeName;

        if (_nodeName == "IFRAME")
            _htmlPost = _objEditor.contents().find("body").html();
        else
            _htmlPost = _objEditor.val();

        if (_htmlPost == "") {
            fnNotificacao("warning", "Nenhum texto para o post.", V_TIME_MESSAGE);
            return false;
        }

        var objContainerLoading = editorPerfil.find('.btnAddPost');
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.Descricao = _htmlPost;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "SalvarTopicoPost";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var response = res.d.Result.Result;

            $(".poseydon-cmm-post-list-size ul").append(response);
            $(".poseydon-cmm-post-list-size ul li.postado-recente").fadeIn("slow");

            fnOcultarContainerEditor(editorPerfil);

            var objTitle = $(".ms-tit-blc-btn"),
                totalTit = (objTitle.text() === undefined || objTitle.text() == "") ? 0 : parseInt(objTitle.text());

            objTitle.text((totalTit + 1));

            setTimeout(function () {
                $(".postado-recente").removeClass("postado-recente");
            }, 500);

            fnIniTooltipSubMenu();

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
        _ws.metodo = "getCmmForumTopicoView";
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

            $(".view-topico").html(view[3]).hide().fadeIn("slow");

            if (view[1] != "") {
                $(".view-editor").html(view[4]);
                editorPerfil = $(".ed-cnt-cmt").editorHtml({ autoHeight: false });
                $(".cmt-cnt-editor").show();
            }

            fnInicializaViewCmm();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }
});