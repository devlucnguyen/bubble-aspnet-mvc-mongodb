var bubbleHub = null, toFriendId = null, messageInput = null;

$(document).ready(function () {
    // Declare a proxy to reference the hub.
    bubbleHub = $.connection.bubbleHub;
    registerClientMethods(bubbleHub);

    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(bubbleHub);
    });
});

function registerClientMethods() {
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

    //Received Message
    bubbleHub.client.sendMessage = function (userid, message, datetime) {
        BubbleMessage.Message.add(message, '');
    }
}

function registerEvents() {
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

function openChat(friendId) {
    $.ajax({
        url: 'Home/GetConversation',
        type: 'post',
        data: {
            friendId: friendId
        },
        success: function (response) {
            var data = JSON.parse(response);

            if(data.type == 'success')
                $('.chat-body').html(data.result);

            $('.chat-header').removeClass('hide');
            $('.chat-footer').removeClass('hide');
            toFriendId = friendId;
        }
    });
}

function setHeaderChat(username) {
    setTimeout(function () {
        $('.friend-name-chat').text(username);
    }, 300);
}

//Send message event
$(document).on('submit', '.layout .content .chat .chat-footer form', function (e) {
    e.preventDefault();
    messageInput = $(this).find('input[type=text]');

    var message = messageInput.val();

    message = $.trim(message);

    if (message) sendMessage(message);
    else messageInput.focus();
});

function sendMessage(message) {
    $.ajax({
        url: 'Home/SendMessage',
        type: 'post',
        data: {
            toUserId: toFriendId,
            message: message
        },
        success: function (response) {
            var data = JSON.parse(response);

            if (data.type == 'success') {
                BubbleMessage.Message.add(message, 'outgoing-message');
                messageInput.val('');
                bubbleHub.server.sendMessage(toFriendId, message);
            }
        }
    });
}