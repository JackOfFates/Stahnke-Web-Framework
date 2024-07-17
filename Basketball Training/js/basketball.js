/// <reference path="../js/Controllers/LoginController.js" />
/// <reference path="../js/jquery-1.11.2.min.js" />
/// <reference path="../js/QueryBuilder.js" />
/// <reference path="../js/jquery.shadow.js" />
/// <reference path="../js/jquery.json.js" />
/// <reference path="../js/Main.js" />

var $SFT;
var PhoneModeTheshhold;
var PhoneModeEnabled = false;

LoginController.Callback = cbFunc;

$(document).ready(function () {

    RevalidateWidth();

    if ($.cookie('SFT') != null) {
        // Session Exists
        LoginFromCookie();
    } else if ($('#Username').val() != '') {
        // Username Saved
        if ($('#Password').val() == '') {
            Login();
        } else {
            // Select Password
            $('#Password').focus();
        }
    } else if ($('#Username').val() == '') {
        $('#Username').focus();
    }
    $(window).resize(function () {
        RevalidateWidth();
    })

    // Bindings
    $('#LoginButton').click(function () {

    })
    $('#Username').keypress(UsernameKeyPress)
    $('#Password').keypress(PasswordKeyPress)
})

function cbFunc(data, status) {
    alert(data);
}

function RevalidateWidth() {
    var docWidth = $(document).width()

    if (docWidth < 768) {
        PhoneModeEnabled = true;
        $('#tContent').css('max-width', '180px');
    } else {
        PhoneModeEnabled = false;
        $('#tContent').css('max-width', '100%');
        $('#tContent').width('auto');
    }
}

function Login() {
    doLogin($('#Username').val(), $('#Password').val());
    $SFT = readCookie();
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function UsernameKeyPress(e) {
    if (e.which == 13 || e.keyCode == 13) {
        $('#Password').focus();
    }
}

function PasswordKeyPress(e) {
    if (e.which == 13 || e.keyCode == 13) {
        // Enter Key
        Login();
    }
}

function LoginFromCookie() {
    $SFT = readCookie();
    loggedInView($SFT.SessionUser);
    setUsernameLink();
}

function Player(isGuardian) {
    $('div#RegistrationContainer ul:first-child').hide()
    $('div#RegistrationContainer').addClass('isRegister');
    $('div#bg').addClass('blur');
    $('#RegitrationInformation').addClass('shadow');
    $('#RegitrationInformation').show();
    if (isGuardian) {

    }

}

function Trainer(isGuardian) {
    $('div#RegistrationContainer ul:first-child').hide()
    $('div#bg').addClass('blur');
    $('#RegitrationInformation').addClass('shadow');
    $('div#RegistrationContainer').addClass('isRegister');
}

function ClickRegister() {
    var u = $('').val();
    var p = $('').val();
    var e = $('').val();

    LoginController.Do_register(u, p, e, pt);
}

function setUsernameLink() {
    $('#UsernameLink').attr('href', 'javascript:void(0);');
    $('#UsernameLink').click(ShowProfile);
}

function ShowProfile() {
    alert(JSON.stringify($SFT));
}

function SignOut() {
    $.removeCookie('SFT');
    location.reload();
}

function loggedInView(u) {
    $('#weclomeLabel').text('Welcome ');
    $('#UsernameLink').text(u);
    $('#LoginForm').html('( <a href="javascript:void(0);" onclick="SignOut();">Sign-Out</a> )').css('float', 'right');
    $('div#RegistrationContainer').css('display', 'none');
}

function readCookie() {
    return $.parseJSON($.cookie('SFT'));
}

function doRegister(isTrainer, u, p, e) {
    return LoginController.Do_register(u, p, e, isTrainer ^ 0);
}

function doLogin(u, p) {
    LoginController.Do_login(u, p);
}