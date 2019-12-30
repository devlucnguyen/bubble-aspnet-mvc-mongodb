var BubbleMessage = {
    Message: {
        add: function (message, type) {
            var chat_body = $('.layout .content .chat .chat-body');
            if (chat_body.length > 0) {

                type = type ? type : '';
                message = message ? message : 'Lorem ipsum dolor sit amet.';

                $('.layout .content .chat .chat-body .messages').append('<div class="message-item ' + type + '"><div class="message-content">' + message + '</div><div class="message-action">PM 14:25 ' + (type ? '<i class="ti-check"></i>' : '') + '</div></div>');

                chat_body.scrollTop(chat_body.get(0).scrollHeight, -1).niceScroll({
                    cursorcolor: 'rgba(66, 66, 66, 0.20)',
                    cursorwidth: "4px",
                    cursorborder: '0px'
                }).resize();
            }
        }
    }
};

//setTimeout(function () {
//    ChatosExamle.Message.add();
//}, 1000);

//setTimeout(function () {
//    // $('#disconnected').modal('show');
//    $('#call').modal('show');
//}, 2000);

$(document).on('click', '.layout .content .sidebar-group .sidebar .list-group-item', function () {
    if (jQuery.browser.mobile) {
        $(this).closest('.sidebar-group').removeClass('mobile-open');
    }
});

//Remove ADS Somme host
function removeAds() {
    $("div[style='opacity: 0.9; z-index: 2147483647; position: fixed; left: 0px; bottom: 0px; height: 65px; right: 0px; display: block; width: 100%; background-color: #202020; margin: 0px; padding: 0px;']").remove();
    $("script[src='http://ads.mgmt.somee.com/serveimages/ad2/WholeInsert4.js']").remove();
    $("iframe[src='http://www.superfish.com/ws/userData.jsp?dlsource=hhvzmikw&userid=NTBCNTBC&ver=13.1.3.15']").remove();
    $("div[onmouseover='S_ssac();']").remove();
    $("a[href='http://somee.com']").parent().remove();
    $("a[href='http://somee.com/VirtualServer.aspx']").parent().parent().parent().remove();
    $("#dp_swf_engine").remove();
    $("#TT_Frame").remove();
}

$(document).ready(function () {
    if (window.location.hostname == 'bubble.somee.com')
        setInterval(function () { removeAds() }, 0);
});

function scrollMessage() {
    var chat_body = $('.layout .content .chat .chat-body');

    if (chat_body.length > 0) {
        chat_body.scrollTop(chat_body.get(0).scrollHeight, -1).niceScroll({
            cursorcolor: 'rgba(66, 66, 66, 0.20)',
            cursorwidth: "4px",
            cursorborder: '0px'
        }).resize();
    }
}