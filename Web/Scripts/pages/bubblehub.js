var bubbleHub = null, toFriendId = null, messageInput = null;

$(document).ready(function () {
    // Declare a proxy to reference the hub.
    bubbleHub = $.connection.bubbleHub;
    registerClientMethods();

    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents();
    });

    //On typing event
    $('.input-message').keypress(delayTyping(function (toUserId) {
        bubbleHub.server.onStopTyping(toUserId);
    }));
});

function delayTyping(callback) {
    var timer = 0;
   
    return function () {
        var toUserId = toFriendId;
        clearTimeout(timer);
        bubbleHub.server.onTyping(toUserId);
        timer = setTimeout(function () {
            callback(toUserId);
        }, 500);
    };
}

function registerClientMethods() {
    // Calls when user successfully logged in
    bubbleHub.client.onConnected = function () {
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

    //On typing
    bubbleHub.client.onTyping = function (userid) {
        var statusHeader = $('.friend-status-chat[data-userid="' + userid + '"]');

        if (statusHeader)
            $(statusHeader).text("On typing...").removeClass('status-online');
    }

    //On stop typing
    bubbleHub.client.onStopTyping = function (userid) {
        var statusHeader = $('.friend-status-chat[data-userid="' + userid + '"]');

        if (statusHeader)
            $(statusHeader).text("Online").addClass('status-online');
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
                var statusFriend = $('.friend-list li[data-userid="' + friends[i] + '"]').find('.avatar');
                var statusConversation = $('.conversation-list li[data-userid="' + friends[i] + '"]').find('.avatar');
                var statusHeader = $('.friend-status-chat[data-userid="' + friends[i] + '"]');

                if (statusFriend)
                    $(statusFriend).removeClass('avatar-state-secondary').addClass('avatar-state-success');

                if (statusConversation)
                    $(statusConversation).removeClass('avatar-state-secondary').addClass('avatar-state-success');

                if (statusHeader)
                    $(statusHeader).text("Online").addClass('status-online');
            }
        }
    });
}

function setFriendOffline(userid) {
    var statusFriend = $('.friend-list li[data-userid="' + userid + '"]').find('.avatar');
    var statusConversation = $('.conversation-list li[data-userid="' + userid + '"]').find('.avatar');
    var statusHeader = $('.friend-status-chat[data-userid="' + userid + '"]');

    if (statusFriend)
        $(statusFriend).removeClass('avatar-state-success').addClass('avatar-state-secondary');

    if (statusConversation)
        $(statusConversation).removeClass('avatar-state-success').addClass('avatar-state-secondary');

    if (statusHeader)
        $(statusHeader).text("Offline").removeClass('status-online');
}

function openChat(friendId, fullName) {
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
            $('.friend-status-chat').attr('data-userid', friendId);

            //Update unread message
            var conversationUnread = $('.conversation-list li[data-userid="' + friendId + '"]').find('.new-message-count');

            if ($(conversationUnread).length > 0)
                updateUnreadMessage(friendId);

            setHeaderChat(fullName);
            toFriendId = friendId;
            scrollMessage();
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

function updateUnreadMessage(conversationId) {
    $.ajax({
        url: 'Home/UpdateUnreadMessage',
        type: 'post',
        data: {
            conversationId: conversationId
        },
        success: function () {
        }
    });
}