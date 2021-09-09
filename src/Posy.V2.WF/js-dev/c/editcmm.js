$(document).ready(function () {

    var _txtNome, _dropCategoria, _dropTipo, editorPerfil;

    _txtNome = $("#txtNome");
    _dropCategoria = $("#dropCategoria");
    _dropTipo = $("#dropTipo");

    editorPerfil = $(".ed-cnt-cmt").editorHtml({ buttonsBottom: false });

    fnView();

    $(document).on("click", ".btnSalvarDadosCmm", function () {
        if (fnVerificaCampos("#formDados") != "n") {
            fnNotificacao("warning", "Preencha os campos corretamente.", V_TIME_MESSAGE);
            $("html, body").animate({ scrollTop: 0 }, "slow");
            return false;
        }

        fnEditarPerfilCmm($(this));
    });

    $(document).on("click", ".btnExcluirCmm", function () {
        if (confirm("Confirma a exclusão da comunidade?") == false) return false;
        fnExcluirCmm($(this));
    });

    $(document).on("keyup", "#txtNome", function () {
        $(".lblNome").text($(this).val());
    });

    $(document).on("click", ".btnSalvarDadosPrivacidade", function () {
        fnSalvarPrivacidade($(this));
    });

    function fnEditarPerfilCmm(objBtn) {

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.Alias = "";
        model.Nome = _txtNome.val();
        model.CategoriaId = _dropCategoria.val();

        var _objEditor = editorPerfil.find('.the-new-com'),
            _nodeName = _objEditor.get(0).nodeName;

        if (_nodeName == "IFRAME")
            model.DescricaoPerfil = _objEditor.contents().find("body").html();
        else
            model.DescricaoPerfil = _objEditor.val();

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "EditarCmmPerfil";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            fnNotificacao("success", "Comunidade criada com sucesso.", V_TIME_MESSAGE);

            $("label[idcontent='content-2']").closest("td").show();
            $("label[idcontent='content-3']").closest("td").show();

            var response = res.d.Result.Result;

            fnLocationHref(response);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnSalvarPrivacidade(objBtn) {

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.Tipo = _dropTipo.val();

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ComunidadeWebService";
        _ws.metodo = "EditarCmmPrivacidade";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

            $("html, body").animate({ scrollTop: 0 }, "slow");

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirCmm(objBtn) {

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var objeto = new Object();
        objeto.CodCmm = codCmm;

        var dto = { "objeto": objeto };

        perfil.metodo = "ExcluirCmm";
        perfil.data = JSON.stringify(dto);
        perfil.ajaxWs(function (resposta) {

            objContainerLoading.buttonLoader('stop');

            fnNotificacao("success", "Comunidade excluída com sucesso.", V_TIME_MESSAGE);

            fnLocationHref(resposta.d);

        });
    }

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewCmm();

        $(".lblNome").html("Nome da Comunidade");
        $(".ms-tit-blc-txt").html("Editar Comunidade");
        $(".btnCriarComunidade").hide();
        $(".btnSalvarDadosCmm").show();
        $(".btnExcluirCmm").show();

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

            _txtNome.val(perfil.Nome);
            _dropCategoria.val(perfil.CategoriaId);
            _dropTipo.val(perfil.Tipo);
            
            editorPerfil.find('.new-com-bt').click();
            editorPerfil.find('.the-new-com').contents().find("body").html(perfil.PerfilHtml);

            var $iframe = editorPerfil.find("iframe.the-new-com");
            $iframe.css('height', ($iframe.contents().find('body').height()) + 'px');

            $(".tooltip2").tipsy({ gravity: 's' });

            $(".pnlEditPerfil").show();

            $(".tabs-tab-label[idcontent='content-2']").closest("td").show();
            $(".tabs-tab-label[idcontent='content-3']").closest("td").show();

            _txtNome.focus();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }
});