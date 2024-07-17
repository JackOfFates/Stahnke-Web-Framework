/// <reference path="jquery-1.11.2.min.js" />
/// <reference path="QueryBuilder.js" />
/// <reference path="Main.js" />

function isNotNull(o) {
    return (o != '' && o != null && o != 'undefined')
}

function AjaxPost(query, postData, Callback) {
    $.ajax({
        url: query,
        type: 'POST',
        data: createQuery(postData),
        cache: false,
        processData: false,
        contentType: 'application/json',
        jsonpCallback: Callback
    });
}


function executeQuery(query, postData, Callback) {
    return AjaxPost(query, postData, Callback);
}

function createQuery(queryStrings) {
    try {
        var qb = new QueryBuilder();
        $.each(queryStrings, function (key, value) {
            qb.addEntry(new keyValuePair(key, value));
        })
        return qb.Build();
    } catch (e) {
        return ""
    }
}

function combineQuery(base, queryStrings) {
    return base + '?' + createQuery(queryStrings);
}

function performTask(items, numToProcess, processItem) {
    var pos = 0;

    function iteration() {
        var j = Math.min(pos + numToProcess, items.length);
        for (var i = pos; i < j; i++) { processItem(items, i); }
        pos += numToProcess;
        if (pos < items.length)
            setTimeout(iteration, 10);
    }

    iteration();

}