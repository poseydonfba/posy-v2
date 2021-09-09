$(document).ready(function () {

    fnView();

    $(document).on("click", ".ed-cnt-cmt .btnAddPost", function () {

        if ($(this).closest(".ed-cnt-cmt").attr("id") == "cmtRecadoFrame")
            fnEnviarRecado($(this).closest(".ed-cnt-cmt"));
        else
            fnRecadoComentario($(this).closest(".ed-cnt-cmt"));

    });

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "RE")
            fnExcluirRecado($(this));

        else if ($(this).attr("opcao") == "RB")
            fnBloquearPost($(this));

        else if ($(this).attr("opcao") == "RCE")
            fnExcluirRecadoComentario($(this));
    });

    $(document).on("click", ".btnExcluirRecadoCheck", function () {
        fnExcluirMultiplosRecados($(this));
    });

    $(document).on("click", ".btnVerMais", function () {
        fnRecadoExibirMaisComents($(this), $(this).attr("codrecado"), $(this).attr("page"));
    });

    $(document).on("click", ".btnVerMenos", function () {
        fnRecadoExibirMenosComents($(this), $(this).attr("codrecado"), 2);
    });

    function fnEnviarRecado(editorPerfil) {

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
        model.RecadoHtml = _htmlPost;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarRecado";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var arrRetorno = res.d.Result.Result;

            if ($(".view-page .tl-cnt-default").length == 0) {
                $(".view-page").html("<section class='tl-cnt-default'></section>");
            }

            $(arrRetorno[0]).hide().prependTo(".view-page .tl-cnt-default").fadeIn("slow");
            $(".view-page .tl-cnt-default").find(".ed-cnt-cmt:eq(0)").editorHtml();

            var objTitleCnt = $(".ms-tit-blc-btn"),
                    totalItens = (objTitleCnt.text() === undefined || objTitleCnt.text() == "") ? 0 : parseInt(objTitleCnt.text());
            objTitleCnt.text((totalItens + 1));

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

            setTimeout(function () {
                $(".postado-recente").removeClass("postado-recente");
            }, 500);

            fnOcultarContainerEditor(editorPerfil);

            $(".view-page .tl-cnt-default .tooltip-dropmenu:eq(0)").tipsy({
                trigger: 'click',
                gravity: 'n',
                html: true,
                title: function () {
                    return $(this).find(".tooltip-dropmenu-html-container").html();
                }
            });

            $(".view-page .tl-cnt-default .cmt-cnt-img-perfil-tooltip-big:eq(0)").tipsy({
                trigger: 'click',
                gravity: 's',
                html: true,
                title: function () {
                    return $(this).next().html();
                }
            });

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnRecadoComentario(editorPerfil) {

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
        model.RecadoId = editorPerfil.closest(".tl-cnt-default-block").attr("codrecado");
        model.Comentario = _htmlPost;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarRecadoComentario";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            fnOcultarContainerEditor(editorPerfil);

            var response = res.d.Result.Result;

            $(response).hide().prependTo(editorPerfil.find(".ed-cnt-cmt-coments")).fadeIn("slow");

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

            setTimeout(function () {
                $(".postado-recente").removeClass("postado-recente");
            }, 500);

            editorPerfil.find(".ed-cnt-cmt-coments").find(".tooltip-dropmenu:eq(0)").tipsy({
                trigger: 'click',
                gravity: 'n',
                html: true,
                title: function () {
                    return $(this).find(".tooltip-dropmenu-html-container").html();
                }
            });

            $(".cmt-cnt-img-perfil-tooltip-big").tipsy({
                trigger: 'click',
                gravity: 's',
                html: true,
                title: function () {
                    return $(this).next().html();
                }
            });

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirRecado(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[codrecado=" + objBtn.attr("codrecado") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.RecadoIdParaExcluir = objBtn.attr("codrecado");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirRecado";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var objTitleCnt = $(".ms-tit-blc-btn"),
                    totalItens = (objTitleCnt.text() === undefined || objTitleCnt.text() == "") ? 0 : parseInt(objTitleCnt.text());
            totalItens = ((totalItens - 1) < 0) ? 0 : totalItens;
            objTitleCnt.text((totalItens - 1));

            $(".tl-cnt-default-block[codrecado=" + objBtn.attr("codrecado") + "]").fadeOut(300, function () {
                $(this).remove();

                if ($(".view-page .tl-cnt-default .tl-cnt-default-block").length == 0) {
                    $(".view-page").html("");
                    $(".view-pager").html("");
                    $(".view-btntop").html("");
                }
            });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirRecadoComentario(objBtn) {

        var objContainerLoading = $(".cmt-cnt[codcoment=" + objBtn.attr("codcoment") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.RecadoComentarioId = objBtn.attr("codcoment");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirRecadoComentario";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".cmt-cnt[codcoment=" + objBtn.attr("codcoment") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirMultiplosRecados(objBtn) {

        if ($(".tl-checkbox:checked").length == 0) {
            fnNotificacao("warning", "Nenhum recado selecionado.", V_TIME_MESSAGE);
            return false;
        }

        var arrayItens = new Array();

        $(".tl-checkbox:checked").each(function () {

            var objeto = new Object();
            objeto.RecadoIdParaExcluir = $(this).val();

            arrayItens.push(objeto);

        });

        $(objBtn).buttonLoader('start');

        var dto = { "model": arrayItens };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirMultiplosRecados";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            $(objBtn).buttonLoader('stop');

            $(".tl-checkbox:checked").each(function () {
                $(this).closest(".tl-cnt-default-block").fadeOut(300, function () {
                    $(this).remove();

                    if ($(".view-page .tl-cnt-default .tl-cnt-default-block").length == 0) {
                        $(".view-page").html("");
                        $(".view-pager").html("");
                        $(".view-btntop").html("");
                    }
                });
            });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            $(objBtn).buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnRecadoExibirMaisComents(objBtn, recadoId, page) {

        $(objBtn).buttonLoader('start');

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getListRecadoComentarios";
        _ws.data = "{'recadoId':'" + recadoId + "','page':'" + page + "'}";
        _ws.ajaxWs(function (res) {

            $(objBtn).buttonLoader('stop');

            var response = res.d.Result.Result;

            objBtn.closest(".ed-cnt-cmt-more").remove();
            $(".tl-cnt-default-block[codrecado=" + recadoId + "] .ed-cnt-cmt-coments").append(response);
            $(".tl-cnt-default-block[codrecado=" + recadoId + "] .ed-cnt-cmt-coments .cmt-cnt").fadeIn("slow");

            $(".tooltip-dropmenu").tipsy({
                trigger: 'click',
                gravity: 'n',
                html: true,
                title: function () {
                    return $(this).find(".tooltip-dropmenu-html-container").html();
                }
            });

            $(".cmt-cnt-img-perfil-tooltip-big").tipsy({
                trigger: 'click',
                gravity: 's',
                html: true,
                title: function () {
                    return $(this).next().html();
                }
            });

        }, function (err) {
            $(objBtn).buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnRecadoExibirMenosComents(objBtn, recadoId, page) {

        var itemComent = 1,
            ultimo_codcoment = -1;

        $(".tl-cnt-default-block[codrecado=" + recadoId + "] .ed-cnt-cmt-coments .cmt-cnt").each(function () {

            if (itemComent > 4) {
                $(this).fadeOut(300, function () {
                    $(this).remove();
                });
            }
            else {
                ultimo_codcoment = $(this).attr("codcoment");
            }
            itemComent++;
        });

        itemComent--;

        var v_html = "<a href='javascript:void(0);' class='button minor1 btnVerMais' codrecado='" + recadoId + "' page='" + page + "'>Ver mais comentários</a>";
        objBtn.closest(".ed-cnt-cmt-more").html(v_html);
    }

    function fnBloquearPost(objBtn) {

        var objContainer = $(".tl-cnt-default-block[codrecado=" + objBtn.attr("codrecado") + "]");
        objContainer.buttonLoader('start');

        var model = new Object();
        model.UsuarioIdBloqueado = objBtn.attr("codperfil");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "BloquearPost";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainer.buttonLoader('stop');
            //objContainer.fadeOut(300, function () {
            //    $(this).remove();
            //});

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainer.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getPerfilRecadosView";
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

            $(".view-btntop").html(view[4]);

            $(".view-page .tl-cnt-default .tl-cnt-default-block").fadeIn("slow");

            $(".view-editor").html(view[3]);
            $(".view-editor .ed-cnt-cmt").editorHtml();

            $(".ed-cnt-cmt[bloqueado=0]").editorHtml();

            $(".cmt-cnt-img-perfil-tooltip-big").tipsy({
                trigger: 'click',
                gravity: 's',
                html: true,
                title: function () {
                    return $(this).next().html();
                }
            });

            fnInicializaViewPerfil();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }
});