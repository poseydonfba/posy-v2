
$(document).ready(function () {

    /* Google */
    startGoogleApp();

    /* LinkedIN */
    $(document).on("click", "#btnLinkedin", function () {
        onLinkedInLoad();
    });

    /* Facebook */
//    FB.init({
//        appId: 'Your_Application_ID',
//        status: true, 
//        cookie: false, 
//        xfbml: true
//    }); 
//    FB.api("/me",
//        function (response) {
//            alert('Name is ' + response.name);
//        }
//    );

});

function updateStatusCallback(data){

    alert(JSON.stringify(data));

}

/* Google */
var googleUser = {};
var startGoogleApp = function() {
    gapi.load('auth2', function(){
        auth2 = gapi.auth2.init({
            client_id: '156193423069-n857vans5a5cqkdu2c22p2880mmdllbh.apps.googleusercontent.com',
            cookiepolicy: 'single_host_origin',
            // Request scopes in addition to 'profile' and 'email'
            //scope: 'additional_scope'
        });
        attachSignin(document.getElementById('btnGoogle'));
    });
};
function attachSignin(element) {
    auth2.attachClickHandler(element, {},

        function(googleUser) {
            var id_token = googleUser.getAuthResponse().id_token;
            var profile = googleUser.getBasicProfile();

            var dados = 'ID: ' + profile.getId() +
                        '\n\nNOME: ' + profile.getName() +
                        '\n\nIMAGE URL: ' + profile.getImageUrl() +
                        '\n\nEMAIL: ' + profile.getEmail() +
                        '\n\nID TOKEN: ' + id_token;

            alert(dados);
            //https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=id_token
        }, 
        
        function(error) {
            alert(JSON.stringify(error, undefined, 2));
        }
    );
}

/* Linkedin */

function onLinkedInLoad() {
    IN.UI.Authorize().place();
    IN.Event.on(IN, "auth", getProfileData);
}

function onLinkedInSuccess(profiles) {
    var member = profiles.values[0];
    var id = member.id;
    var firstName = member.firstName;
    var lastName = member.lastName;
    var photo = member.pictureUrl;
    var headline = member.headline;
    var email = member.emailAddress;
    var local = member.location.name;
    var publicProfileUrl = member.publicProfileUrl;

    alert("MEMBER: " + member +
        "\n\nID: " + id +
        "\n\nFIRST NAME: " + firstName +
        "\n\nLAST NAME: " + lastName +
        "\n\nPHOTO: " + photo +
        "\n\nHEADLINE: " + headline +
        "\n\nEMAIL: " + email +
        "\n\nLOCAL: " + local +
        "\n\nPUBLIC PROFILE URL: " + publicProfileUrl);
}

function onLinkedInError(error) {
}

function getProfileData() {
    IN.API.Profile("me")
        .fields("id",
                "firstName",
                "lastName",
                "industry",
                "location:(name)",
                "picture-url",
                "headline",
                "summary",
                "num-connections",
                "public-profile-url",
                "distance",
                "positions",
                "email-address",
                "educations",
                "date-of-birth")
        .result(onLinkedInSuccess)
        .error(onLinkedInError);
}