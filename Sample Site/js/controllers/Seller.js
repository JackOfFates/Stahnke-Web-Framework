/// <reference path="jquery-1.11.2.min.js" />
/// <reference path="QueryBuilder.js" />
/// <reference path="Main.js" />

function SellerAuth() {

    var Seller = new Object();

    var handlerBase = '../Handlers/Seller.ashx'
    var updateQuery = combineQuery(handlerBase, { 't': 'Update' });

    function Update(bn, w, b, l, onComplete) {
        executeQuery(updateQuery,
        {
            'businessname': bn,
            'businesswebsite': w,
            'businessbiography': b,
            'businesslocation': l
        }, onComplete);
    }

    Seller.Update = Update;

    return Seller;
}