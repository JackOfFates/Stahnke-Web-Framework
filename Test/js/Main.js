/// <reference path="jquery-1.11.2.min.js" />
/// <reference path="QueryBuilder.js" />
/// <reference path="Main.js" />

function isNotNull(o) {
    return (o != '' && o != null && o != 'undefined')
}

function AjaxPost(query, postData) {
    return $.ajax({
        url: query,
        type: 'POST',
        data: createQuery(postData),
        cache: false,
        processData: false,
    });
}

function executeQuery(query, postData) {
    return AjaxPost(query, postData)
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