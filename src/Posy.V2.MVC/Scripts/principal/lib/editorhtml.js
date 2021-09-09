(function ($) {
    $.fn.editorHtml = function (settings) {
        var config = {
            'buttonsBottom': true,
            'autoHeight': true
        };
        if (settings) { $.extend(config, settings); }

        return this.each(function () {

            var dataAtual = new Date();
            var idcmt = dataAtual.getTime() + "_" + dataAtual.getHours() + "_" + dataAtual.getMinutes() + "_" + dataAtual.getSeconds() + "_" + dataAtual.getMilliseconds();

            var v_blocoforecolor = "<div class='ed-cnt-forecolor " + idcmt + "'>" +
                                      "<a class='forecolor' cor='#ac725e' style='background:#ac725e;'></a>" +
                                      "<a class='forecolor' cor='#d06b64' style='background:#d06b64;'></a>" +
                                      "<a class='forecolor' cor='#f83a22' style='background:#f83a22;'></a>" +
                                      "<a class='forecolor' cor='#fa573c' style='background:#fa573c;'></a>" +
                                      "<a class='forecolor' cor='#ff7537' style='background:#ff7537;'></a>" +
                                      "<a class='forecolor' cor='#ffad46' style='background:#ffad46;'></a>" +
                                      "<a class='forecolor' cor='#42d692' style='background:#42d692;'></a>" +
                                      "<a class='forecolor' cor='#16a765' style='background:#16a765;'></a>" +
                                      "<a class='forecolor' cor='#7bd148' style='background:#7bd148;'></a>" +
                                      "<a class='forecolor' cor='#b3dc6c' style='background:#b3dc6c;'></a>" +
                                      "<a class='forecolor' cor='#fbe983' style='background:#fbe983;'></a>" +
                                      "<a class='forecolor' cor='#fad165' style='background:#fad165;'></a>" +
                                      "<a class='forecolor' cor='#92e1c0' style='background:#92e1c0;'></a>" +
                                      "<a class='forecolor' cor='#9fe1e7' style='background:#9fe1e7;'></a>" +
                                      "<a class='forecolor' cor='#9fc6e7' style='background:#9fc6e7;'></a>" +
                                      "<a class='forecolor' cor='#4986e7' style='background:#4986e7;'></a>" +
                                      "<a class='forecolor' cor='#9a9cff' style='background:#9a9cff;'></a>" +
                                      "<a class='forecolor' cor='#b99aff' style='background:#b99aff;'></a>" +
                                      "<a class='forecolor' cor='#c2c2c2' style='background:#c2c2c2;'></a>" +
                                      "<a class='forecolor' cor='#cabdbf' style='background:#cabdbf;'></a>" +
                                      "<a class='forecolor' cor='#cca6ac' style='background:#cca6ac;'></a>" +
                                      "<a class='forecolor' cor='#f691b2' style='background:#f691b2;'></a>" +
                                      "<a class='forecolor' cor='#cd74e6' style='background:#cd74e6;'></a>" +
                                      "<a class='forecolor' cor='#a47ae2' style='background:#a47ae2;'></a>" +
                                      "<a class='forecolor' cor='#000000' style='background:#000000;'></a>" +
                                      "<a class='forecolor' cor='#ffffff' style='background:#ffffff;'></a>" +
                                   "</div>";

            var perfixUrlEmot = "/Images/emoticon/";//ResolveUrl("~/img/emoticon/");

            var v_blocoEmoticon = "<div class='ed-cnt-emot " + idcmt + "'>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "act-up.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "airplane.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "alien.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "angel.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "angry.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "arrogant.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "bad.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "bashful.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "beat-up.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "beauty.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "beer.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "blowkiss.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "bomb.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "bowl.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "boy.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "brb.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "bye.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "cake.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "call-me.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "camera.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "can.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "car.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "cat.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "chicken.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "clap.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "clock.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "cloudy.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "clover.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "clown.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "coffee.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "coins.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "computer.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "confused.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "console.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "cow.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "cowboy.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "crying.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "curl-lip.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "curse.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "cute.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "dance.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "dazed.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "desire.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "devil.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "disapointed.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "disdain.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "doctor.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "dog.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "doh.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "dont-know.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "drink.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "drool.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "eat.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "embarrassed.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "excruciating.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "eyeroll.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "film.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "fingers-crossed.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "foot-in-mouth.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "freaked-out.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "ghost.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "giggle.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "girl.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "glasses-cool.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "glasses-nerdy.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "go-away.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "goat.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "good.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "hammer.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "handcuffs.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "handshake.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "highfive.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "hug-left.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "hug-right.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "hungry.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "hypnotized.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "in-love.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "island.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "jump.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "kiss.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "knife.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "lamp.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "lashes.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "laugh.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "liquor.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "love-over.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "love.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "lying.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "mad-tongue.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "mail.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "mean.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "meeting.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "mobile.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "moneymouth.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "monkey.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "moon.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "musical-note.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "nailbiting.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "neutral.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "party.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "peace.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "phone.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "pig.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "pill.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "pissed-off.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "pizza.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "plate.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "pray.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "present.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "pumpkin.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "qq.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "question.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "quiet.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "rain.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "rainbow.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "rose-dead.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "rose.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "rotfl.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sad.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sarcastic.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "search.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "secret.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "shame.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sheep.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "shock.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "shout.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "shut-mouth.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sick.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sigarette.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "silly.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "skeleton.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "skywalker.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sleepy.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "smile-big.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "smile.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "smirk.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "snail.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "snicker.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "snowman.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "soccerball.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "soldier.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "star.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "starving.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "struggle.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sun.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "sweat.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "teeth.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "terror.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "thinking.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "thunder.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "tongue.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "tremble.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "turtle.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "tv.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "umbrella.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "vampire.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "victory.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "waiting.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "watermelon.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "weep.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "wilt.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "wink.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "worship.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "yawn.png' /></a>" +
                                    "<a class='emot'><img class='emotimg' src='" + perfixUrlEmot + "yin-yang.png' /></a>" +
                                "</div>";

            var style_btn = " style='display:none;' ";
            if (config.buttonsBottom == true) {
                style_btn = "";
            }

            var v_editorHtml = '' +
                                '<div class="new-com-bt">' +
                                    '<span>Escreva um comentário ...</span>' +
                                '</div>' +
                                '<div class="new-com-cnt">' +
                                    '<!--textarea class="the-new-com"></textarea-->' +
                                    '<iframe frameborder="1" marginheight="5" marginwidth="5" scrolling="auto" class="ed-area-iframe the-new-com"></iframe>' +
                                '<div class="box-iframe-editor">' +
                                '</div>' +
                                '<div class="ed-cnt-cmt-btn-editor">' +
                                    '<div class="ed-cnt-html">' +
                                        '<ul>' +
                                            '<li active="0" class="ed-item-bold"></li>' +
                                            '<li active="0" class="ed-item-italic"></li>' +
                                            '<li active="0" class="ed-item-underline"></li>' +
                                            '<li active="0" class="ed-item-forecolor cmt-cnt-img-perfil-tooltip-big" title="' + v_blocoforecolor + '"></li>' +
                                            '<li active="0" class="ed-item-emoticon cmt-cnt-img-perfil-tooltip-big" title="' + v_blocoEmoticon + '"></li>' +
                                            '<li active="0" class="ed-item-html"></li>' +
                                        '</ul>' +
                                    '</div>' +
                                    '<div class="button-group" ' + style_btn + '>' +
                                        '<a href="javascript:void(0);" class="button big icon add primary bt-add-com btnAddPost">Postar</a>' +
                                        '<a href="javascript:void(0);" class="button big icon trash bt-cancel-com btnCancelPost">Cancelar</a>' +
                                    '</div>' +
                                '</div>' +
                                '</div>' +
                                '<div class="clear"></div>';

            var objContainer = $(this);

            if (objContainer.attr("ativo") === undefined) {

                objContainer.attr("ativo", "1");
                objContainer.append(v_editorHtml);

                objContainer.find(".ed-cnt-html .cmt-cnt-img-perfil-tooltip-big").tipsy({
                    trigger: 'click',
                    gravity: 'n',
                    html: true
                });

                var objIframe = objContainer.find("iframe.the-new-com"),
                    objBtnNewCmt = objContainer.find('.new-com-bt'),
                    objBtnCancelCmt = objContainer.find('.btnCancelPost'),
                    objBtnAddCmt = objContainer.find('.btnAddPost'),
                    objNewCmtCnt = objContainer.find('.new-com-cnt');

                var limparEditor = function () {
                    alert("ok");
                }

                /* quando clica no textbox */

                objBtnNewCmt.click(function () {

                    $(this).hide();

                    _editor = objIframe[0].contentWindow.document;
                    _editor.designMode = 'on';

                    try {
                        _editor.execCommand("styleWithCSS", 0, false);
                    } catch (e) {
                        try {
                            _editor.execCommand("useCSS", 0, true);
                        } catch (e) {
                            try {
                                _editor.execCommand('styleWithCSS', false, false);
                            }
                            catch (e) {
                            }
                        }
                    }

                    objContainer.find('textarea.the-new-com').remove();
                    objNewCmtCnt.show();
                    objIframe.css({ "height": "100px" });
                    objIframe.show().focus();

                    var styleIframe = 'font-family:Verdana;' +
                        'font-size: 13px;' +
                        'font-size: 0.8125rem;' +
                        'line-height: 1.6;' +
                        'word-wrap: break-word;' +
	                    'text-align: -webkit-auto;' +
	                    'white-space:normal';

                    objIframe.contents().find("head").html($("<style type='text/css'>  body{" + styleIframe + "}  </style>"));
                    objIframe.contents().find('body').html('');

                    return false;
                });

                /* quando se esta escrevendo ativa ou desativa o botao de post, transparencia */

                objContainer.find('textarea').on("keyup", function () {

                });
                $("body", objIframe.contents()).on('keyup', function (e) {
                    $('.tipsy:last').remove();
                    objBtnAddCmt.css({ opacity: 0.6 });
                    var checklength = $(this).html().length;
                    if (checklength) {
                        objBtnAddCmt.css({ opacity: 1 });
                    }

                    if (config.autoHeight) {
                        var $iframe = objContainer.find("iframe.the-new-com");
                        $iframe.css('height', ($iframe.contents().find('body').height()) + 'px');
                    }
                });
                objContainer.on('change keyup keydown paste cut', 'textarea', function () {
                    $('.tipsy:last').remove();
                    objBtnAddCmt.css({ opacity: 0.6 });
                    var checklength = $(this).val().length;
                    if (checklength) {
                        objBtnAddCmt.css({ opacity: 1 });
                    }

                    if (config.autoHeight)
                        $(this).height(0).height(this.scrollHeight);

                }).find('textarea').change();

                if (config.autoHeight) {
                    /* auto height iframe  */
                    setTimeout(function () {
                        var $iframe = objContainer.find("iframe.the-new-com");
                        $iframe.css('height', ($iframe.contents().find('body').height()) + 'px');
                    }, 500);
                } else {
                    var $iframe = objContainer.find("iframe.the-new-com");
                    $iframe.addClass("ed-height");
                }

                /* quando clica no botao cancelar */
                objBtnCancelCmt.click(function () {
                    $('.tipsy:last').remove();
                    objContainer.find('textarea.the-new-com').val('');
                    objIframe.contents().find('body').html('');
                    objNewCmtCnt.fadeOut('fast', function () {
                        objBtnNewCmt.fadeIn('fast');
                    });
                    return false;
                });

                var objBtnBold = objContainer.find(".ed-item-bold"),
                    objBtnItalic = objContainer.find(".ed-item-italic"),
                    objBtnUnderline = objContainer.find(".ed-item-underline"),
                    objBtnEmoticon = objContainer.find(".ed-item-emoticon"),
                    objBtnHtml = objContainer.find(".ed-item-html"),
                    objBtnForeColor = objContainer.find(".ed-item-forecolor");

                objBtnBold.click(function () {

                    $('.tipsy:last').remove();

                    if ($(this).attr("ativo") == "0") return false;

                    var _editor = objIframe[0].contentWindow;
                    _editor.document.execCommand('bold', false, null);
                    _editor.focus();

                    if (!$(this).closest("li").hasClass("ed-item-selected"))
                        $(this).closest("li").addClass("ed-item-selected");
                    else
                        $(this).closest("li").removeClass("ed-item-selected");

                    return false;
                });

                objBtnItalic.click(function () {

                    $('.tipsy:last').remove();

                    if ($(this).attr("ativo") == "0") return false;

                    var _editor = objIframe[0].contentWindow;
                    _editor.document.execCommand('italic', false, null);
                    _editor.focus();

                    if (!$(this).closest("li").hasClass("ed-item-selected"))
                        $(this).closest("li").addClass("ed-item-selected");
                    else
                        $(this).closest("li").removeClass("ed-item-selected");

                    return false;
                });

                objBtnUnderline.click(function () {

                    $('.tipsy:last').remove();

                    if ($(this).attr("ativo") == "0") return false;

                    var _editor = objIframe[0].contentWindow;
                    _editor.document.execCommand('Underline', false, null);
                    _editor.focus();

                    if (!$(this).closest("li").hasClass("ed-item-selected"))
                        $(this).closest("li").addClass("ed-item-selected");
                    else
                        $(this).closest("li").removeClass("ed-item-selected");

                    return false;
                });

                $(document).on("click", "." + idcmt + " .emot", function () {

                    $('.tipsy:last').remove();

                    if ($(this).attr("ativo") == "0") return false;

                    var _editor = objContainer.find("iframe.the-new-com")[0].contentWindow;
                    _editor.document.execCommand('insertimage', false, $(this).find("img").attr("src"));
                    objBtnAddCmt.css({ opacity: 1 });
                    $('.tipsy:last').remove();
                    _editor.focus();
                    return false;
                });

                $(document).on("click", "." + idcmt + " .forecolor", function () {

                    $('.tipsy:last').remove();

                    if ($(this).attr("ativo") == "0") return false;

                    var _editor = objContainer.find("iframe.the-new-com")[0].contentWindow;
                    _editor.document.execCommand('ForeColor', false, $(this).attr("cor"));
                    objBtnAddCmt.css({ opacity: 1 });
                    $('.tipsy:last').remove();
                    _editor.focus();
                    return false;
                });

                objBtnHtml.click(function () {

                    $('.tipsy:last').remove();

                    if ($(this).attr("active") == "0") {

                        /* Habilita textarea */

                        var _html = objIframe.contents().find("body").html(),
                        _textArea = '<textarea class="ed-area-textarea the-new-com">' + _html + '</textarea>';

                        objIframe.removeClass("the-new-com");
                        objIframe.hide();

                        objNewCmtCnt.find(".box-iframe-editor").html(_textArea);
                        objContainer.find(".ed-area-textarea").focus();

                        objBtnBold.attr("ativo", "0").addClass("ed-item-disabled");
                        objBtnItalic.attr("ativo", "0").addClass("ed-item-disabled");
                        objBtnUnderline.attr("ativo", "0").addClass("ed-item-disabled");
                        objBtnEmoticon.attr("ativo", "0").addClass("ed-item-disabled").removeClass("cmt-cnt-img-perfil-tooltip-big");
                        objBtnForeColor.attr("ativo", "0").addClass("ed-item-disabled").removeClass("cmt-cnt-img-perfil-tooltip-big");

                        $(this).attr("active", "1");

                        if (config.autoHeight)
                            objContainer.find(".ed-area-textarea").change();

                    } else {

                        /* Habilita iframe */

                        var objTextarea = objNewCmtCnt.find(".ed-area-textarea"),
                        _texto = objTextarea.val();

                        objIframe.addClass("the-new-com");
                        objIframe.show();
                        objTextarea.remove();
                        objIframe.contents().find("body").html(_texto).focus();

                        objBtnBold.attr("ativo", "1").removeClass("ed-item-disabled");
                        objBtnItalic.attr("ativo", "1").removeClass("ed-item-disabled");
                        objBtnUnderline.attr("ativo", "1").removeClass("ed-item-disabled");
                        objBtnEmoticon.attr("ativo", "1").removeClass("ed-item-disabled").addClass("cmt-cnt-img-perfil-tooltip-big");
                        objBtnForeColor.attr("ativo", "1").removeClass("ed-item-disabled").addClass("cmt-cnt-img-perfil-tooltip-big");

                        $(this).attr("active", "0");
                    }

                });

            }

        });
    };
})(jQuery);

function fnOcultarContainerEditor(objContainer) {
    $('.tipsy:last').remove();

    var objNewCmtCnt = objContainer.find('.new-com-cnt'),
        objBtnNewCmt = objContainer.find('.new-com-bt');

    objNewCmtCnt.fadeOut('fast', function () {
        objBtnNewCmt.fadeIn('fast');
    });
}