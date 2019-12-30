using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Hubs
{
    [HubName("bubbleHub")]
    public class BubbleHub : Hub
    {
        static List<ConnectedUser> ConnectedUsers = new List<ConnectedUser>();

        public void Connect(string accountId)
        {
            var id = Context.ConnectionId;

            if (ConnectedUsers.Count(user => user.ConnectionId == id) == 0)
            {
                var logintime = DateTime.Now.ToString();
                var user = new ConnectedUser { ConnectionId = id, AccountId = accountId, LoginTime = logintime };
                var currentUser = ConnectedUsers.Where(connectedUser => connectedUser.AccountId.Equals(accountId)).FirstOrDefault();

                if (currentUser == null)
                    ConnectedUsers.Add(user);
                else ConnectedUsers[ConnectedUsers.IndexOf(currentUser)].ConnectionId = id;

                // send to caller
                Clients.Caller.onConnected();

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected();
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectedUser = ConnectedUsers.FirstOrDefault(user => user.ConnectionId == Context.ConnectionId);

            if (connectedUser != null)
            {
                ConnectedUsers.Remove(connectedUser);
                Clients.All.onUserDisconnected(connectedUser.AccountId);
            }

            return base.OnDisconnected(stopCalled);
        }

        public static List<ConnectedUser> GetAllConnectedUser()
        {
            return ConnectedUsers;
        }

        //Function send message if both user online
        public void SendMessage(string toUserId, string message)
        {
            var fromUserId = Context.ConnectionId;
            var toUser = ConnectedUsers.FirstOrDefault(user => user.AccountId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(user => user.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                var currentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                Clients.Client(toUser.ConnectionId).sendMessage(fromUser.AccountId, message, currentDateTime);
            }
        }

        public void OnTyping(string toUserId)
        {
            var fromUserId = Context.ConnectionId;
            var toUser = ConnectedUsers.FirstOrDefault(user => user.AccountId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(user => user.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
                Clients.Client(toUser.ConnectionId).onTyping(fromUser.AccountId);
        }

        public void OnStopTyping(string toUserId)
        {
            var fromUserId = Context.ConnectionId;
            var toUser = ConnectedUsers.FirstOrDefault(user => user.AccountId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(user => user.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
                Clients.Client(toUser.ConnectionId).onStopTyping(fromUser.AccountId);
        }
    }
}