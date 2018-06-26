function toast(msg) {
    setTimeout(function () {
        document.getElementsByClassName('Jtoast-wrap')[0].getElementsByClassName('Jtoast-msg')[0].innerHTML = msg;
        var toastTag = document.getElementsByClassName('Jtoast-wrap')[0];
        toastTag.className = toastTag.className.replace('JtoastAnimate', '');
        setTimeout(function () {
            toastTag.className = toastTag.className + ' JtoastAnimate';
        }, 100);
    }, 100);
}