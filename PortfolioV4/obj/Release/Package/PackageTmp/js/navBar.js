$(document).ready(function () {
    var url = window.location.pathname;
    var substr = url.split('/');
    var urlaspx = substr[substr.length - 1];
    if (urlaspx === "") { urlaspx = 'index';}
    //$('.nav').find('.active').removeClass('active');
    $('.nav li a').each(function () {
        if (this.href.indexOf(urlaspx) >= 0) {
            $(this).parent().addClass('active');
        }
        else { $(this).parent().removeClass('active');}
    });
});