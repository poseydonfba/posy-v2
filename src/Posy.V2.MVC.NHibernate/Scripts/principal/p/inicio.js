$(document).ready(function () {

    $("#liInicio").addClass("mn-horiz-active");

    fnViewMaster();
    fnView();

    $(document).on("click", ".ed-cnt-cmt .btnAddPost", function () {

        if ($(this).closest(".ed-cnt-cmt").attr("id") == "cmtInicioPessoal")
            fnPost($(this).closest(".ed-cnt-cmt"));
        else
            fnPostComentario($(this).closest(".ed-cnt-cmt"));

    });

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "E")
            fnExcluirPost($(this));

        else if ($(this).attr("opcao") == "O")
            fnOcultarPost($(this));

        else if ($(this).attr("opcao") == "B")
            fnBloquearPost($(this));

        else if ($(this).attr("opcao") == "EC")
            fnExcluirPostComentario($(this));

    });

    $(document).on("click", ".btnExcluirPostCheck", function () {
        fnExcluirMultiplosPost($(this));
    });

    $(document).on("click", ".btnVerMaisPost", function () {
        fnPostExibirMaisPosts($(this));
    });

    $(document).on("click", ".btnVerMais", function () {
        fnPostExibirMaisComents($(this), $(this).attr("postId"), $(this).attr("page"));
    });

    $(document).on("click", ".btnVerMenos", function () {
        fnPostExibirMenosComents($(this), $(this).attr("postId"), 2);
    });


    // SignalR
    var conn = connector;
    conn.client.AtualizarFeeds = function (feed) {
        $(".view-page .tl-cnt-default").prepend(feed).hide().fadeIn("slow");
    }


    function fnPost(editorPerfil) {

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
        model.PostHtml = _htmlPost;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarPost";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var response = res.d.Result.Result;

            if ($(".view-page .tl-cnt-default").length == 0) {
                $(".view-page").html("<section class='tl-cnt-default'></section>");
                $(".view-btntop").html(response[1]);
            }

            $(response[0]).hide().prependTo(".view-page .tl-cnt-default").fadeIn("slow");
            $(".view-page .tl-cnt-default").find(".ed-cnt-cmt:eq(0)").editorHtml();

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

            // SignalR
            connector.server.informarFeed(response[0]);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnPostComentario(editorPerfil) {

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
        model.PostId = editorPerfil.closest(".tl-cnt-default-block").attr("postId");
        model.Comentario = _htmlPost;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarPostComentario";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            fnOcultarContainerEditor(editorPerfil);

            var response = res.d.Result.Result;

            $(response).hide().prependTo(editorPerfil.find(".ed-cnt-cmt-coments")).fadeIn("slow");

            editorPerfil.find(".ed-cnt-cmt-coments").find(".tooltip-dropmenu:eq(0)").tipsy({
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

    function fnExcluirPost(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[postId=" + objBtn.attr("postId") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.PostId = objBtn.attr("postId");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirPost";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".tl-cnt-default-block[postId=" + objBtn.attr("postId") + "]").fadeOut(300, function () {
                $(this).remove();

                if ($(".view-page .tl-cnt-default .tl-cnt-default-block").length == 0) {
                    $(".view-btntop").html("");
                    $(".view-page").html("");
                }
            });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirMultiplosPost(objBtn) {

        if ($(".tl-checkbox:checked").length == 0) {
            fnNotificacao("warning", "Nenhum post selecionado.", V_TIME_MESSAGE);
            return false;
        }

        var arrayItens = new Array();

        $(".tl-checkbox:checked").each(function () {

            var model = new Object();
            model.PostId = $(this).val();

            arrayItens.push(model);

        });

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var dto = { "model": arrayItens };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirMultiplosPost";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".tl-checkbox:checked").each(function () {
                $(this).closest(".tl-cnt-default-block").fadeOut(300, function () {
                    $(this).remove();

                    if ($(".view-page .tl-cnt-default .tl-cnt-default-block").length == 0) {
                        $(".view-btntop").html("");
                        $(".view-page").html("");
                    }
                });
            });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirPostComentario(objBtn) {

        var objContainerLoading = $(".cmt-cnt[codcoment=" + objBtn.attr("codcoment") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.ComentarioId = objBtn.attr("codcoment");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirPostComentario";
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

    function fnOcultarPost(objBtn) {

        $(objBtn).buttonLoader('start');

        var model = new Object();
        model.PostId = objBtn.attr("postId");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "OcultarPost";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            $(objBtn).buttonLoader('stop');

            $(".tl-cnt-default-block[postId=" + objBtn.attr("postId") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            $(objBtn).buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnBloquearPost(objBtn) {

        var objContainer = $(".tl-cnt-default-block[codperfil=" + objBtn.attr("codperfil") + "]");

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

    function fnPostExibirMaisPosts(objBtn) {

        if (objBtn.attr("ativo") == "0") return false;

        $(objBtn).buttonLoader('start');

        var page = objBtn.attr("page");

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getListInicioAtualizacao";
        _ws.data = "{'page':'" + page + "'}";
        _ws.ajaxWs(function (res) {

            $(objBtn).buttonLoader('stop');

            var response = res.d.Result;
            var view = response.Result;

            $(".view-page .tl-cnt-default").append(view[1]);
            $(".view-page .tl-cnt-default .tl-cnt-default-block").fadeIn("slow");

            $(".ed-cnt-cmt[bloqueado=0]").editorHtml();

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

            //var scrollTop = $(".tl-cnt-default-block[codpost=" + codpostultimo + "]").offset().top + $(".tl-cnt-default-block[codpost=" + codpostultimo + "]").height();
            //$('html, body').animate({
            //    scrollTop: (scrollTop - 50)
            //}, 1000);

            $(".view-btnvermais").html(view[4]);

        }, function (err) {
            $(objBtn).buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnPostExibirMaisComents(objBtn, postId, page) {

        $(objBtn).buttonLoader('start');

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getListPostComentarios";
        _ws.data = "{'postId':'" + postId + "','page':'" + page + "'}";
        _ws.ajaxWs(function (res) {

            $(objBtn).buttonLoader('stop');

            var response = res.d.Result.Result;

            objBtn.closest(".ed-cnt-cmt-more").remove();
            $(".tl-cnt-default-block[postId=" + postId + "] .ed-cnt-cmt-coments").append(response);
            $(".tl-cnt-default-block[postId=" + postId + "] .ed-cnt-cmt-coments .cmt-cnt").fadeIn("slow");

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

    function fnPostExibirMenosComents(objBtn, postId, page) {

        var itemComent = 1,
            ultimo_codcoment = -1;

        $(".tl-cnt-default-block[postId=" + postId + "] .ed-cnt-cmt-coments .cmt-cnt").each(function () {

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

        var v_html = "<a href='javascript:void(0);' class='button minor1 btnVerMais' postId='" + postId + "' page='" + page + "'>Ver mais comentários</a>";
        objBtn.closest(".ed-cnt-cmt-more").html(v_html);
    }

    function fnViewMaster() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getInicioView";
        _ws.type = "GET";
        _ws.ajaxWs(function (res) {

            objLoader.stop();

            var response = res.d.Result.Result;

            var perfil = response[0],
                amigos = response[1],
                cmm = response[2],
                menuVertical = response[3],
                visitantes = response[4];

            fnShowViewPerfil(perfil, amigos, cmm, menuVertical, visitantes);

            fnInicializaViewPerfil();

        }, function (err) {
            objLoader.stop();
            fnErrorGlobal(err);
        });
    }

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getListInicioAtualizacao";
        _ws.data = "{'page':'1'}";
        _ws.ajaxWs(function (res) {

            objLoader.stop();

            var response = res.d.Result.Result;

            var view = response;

            fnShowViewPerfilData(view);

            $(".view-editor").html(view[3]);
            $(".ed-cnt-cmt").editorHtml();
            $(".view-page .tl-cnt-default .tl-cnt-default-block").fadeIn("slow");
            $(".ed-cnt-cmt[bloqueado=0]").editorHtml();
            $(".view-btntop").html(view[4]);
            $(".view-btnvermais").html(view[5]);

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


/*

    function fnView() {

        var objLoader = $(".mv-loading").boxLoader();

        fnLimparViewPerfil();

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getInicioView";
        _ws.data = "{'page':'1'}";
        //_ws.type = "GET";
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

            $(".view-editor").html(view[3]);
            $(".ed-cnt-cmt").editorHtml();

            $(".view-page .tl-cnt-default .tl-cnt-default-block").fadeIn("slow");

            $(".ed-cnt-cmt[bloqueado=0]").editorHtml();

            $(".view-btntop").html(view[4]);
            $(".view-btnvermais").html(view[5]);

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

*/