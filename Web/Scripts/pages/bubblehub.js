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
    bubbleHub.client.onNewUserConnected = function (id, name, loginDate) {
        AddUser(bubbleHub, id, name, loginDate);
    }
}

function registerEvents(bubbleHub) {
    if (userId)
        bubbleHub.server.connect(userId);
}

function AddUser(id, name, date) {
    var code = "";

    code = $('<br/><span class="username">' + id + '  ' + '<a id="' + id + '" class="user" >' + name + '<a>' + '<span class="text-muted pull-right">' + date + '</span>  </span></div></div>');
    $("#divusers").append(code);
}

function getFriendOnline() {
    $.ajax({
        url: 'Account/GetFriendOnline',
        type: 'post',
        success: function (response) {
            var friends = JSON.parse(response).user;
        }
    });
}