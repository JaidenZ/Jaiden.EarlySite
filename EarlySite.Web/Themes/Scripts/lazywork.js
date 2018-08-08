
onmessage = function (event) {
    
    var timeout = Math.floor(Math.random() * 10 + 1) * 10;
    var id = event.data;
    setTimeout(function () {
        postMessage(id);
    }, timeout);

    
}
