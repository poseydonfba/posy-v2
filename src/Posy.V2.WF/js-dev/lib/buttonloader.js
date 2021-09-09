/*A jQuery plugin which add loading indicators into buttons
* By Minoli Perera
* MIT Licensed.
*/
(function ($) {
    $.fn.buttonLoader = function (action) {
        var self = $(this);

        if (action == 'start') {
            if ($(self).attr("disabled") == "disabled") {
                e.preventDefault();
            }

            $(self).attr('data-btn-text', $(self).html());
            $(self).attr('data-btn-class', $(self).attr("class"));
            
            var classes = $(self).attr("class").split(' ');

            for (i = 0; i < classes.length; i++) {
                if (classes[i] != "button" && classes[i] != "minor1" && classes[i] != "icon" && classes[i].substring(0, 3) == "btn") {
                    $(self).removeClass(classes[i]);
                }
            }

            var preloader = '<div class="preloader_1">' +
                                '<span></span>' +
                                '<span></span>' +
                                '<span></span>' +
                                '<span></span>' +
                                '<span></span>' +
                            '</div>';
            $(self).html($(self).html() + preloader);
            $(self).addClass('active');
        }

        if (action == 'stop') {
            $(self).html($(self).attr('data-btn-text'));
            $(self).attr("class", $(self).attr('data-btn-class'));
            $(self).removeClass('active');
        }
    }
})(jQuery);

(function ($) {
    $.fn.boxLoader = function (options) {
        var settings = $.extend({
            param: 'defaultValue'
        }, options || {});

        this.stop = function () {

            $(this).find(".preloader_2").fadeOut(300, function () {
                $(this).remove();
            });
        };

        return this.each(function () {
            var element = $(this);

            var preloader = '<div class="preloader_2">' +
                                '<span></span>' +
                                '<span></span>' +
                                '<span></span>' +
                                '<span></span>' +
                                '<span></span>' +
                            '</div>';

            element.append(preloader);

        });
    };
})(jQuery);