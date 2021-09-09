$(document).ready(function () {

    fnCarregarDropAno($("#dropCadDNAno"));
    fnCarregarDropMes($("#dropCadDNMes"));
    fnCarregarDropDia($("#dropCadDNDia"));

    $("#txtEmail").focus();

    $(".btnCadastrar").click(function () {

        //if (_txtCadNome.val() == "") {
        //    fnNotificacao("warning", "Informe seu nome.", V_TIME_MESSAGE);
        //    _txtCadNome.focus();
        //    return false;
        //} else if (_txtCadSobrenome.val() == "") {
        //    fnNotificacao("warning", "Informe seu sobrenome.", V_TIME_MESSAGE);
        //    _txtCadSobrenome.focus();
        //    return false;
        //} else if (_txtCadEmail.val() == "") {
        //    fnNotificacao("warning", "Informe um email válido.", V_TIME_MESSAGE);
        //    _txtCadEmail.focus();
        //    return false;
        //} else if (_txtCadSenha.val() == "") {
        //    fnNotificacao("warning", "Informe uma senha.", V_TIME_MESSAGE);
        //    _txtCadSenha.focus();
        //    return false;
        //} else if (_txtCadRepetirSenha.val() == "") {
        //    fnNotificacao("warning", "Repita a senha.", V_TIME_MESSAGE);
        //    _txtCadRepetirSenha.focus();
        //    return false;
        //} else if (_txtCadSenha.val() != _txtCadRepetirSenha.val()) {
        //    fnNotificacao("warning", "As senhas estão diferentes.", V_TIME_MESSAGE);
        //    _txtCadRepetirSenha.focus();
        //    return false;
        //} else if ($("#txtCaptcha").val() == "") {
        //    fnNotificacao("warning", "Informe o texto da imagem.", V_TIME_MESSAGE);
        //    $("#txtCaptcha").focus();
        //    return false;
        //}

        var objContainerLoading = $(this);
        objContainerLoading.buttonLoader('start');

        _ws.clean();
        _ws.service = "ContaWebService";
        _ws.metodo = "ValidaCaptcha";
        _ws.data = "{'captcha':'" + $("#txtCaptcha").val() + "'}";
        _ws.ajaxWs(function (res) {

            var model = {
                Nome: $("#txtCadNome").val(),
                Sobrenome: $("#txtCadSobrenome").val(),
                DataNascimento: $("#dropCadDNAno").val() + "-" + $("#dropCadDNMes").val() + "-" + $("#dropCadDNDia").val(),
                Sexo: $("#dropCadSexo").val(),
                EstadoCivil: $("#dropCadEstadoCivil").val(),
                Email: $("#txtCadEmail").val(),
                Senha: $("#txtCadSenha").val(),
                ConfirmacaoSenha: $("#txtCadRepetirSenha").val(),
                PaisId: "pt-BR"
            };

            var dto = { "model": model };

            _ws.clean();
            _ws.service = "ContaWebService";
            _ws.metodo = "Registrar";
            _ws.data = JSON.stringify(dto);
            _ws.ajaxWs(function (res) {

                objContainerLoading.buttonLoader('stop');

                var response = res.d.Result;
                fnLocationHref(response.Result);

            }, function (err) {

                objContainerLoading.buttonLoader('stop');

                var errorMsg = (err.status === 401) ? err.responseJSON.Message :
                        (err.responseJSON.d === undefined) ? JSON.parse(err.responseText).Message : err.responseJSON.d.Result.ReasonPhrase;

                fnNotificacao("warning", errorMsg, V_TIME_MESSAGE);

            });

        }, function (err) {

            objContainerLoading.buttonLoader('stop');

            $("#imgCaptcha").attr("src", defaults.V_ROUTE_URL_CAPTCHA);

            var errorMsg = (err.status === 401) ? err.responseJSON.Message :
                    (err.responseJSON.d === undefined) ? JSON.parse(err.responseText).Message : err.responseJSON.d.Result.ReasonPhrase;

            fnNotificacao("warning", errorMsg, V_TIME_MESSAGE);

        });
    });

    $(".btnLogin").click(function () {

        //if (_txtEmail.val() == "") {
        //    fnNotificacao("warning", "Informe um email válido.", V_TIME_MESSAGE);
        //    _txtEmail.focus();
        //    return false;
        //} else if (_txtSenha.val() == "") {
        //    fnNotificacao("warning", "Informe uma senha.", V_TIME_MESSAGE);
        //    _txtSenha.focus();
        //    return false;
        //}

        var objContainerLoading = $(this);
        objContainerLoading.buttonLoader('start');

        var model = {
            Email: $("#txtEmail").val(),
            Senha: $("#txtSenha").val(),
            Persistir: $("#ckPersistLogin").is(":checked")
        };

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "ContaWebService";
        _ws.metodo = "Autenticar";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            //var response = res.d.Result;

            var url = ($("#txtReturnUrl").val() === undefined) ? "inicio" : $("#txtReturnUrl").val();
            fnLocationHref(url);

        }, function (err) {

            objContainerLoading.buttonLoader('stop');

            var errorMsg = (err.status === 401) ? err.responseJSON.Message :
                    (err.responseJSON.d === undefined) ? JSON.parse(err.responseText).Message : err.responseJSON.d.Result.ReasonPhrase;

            fnNotificacao("warning", errorMsg, V_TIME_MESSAGE);

        });
    });

}).keypress(function (e) {
    if (e.which == 13)
        $(".btnLogin").click();
});

/*

        var loginData = {
            grant_type: 'password',
            username: _txtEmail.val(),
            password: _txtSenha.val()
        };

        $.ajax({
            type: "POST",
            url: "http://localhost/PoseydonRS.Api/api/security/token",
            data: loginData,
            success: function (response) {
                alert(JSON.stringify(response));

                localStorage.setItem("token", response.access_token);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });


*/