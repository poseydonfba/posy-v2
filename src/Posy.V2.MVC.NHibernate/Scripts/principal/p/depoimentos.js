$(document).ready(function () {

    fnView();

    $(document).on("click", ".ed-cnt-cmt .btnAddPost", function () {

        if ($(this).closest(".ed-cnt-cmt").attr("id") == "cmtDepoimentoFrame")
            fnEnviarDepoimento($(this).closest(".ed-cnt-cmt"));

    });

    $(document).on("click", ".menuitem", function () {

        if ($(this).attr("opcao") == "A")
            fnAceitarDepoimento($(this));

        else if ($(this).attr("opcao") == "R")
            fnRecusarDepoimento($(this));

        else if ($(this).attr("opcao") == "E")
            fnExcluirDepoimento($(this));

    });

    $(document).on("click", ".tooltipSubMenuDrop", function () {
        $(this).tipsy("show");
    });

    function fnEnviarDepoimento(editorPerfil) {

        var _htmlPost;
        var _objEditor = editorPerfil.find('.the-new-com'),
            _nodeName = _objEditor.get(0).nodeName;

        if (_nodeName == "IFRAME")
            _htmlPost = _objEditor.contents().find("body").html();
        else
            _htmlPost = _objEditor.val();

        if (_htmlPost == "") {
            alert("Nenhum texto para o post.");
            return false;
        }

        var objContainerLoading = editorPerfil.find('.btnAddPost');
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.Depoimento = _htmlPost;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarDepoimento";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var response = res.d.Result.Result;

            $(response).hide().prependTo(".view-page .tl-cnt-default").fadeIn("slow");

            $(".view-page .tl-cnt-default .tooltip-dropmenu:eq(0)").tipsy({
                trigger: 'click',
                gravity: 'n',
                html: true,
                title: function () {
                    return $(this).find(".tooltip-dropmenu-html-container").html();
                }
            });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

            setTimeout(function () {
                $(".postado-recente").removeClass("postado-recente");
            }, 500);

            fnOcultarContainerEditor(editorPerfil);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnAceitarDepoimento(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cd=" + objBtn.attr("cd") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.DepoimentoId = objBtn.attr("cd");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "AceitarDepoimento";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var linkExcluir = "<a href='javascript:void(0);'><div class='menuitem' cd='" + objBtn.attr("cd") + "' opcao='E'>Excluir</div></a>";
            $(".tooltip-dropmenu-menu[cd=" + objBtn.attr("cd") + "]").html(linkExcluir);
            $(".tl-cnt-default-block[cd=" + objBtn.attr("cd") + "] h2 i").fadeOut(300, function () {
                $(this).remove();
            });

            $(".tooltip-dropmenu").tipsy({
                trigger: 'click',
                gravity: 'n',
                html: true,
                title: function () {
                    return $(this).find(".tooltip-dropmenu-html-container").html();
                }
            });

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnRecusarDepoimento(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cd=" + objBtn.attr("cd") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.DepoimentoId = objBtn.attr("cd");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "RecusarDepoimento";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".tl-cnt-default-block[cd=" + objBtn.attr("cd") + "]").fadeOut(300, function () {
                $(this).remove();
            });

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirDepoimento(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[cd=" + objBtn.attr("cd") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.DepoimentoId = objBtn.attr("cd");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirDepoimento";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var objTitleCnt = $(".ms-tit-blc-btn"),
                    totalItens = (objTitleCnt.text() === undefined || objTitleCnt.text() == "") ? 0 : parseInt(objTitleCnt.text());

            totalItens = ((totalItens - 1) < 0) ? 0 : totalItens;

            objTitleCnt.text((totalItens - 1));

            $(".tl-cnt-default-block[cd=" + objBtn.attr("cd") + "]").fadeOut(300, function () {
                $(this).remove();
            });

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getPerfilDepoimentosView";
        _ws.type = "GET";
        _ws.ajaxWs(function (res) {

            objLoader.stop();

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

            $(".view-editor").html(view[3]);
            $(".ed-cnt-cmt").editorHtml().show();

            $(".view-page .tl-cnt-default .tl-cnt-default-block").fadeIn("slow");

            fnInicializaViewPerfil();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }
});