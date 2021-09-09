var chatHub = chatHub || {};

$(function () {
    chatHub = $.connection.chatHub;
    $.connection.hub.start();

    var chatWin = $("#chatWindow");

    // Remap para diminuir o tamanho do tráfego de mensagens
    var userContract = {
        n: "nome",
        i: "id",
        c: "connectionId",
        f: "frase"
    };

    //fnView();
    fnCount();

    chatHub.client.addChatMessageRecebida = function (nome, msg, time) {

        var html = '' +
            '<li class="other animated fadeInLeft">' +
            '<div class="msg">' +
            '<div class="user">' + nome + '</div>' +
            '<p>' + msg + '</p>' +
            '<time>' + time + '</time>' +
            '</div>' +
            '</li>';

        chatWin.append(html);
        chatWin.animate({ scrollTop: chatWin[0].scrollHeight }, 'slow');
    };

    chatHub.client.addChatMessageEnviada = function (msg, time) {

        var html = '' +
            '<li class="self animated fadeInRight">' +
            '<div class="msg">' +
            '<p>' + msg + '</p>' +
            '<time>' + time + '</time>' +
            '</div>' +
            '</li>';

        chatWin.append(html);
        chatWin.animate({ scrollTop: chatWin[0].scrollHeight }, 'slow');
    };

    chatHub.client.userOnline = function (user) {

        var userModel = reMap(user, userContract);

        console.log(userModel);

        $(".mb-attribution[cp='" + userModel.id + "']").remove();

        var v_html = '' +
            '<div class="mb-attribution animated slideInUp" cp="' + userModel.id + '" ci="' + userModel.connectionId + '">' +
            '<p class="mb-author">' + userModel.name + '</p>' +
            '<cite>' + userModel.frase + '</cite>' +
            '<div class="mb-thumb" style="background-image: url(/Images/perfil/' + userModel.id + '/1.jpg);"></div>' +
            '<div class="mb-notification"><span></span>12:00</div>' +
            '</div>';
        $(".chatmembers").prepend(v_html);

        fnCount();
    };

    chatHub.client.setOnlineUsers = function (onlineUsers) {
        var v_html = '';
        for (var i = 0; i < onlineUsers.length; i++) {
            var userModel = reMap(onlineUsers[i], userContract);
            v_html += '' +
                '<div class="mb-attribution animated slideInUp" cp="' + userModel.id + '" ci="' + userModel.connectionId + '">' +
                '<p class="mb-author">' + userModel.name + '</p>' +
                '<cite>' + userModel.frase + '</cite>' +
                '<div class="mb-thumb" style="background-image: url(/Images/perfil/' + userModel.id + '/1.jpg);"></div>' +
                '<div class="mb-notification"><span></span>12:00</div>' +
                '</div>';
        }
        $(".chatmembers").html(v_html);

        fnCount();
    };

    $(".send").click(function () {
        fnEnviarMensagem();
    });

    $("#mensagem").keypress(function (e) {
        if (e.which === 13)
            fnEnviarMensagem();
    });

    function fnCount() {
        var total = $(".chatmembers .mb-attribution").length;
        $(".count li").html(total);

        if (total === 0)
            $(".count").hide();
        else
            $(".count").show();
    }

    function fnEnviarMensagem() {
        if ($("#mensagem").val() === "") return;

        chatHub.server.broadcast(chatWin.attr("ci"), $("#mensagem").val());

        $("#mensagem").val("");
    }

    $(document).on("click", ".mb-attribution", function () {

        $(".mb-hover").removeClass("mb-hover");
        $(".mb-thumb").css("border-left-color", "rgba(213,215,200,1)");

        var id = $(this).attr("cp"),
            ci = $(this).attr("ci"),
            name = $(this).find(".mb-author").html(),
            src = "Images/perfil/" + id + "/1.jpg";

        chatWin.attr("cp", id);
        chatWin.attr("ci", ci);

        $(this).addClass("mb-hover");
        $(this).find(".mb-thumb").css("border-left-color", "#336699");

        $(".menu .name").html(name);
        $(".menu img").attr("src", src);

        $("#mensagem").focus();
    });

    function fnView() {
        var objLoader = $(".mv-loading").boxLoader();

        $.get("/ajax/chat/membros/template/view", function (response) {

            $(".chatmembers").html(response);

            //fnAnimationMembers();

        }).always(function () {
            objLoader.stop();
        });
    }

    function fnAnimationMembers() {

        $('.mb-attribution').each(function (idx) {
            idx = (idx) * 0.1;
            $(this).css({
                "-webkit-animation-delay": idx + "s",
                "-moz-animation-delay": idx + "s",
                "-ms-animation-delay": idx + "s",
                "-o-animation-delay": idx + "s",
                "animation-delay": idx + "s"
            });
            $(this).addClass("slideInUp");
        });
    }

    //$('.mb-attribution1').each(function (i) {
    //    var t = $(this);
    //    setTimeout(
    //        function () {
    //            //t.addClass('animated');
    //            t.addClass('slideInUp');
    //            t.show();
    //        },
    //        (i + 1) * 100
    //    );
    //});

    var cartWrapper = $('.cd-cart-container');

    if (cartWrapper.length > 0) {
        //store jQuery objects
        var cartBody = cartWrapper.find('.body')
        var cartTrigger = cartWrapper.children('.cd-cart-trigger');

        //open/close cart
        cartTrigger.on('click', function (event) {
            event.preventDefault();
            toggleCart();
        });

        //close cart when clicking on the .cd-cart-container::before (bg layer)
        //cartWrapper.on('click', function (event) {
        //    if ($(event.target).is($(this))) toggleCart(true);
        //});
    }

    function toggleCart(bool) {
        var cartIsOpen = (typeof bool === 'undefined') ? cartWrapper.hasClass('cart-open') : bool;
        if (cartIsOpen) {
            cartWrapper.removeClass('cart-open');
        } else {
            cartWrapper.addClass('cart-open');
            fnAnimationMembers();
        }
    }

});