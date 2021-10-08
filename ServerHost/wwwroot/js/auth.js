const signin = $("#signin");
const signup = $("#signup");


$(function() {

    HashChecker();
    $(window).on("hashchange", HashChecker);

});

function HashChecker(){
    const hash = window.location.hash == "" ? "#signin" : window.location.hash;

    if (hash === "#signin") {
        signup.slideUp();
        signin.slideDown();
    }

    if (hash === "#signup") {
        signin.slideUp();
        signup.slideDown();
    }
}