using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace SignalR.Hubs
{
    [Authorize]
    public class GroupChatHub : Hub
    {
        static List<Connection> connection = new List<Connection>();
        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var userId = Context.User.Identity.GetUserId();
            connection.Add(new Connection()
            {
                UserId = userId,
                ConnectionId = connectionId
            });
            var onlineUserId = connection.Select(c => c.UserId);
            Clients.All.getOnlineUsers(onlineUserId);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            var removedConnection = connection.FirstOrDefault(c => c.ConnectionId == connectionId);
            connection.Remove(removedConnection);
            var onlineUserId = connection.Select(c => c.UserId);
            Clients.All.getOnlineUsers(onlineUserId);
            return base.OnDisconnected(stopCalled);
        }
    }
    public class Connection{
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}