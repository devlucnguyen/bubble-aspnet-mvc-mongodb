$(document).ready(function () {
    // Declare a proxy to reference the hub.
    var bubbleHub = $.connection.bubbleHub;
    registerClientMethods(bubbleHub);

    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(bubbleHub);
    });
});

function registerClientMethods(bubbleHub) {
    // Calls when user successfully logged in
    bubbleHub.client.onConnected = function (id, userName) {
        $('.userid').val(id);
        $('.username').val(userName);
        getFriendOnline();
    }

    // On New User Connected
    bubbleHub.client.onNewUserConnected = function () {
        getFriendOnline();
    }

    // On User Disconnected
    bubbleHub.client.onUserDisconnected = function (userid) {
        setFriendOffline(userid);
    }
}

function registerEvents(bubbleHub) {
    if (userId)
        bubbleHub.server.connect(userId);
}

function getFriendOnline() {
    $.ajax({
        url: 'Account/GetFriendOnline',
        type: 'post',
        success: function (response) {
            var friends = JSON.parse(response).user;

            for (var i = 0; i < friends.length; ++i) {
                var status = $('.friend-list li[data-userid="' + friends[i] + '"]').find('.avatar');

                if (status)
                    $(status).removeClass('avatar-state-secondary').addClass('avatar-state-success');
            }
        }
    });
}

function setFriendOffline(userid) {
    var status = $('.friend-list li[data-userid="' + userid + '"]').find('.avatar');

    if (status)
        $(status).removeClass('avatar-state-success').addClass('avatar-state-secondary');
}