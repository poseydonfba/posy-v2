/**
* This javascript file checks for the brower/browser tab action.
* It is based on the file menstioned by Daniel Melo.
* Reference: http://stackoverflow.com/questions/1921941/close-kill-the-session-when-the-browser-or-tab-is-closed
*/
var validNavigation = false;

//document.addEventListener('click', function (e) {
//    alert(e.target.tagName);
//}, false);

function wireUpEvents() {
    /**
    * For a list of events that triggers onbeforeunload on IE
    * check http://msdn.microsoft.com/en-us/library/ms536907(VS.85).aspx
    *
    * onbeforeunload for IE and chrome
    * check http://stackoverflow.com/questions/1802930/setting-onbeforeunload-on-body-element-in-chrome-and-ie-using-jquery
    */
    var dont_confirm_leave = 0; //set dont_confirm_leave to 1 when you want the user to be able to leave withou confirmation
    var leave_message = 'Deseja sair do sistema?';

    function goodbye(e) {

        if (window.event) {
            if (window.event.clientX < 40 && window.event.clientY < 0) {
                //alert("Browser back button is clicked...");
                validNavigation = true;
            }
            else if ((window.event.clientX < 100 && window.event.clientY < 0) ||
                     (window.event.clientX > 100 && window.event.clientY < 0)) {
                //alert("Browser refresh button is clicked...");
                validNavigation = true;
            }
        }
        else {
            if (event.currentTarget.performance.navigation.type == 1) {
                //alert("Browser refresh button is clicked...");
                validNavigation = true;
            }
            else if (event.currentTarget.performance.navigation.type == 2) {
                //alert("Browser back button is clicked...");
                validNavigation = true;
            }
        }

        if (!validNavigation) {     

            if (dont_confirm_leave !== 1) {
                if (!e) e = window.event;
                //e.cancelBubble is supported by IE - this will kill the bubbling process.
                e.cancelBubble = true;
                e.returnValue = leave_message;
                //e.stopPropagation works in Firefox.
                if (e.stopPropagation) {
                    e.stopPropagation();
                    e.preventDefault();
                }
                //return works for Chrome and Safari
                //return leave_message;
                return "a = " + JSON.stringify(e);
            }
        }
    }

    //function disableF5(e) { if ((e.which || e.keyCode) == 116) e.preventDefault(); };
    //$(document).on("keydown", disableF5);

//    $(document).on('mousemove', function (e) {
//        document.title = window.event.clientX + " - " + window.event.clientY;
    //    });

    //window.onbeforeunload = goodbye;
    
    // Attach the event keypress to exclude the F5 refresh
    $(document).on('keydown', function (e) {
        if ((e.which || e.keyCode) == 116) {
            validNavigation = true;
        }
        else if ((e.which || e.keyCode) == 8) {
            validNavigation = true;
        }
        else if ((e.which || e.keyCode) == 65 && e.ctrlKey) {
            validNavigation = true;
        }
        else if ((e.which || e.keyCode) == 82 && e.ctrlKey) {
            validNavigation = true;
        }
    });

    // Attach the event click for all links in the page
    $("a").on("click", function () {
        validNavigation = true;
    });

    // Attach the event submit for all forms in the page
    $("form").on("submit", function () {
        validNavigation = true;
    });

    // Attach the event click for all inputs in the page
    $("input[type=submit]").on("click", function () {
        validNavigation = true;
    });

}

// Wire up the events as soon as the DOM tree is ready
$(document).ready(function () {
    wireUpEvents();
});