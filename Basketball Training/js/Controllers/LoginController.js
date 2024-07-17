/// <reference path="../jquery-1.11.2.min.js" />
/// <reference path="../QueryBuilder.js" />
/// <reference path="../Main.js" />

function autoGen_LoginController() {
    var LoginControllerObj = new Object();
    var Callback = function (data, type) { }
    var handlerBase = '../Handlers/LoginController.ashx';

    var Do_loginQuery = combineQuery(handlerBase, { 't': 'Do_login' });
    var Do_registerQuery = combineQuery(handlerBase, { 't': 'Do_register' });
    var Get_sessionQuery = combineQuery(handlerBase, { 't': 'Get_session' });

    function Do_login(Username,Password) {
        return executeQuery(Do_loginQuery, 
            {
                'Username': Username,
                'Password': Password
            }, Callback);
    }

    function Do_register(Username,Password,Email,ProfileType) {
        return executeQuery(Do_registerQuery, 
            {
                'Username': Username,
                'Password': Password,
                'Email': Email,
                'ProfileType': ProfileType
            }, Callback);
    }

    function Get_session() {
        return executeQuery(Get_sessionQuery,  null);
    }

    LoginControllerObj.Do_login = Do_login;
    LoginControllerObj.Do_register = Do_register;
    LoginControllerObj.Get_session = Get_session;
    LoginControllerObj.Callback = Callback
    return LoginControllerObj;
}

    var LoginController = new autoGen_LoginController();