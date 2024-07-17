function isNotNull(o) {
    return (o != '' && o != null && o != 'undefined')
}

function parseSeller(b) {
    var t = 'STANDARD';
    if (b == true) { t = 'SELLER' };
    return t;
}

function AjaxPost(query, postData, callBack) {
    $.ajax({
        url: query,
        type: 'POST',
        data: createQuery(postData),
        cache: false,
        processData: false,
        success: function (response) { callBack(response.Success, response.Data); },
        error: function (response) {  callBack(false, response.Data); }
    });
}

function executeQuery(query, postData, onComplete) {
    AjaxPost(query, postData, function (success, data) { onComplete(success, data); })
}

function createQuery(queryStrings) {
    var qb = new QueryBuilder();
    $.each(queryStrings, function (key, value) {
        qb.addEntry(new keyValuePair(key, value));
    })
    return qb.Build();
}

function combineQuery(base, queryStrings) {
    return base + '?' + createQuery(queryStrings);
}