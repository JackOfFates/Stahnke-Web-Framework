/// <reference path="jquery-1.11.2.min.js" />
/// <reference path="QueryBuilder.js" />
/// <reference path="Seller.js" />
/// <reference path="Main.js" />

function UserAuth() {

    var UserObj = new Object();

    var handlerBase = '../Handlers/User.ashx'
    var loginQuery = combineQuery(handlerBase, { 't': 'Login' });
    var logoutQuery = combineQuery(handlerBase, { 't': 'Logout' });
    var registerQuery = combineQuery(handlerBase, { 't': 'Register' });
    var updateQuery = combineQuery(handlerBase, { 't': 'Update' });

    function Login(u, p, onComplete) {
        executeQuery(loginQuery,
            {
                'username': u,
                'password': p
            }, onComplete);
    }

    function Logout() {
        executeQuery(logoutQuery, { 'nothing': 'true' }, function () { location.reload(); });
    }

    function RegisterSeller(u, p, fn, ln, e, b, c, s, bn, bw, bb, bl, onComplete) {
        var AccountType = parseSeller(s);
        if (s) {
            executeQuery(registerQuery,
            {
                'username': u,
                'password': p,
                'firstname': fn,
                'lastname': ln,
                'email': e,
                'biography': b,
                'accounttype': AccountType,
                'businessname': bn,
                'businesswebsite': bw,
                'businessbiography': bb,
                'businesslocation': bl
            }, onComplete);
        } else {
            executeQuery(registerQuery,
            {
                'username': u,
                'password': p,
                'firstname': fn,
                'lastname': ln,
                'email': e,
                'biography': b,
                'accounttype': AccountType
            }, onComplete);
        }

    }

    function RegisterStandard(u, p, fn, ln, e, b, c, onComplete) {
        executeQuery(registerQuery,
        {
            'username': u,
            'password': p,
            'firstname': fn,
            'lastname': ln,
            'email': e,
            'biography': b,
            'accounttype': parseSeller(false)
        }, onComplete);
    }

    function Update(p, fn, ln, e, b, s, c, onComplete) {
        executeQuery(updateQuery,
        {
            'password': p,
            'firstname': fn,
            'lastname': ln,
            'email': e,
            'biography': b,
            'accounttype': parseSeller(s),
            'contact': c
        }, onComplete);
    }

    UserObj.Login = Login;
    UserObj.Logout = Logout;
    UserObj.Update = Update;
    UserObj.RegisterSeller = RegisterSeller;
    UserObj.RegisterStandard = RegisterStandard;

    return UserObj;
}