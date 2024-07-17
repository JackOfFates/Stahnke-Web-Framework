/// <reference path="jquery-1.11.2.min.js" />
/// <reference path="Main.js" />

function QueryBuilder() {

    var myBuilder = new Object();
    var Entries = [];

    function keyValueToQuery(newKey, newValue) {
        return encodeURIComponent(newKey) + '=' + encodeURIComponent(newValue)
    }

    function combine(input, keyValuePair) {
        return input + '&' + keyValuePair
    }

    function appendEntry(input, newKey, newValue) {
        var newEntry = keyValueToQuery(newKey, newValue)
        return combine(input, newEntry)
    }

    function addEntry(kv) {
        Entries.push(kv);
    }

    function addEntries(kv) {
        for (var i = 0; i < kv.length; i++) {
            addEntry(kv[i]);
        }
    }

    function Build() {
        var FinalString = '';
        for (var i = 0; i < Entries.length; i++) {
            var Entry = Entries[i];
            if (FinalString == '') {
                FinalString = keyValueToQuery(Entry.key, Entry.value);
            } else {
                FinalString = appendEntry(FinalString, Entry.key, Entry.value);
            }
        }
        return FinalString;
    }

    myBuilder.addEntry = addEntry;
    myBuilder.addEntries = addEntries;
    myBuilder.Build = Build;

    return myBuilder;
}

function keyValuePair(key, value) {
    var ValuePair = new Object();

    ValuePair.key = key;
    ValuePair.value = value;

    return ValuePair;
}