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
                Clients.Caller.onConnected(id, accountId);

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
    }
}