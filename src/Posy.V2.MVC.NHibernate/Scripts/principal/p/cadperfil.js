$(document).ready(function () {

    var _txtNome, _txtNomeId, _txtSobrenome, _dropPais, _dropDNDia, _dropDNMes, _dropDNAno, _dropSexo, _dropEstadoCivil, _txtFrase, _btnSalvarDadosPessoais, editorPerfil;
    var _dropVerRecados, _dropEscreverRecados, _btnSalvarDadosPrivacidade;

    _txtNome = $("#txtNome");
    _txtNomeId = $("#txtNomeId");
    _txtSobrenome = $("#txtSobrenome");
    _dropPais = $("#dropPais");
    _dropDNDia = $("#dropDNDia");
    _dropDNMes = $("#dropDNMes");
    _dropDNAno = $("#dropDNAno");
    _dropSexo = $("#dropSexo");
    _dropEstadoCivil = $("#dropEstadoCivil");
    _txtFrase = $("#txtFrase");
    _btnSalvarDadosPessoais = $(".btnSalvarDadosPessoais");

    _dropVerRecados = $("#dropVerRecados");
    _dropEscreverRecados = $("#dropEscreverRecados");
    _btnSalvarDadosPrivacidade = $(".btnSalvarDadosPrivacidade");

    editorPerfil = $(".ed-cnt-cmt").editorHtml({ buttonsBottom: false });

    fnView();

    fnCarregarDropAno(_dropDNAno);
    fnCarregarDropMes(_dropDNMes);
    fnCarregarDropDia(_dropDNDia);

    _txtNome.focus();

    _btnSalvarDadosPessoais.click(function () {
        if (fnVerificaCampos("#containerPerfilCad") != "n") {
            fnNotificacao("warning", "Preencha os campos corretamente.", V_TIME_MESSAGE);
            $("html, body").animate({ scrollTop: 0 }, "slow");
            return false;
        }

        fnSalvar($(this));
    });

    _btnSalvarDadosPrivacidade.click(function () {
        fnSalvarPrivacidade($(this));
    });

    function fnSalvar(objBtn) {

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.Nome = _txtNome.val();
        model.Sobrenome = _txtSobrenome.val();
        model.Alias = _txtNomeId.val();
        model.PaisId = _dropPais.val();
        model.DataNascimento = _dropDNAno.val() + "-" + _dropDNMes.val() + "-" + _dropDNDia.val();
        model.Sexo = _dropSexo.val();
        model.EstadoCivil = _dropEstadoCivil.val();
        model.FrasePerfil = _txtFrase.val();

        var _objEditor = editorPerfil.find('.the-new-com'),
            _nodeName = _objEditor.get(0).nodeName;

        if (_nodeName == "IFRAME")
            model.DescricaoPerfil = _objEditor.contents().find("body").html();
        else
            model.DescricaoPerfil = _objEditor.val();

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "EditarPerfil";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".lblNome").html(_txtNome.val());
            $(".lblSobrenome").html(_txtSobrenome.val());
            $(".lblFrase").html(_txtFrase.val());
            fnNotificacao("success", "Perfil salvo com sucesso.", V_TIME_MESSAGE);
            _txtNome.focus();
            $(".has-error").removeClass("has-error");
            $("html, body").animate({ scrollTop: 0 }, "slow");

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnSalvarPrivacidade(objBtn) {

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.VerRecado = _dropVerRecados.val();
        model.EscreverRecado = _dropEscreverRecados.val();

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarPrivacidade";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            fnNotificacao("success", "Perfil salvo com sucesso.", V_TIME_MESSAGE);

            $("html, body").animate({ scrollTop: 0 }, "slow");

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
        _ws.metodo = "getEditPerfilView";
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
                view = array[5];

            fnShowViewPerfil(perfil, amigos, cmm, menuVertical, visitantes, view);

            var dataNascimento = fnDataJsonToDate(perfil.DataNascimento);

            _txtNome.val(perfil.Nome);
            _txtNomeId.val(perfil.Alias);
            _txtSobrenome.val(perfil.Sobrenome);
            _dropPais.val(perfil.PaisId);
            _dropDNDia.val(dataNascimento.getDate());
            _dropDNMes.val(dataNascimento.getMonth() + 1);
            _dropDNAno.val(dataNascimento.getFullYear());
            _dropSexo.val(perfil.Sexo);
            _dropEstadoCivil.val(perfil.EstadoCivil);
            _txtFrase.val(perfil.FraseHtml);

            _dropVerRecados.val(view.VerRecado);
            _dropEscreverRecados.val(view.EscreverRecado);

            editorPerfil.find('.new-com-bt').click();
            editorPerfil.find('.the-new-com').contents().find("body").html(perfil.PerfilHtml);

            var $iframe = editorPerfil.find("iframe.the-new-com");
            $iframe.css('height', ($iframe.contents().find('body').height()) + 'px');

            $(".pnlEditPerfil").show();

            _txtNome.focus();

            fnInicializaViewPerfil();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }

});