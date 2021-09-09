$(document).ready(function () {

    $(".ed-cnt-cmt").editorHtml();

    fnView();

    $(document).on("click", ".btnMenuItem", function () {

        if ($(this).attr("opcao") == "EV")
            fnExcluirVideo($(this));

        else if ($(this).attr("opcao") == "B")
            fnBloquearPost($(this));

        else if ($(this).attr("opcao") == "EC")
            fnExcluirVideoComentario($(this));

    });

    $(document).on("click", ".btnVerMais", function () {
        fnVideoExibirMaisComents($(this), $(this).attr("codvideo"), $(this).attr("page"));
    });

    $(document).on("click", ".btnVerMenos", function () {
        fnVideoExibirMenosComents($(this), $(this).attr("codvideo"), 2);
    });

    $(document).on("click", ".btnSalvarVideo", function () {

        if (fnVerificaCampos("#containerForm") != "n") {
            fnNotificacao("warning", "Nenhuma url informada.", V_TIME_MESSAGE);
            $("html, body").animate({ scrollTop: 0 }, "slow");
            $(".txtUrlVideo").focus();
            return false;
        }

        var objContainer = $(this);
        objContainer.buttonLoader('start');

        var url = 'https://www.youtube.com/watch?v=' + $(".txtUrlVideo").val().split('=')[1];

        $.getJSON('https://noembed.com/embed',
        {
            format: 'json', url: url
        }, function (data) {
            objContainer.buttonLoader('stop');
            fnSalvar(data, objContainer);
        });

        return false;
    });

    $(document).on("click", ".ed-cnt-cmt .btnAddPost", function () {
        fnVideoComentario($(this).closest(".ed-cnt-cmt"));
    });

    $(document).on("click", ".album-video-container img.gl-zoom-img", function () {

        var videoid = $(this).attr("videoid"),
            videotitle = $(this).attr("videotitle"),
            codvideo = $(this).attr("codvideo"),
            urlvideo = "https://www.youtube.com/embed/" + videoid + "?autoplay=1";

        $(".poseydon-video-view").attr("codvideo", codvideo);
        $(".poseydon-video-view .image-view-foto iframe").attr("src", urlvideo);
        $(".poseydon-video-view .video-caption h3").html(videotitle);
        $(".poseydon-video-view").fadeIn("slow", function () { });
        $(".ed-cnt-cmt .ed-cnt-cmt-coments").html($(this).closest(".video-container").find(".ed-cnt-cmt-coments-temp").html()).hide().fadeIn("slow");

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

        $("html, body").animate({ scrollTop: 0 }, "slow");

        return false;
    });

    $(document).on("click", ".poseydon-video-view img", function () {
        $(".poseydon-video-view").fadeOut("slow", function () { });
    });

    function fnSalvar(data, objBtn) {

        var objContainerLoading = objBtn;
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.Url = $(".txtUrlVideo").val().split('=')[1];
        model.Titulo = data.title;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarVideo";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var response = res.d.Result.Result;

            $(response).hide().prependTo(".view-page .album-video-container").fadeIn("slow");
            $(".txtUrlVideo").val("");

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
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirVideo(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-container[codvideo=" + objBtn.attr("codvideo") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.VideoId = objBtn.attr("codvideo");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirVideo";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".video-container[codvideo=" + objBtn.attr("codvideo") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            $(".poseydon-video-view .image-view-foto iframe").attr("src", "");
            $(".poseydon-video-view").fadeOut("slow", function () { });

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnVideoComentario(editorPerfil) {

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
        model.VideoId = $(".poseydon-video-view").attr("codvideo");
        model.Comentario = _htmlPost;

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "SalvarVideoComentario";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            var response = res.d.Result.Result;

            $(response).hide().prependTo($(".ed-cnt-cmt .ed-cnt-cmt-coments")).fadeIn("slow");
            $(response).hide().prependTo($(".video-container[codvideo=" + $(".poseydon-video-view").attr("codvideo") + "] .ed-cnt-cmt-coments-temp")).fadeIn("slow");

            $(".tooltip-dropmenu").tipsy({
                trigger: 'click',
                gravity: 'n',
                html: true,
                title: function () {
                    return $(this).find(".tooltip-dropmenu-html-container").html();
                }
            });

            fnOcultarContainerEditor(editorPerfil);

            setTimeout(function () {
                $(".postado-recente").removeClass("postado-recente");
            }, 500);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnExcluirVideoComentario(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-comentario-container[codcoment=" + objBtn.attr("codcoment") + "]");
        objContainerLoading.buttonLoader('start');

        var model = new Object();
        model.VideoComentarioId = objBtn.attr("codcoment");

        var dto = { "model": model };

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "ExcluirVideoComentario";
        _ws.data = JSON.stringify(dto);
        _ws.ajaxWs(function (res) {

            objContainerLoading.buttonLoader('stop');

            $(".cmt-cnt[codcoment=" + objBtn.attr("codcoment") + "]").fadeOut(300, function () {
                $(this).remove();
            });

            $(".video-container[codvideo=" + $(".poseydon-video-view").attr("codvideo") + "] .ed-cnt-cmt-coments-temp .cmt-cnt[codcoment=" + objBtn.attr("codcoment") + "]").remove();

            fnNotificacao("success", "Sucesso na operação.", V_TIME_MESSAGE);

        }, function (err) {
            objContainerLoading.buttonLoader('stop');
            fnErrorGlobal(err);
        });
    }

    function fnVideoExibirMaisComents(objBtn, videoId, page) {

        $(objBtn).buttonLoader('start');

        _ws.clean();
        _ws.service = "UsuarioWebService";
        _ws.metodo = "getListVideoComentarios";
        _ws.data = "{'videoId':'" + videoId + "','page':'" + page + "'}";
        _ws.ajaxWs(function (res) {

            $(objBtn).buttonLoader('stop');

            var response = res.d.Result.Result;

            objBtn.closest(".ed-cnt-cmt-more").remove();

            $(".ed-cnt-cmt .ed-cnt-cmt-coments").append(response);
            $(".ed-cnt-cmt .ed-cnt-cmt-coments .cmt-cnt").fadeIn("slow");

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

    function fnVideoExibirMenosComents(objBtn, videoId, page) {

        var itemComent = 1,
            ultimo_codcoment = -1;

        $(".ed-cnt-cmt .ed-cnt-cmt-coments .cmt-cnt").each(function () {

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

        var v_html = "<a href='javascript:void(0);' class='button minor1 btnVerMais' codvideo='" + videoId + "' page='" + page + "'>Ver mais comentários</a>";
        objBtn.closest(".ed-cnt-cmt-more").html(v_html);
    }

    function fnBloquearPost(objBtn) {

        var objContainerLoading = $(".tooltip-dropmenu-comentario-container[codcoment=" + objBtn.attr("codcoment") + "]");
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
        _ws.metodo = "getPerfilVideosView";
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

            $(".view-form").html(view[3]);

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