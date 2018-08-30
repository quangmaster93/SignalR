using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using SignalR.Entities;
using SignalR.Models;

namespace SignalR.Hubs
{
    [Authorize]
    public class GroupChatHub : Hub
    {
        public readonly ApplicationDbContext _context;
        public GroupChatHub()
        {
            _context = new ApplicationDbContext();
        }
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

            var listConversationIds = _context.UserConversations.Where(u => u.UserId == userId).Select(u => u.ConversationId).ToList();
            foreach (var item in listConversationIds)
            {
                Groups.Add(connectionId, item);
            }
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

        public async Task SendMessage(string userIdPar, string message, string conId)
        {
            try
            {          
            var loginUserId= Context.User.Identity.GetUserId();
            var isInit = _context.Conversations.Any(c=>c.Id == conId);
            if (!isInit)
            {
                var con = new Conversation()
                {
                    Id = conId,
                    Name = $"{loginUserId}_{userIdPar}",
                    StartTime = DateTime.Now,
                    CreatedBy= loginUserId,
                };
                var savedCon= _context.Conversations.Add(con);
                var mes = new Message()
                {
                    Id= Guid.NewGuid().ToString(),
                    Content=message,
                    ConversationId=savedCon.Id,
                    SendTime= DateTime.Now,
                    SentUserId= loginUserId
                };
                var savedMes = _context.Messages.Add(mes);
                var savedUserConversation = _context.UserConversations.AddRange(new List<UserConversation>(){ new UserConversation() {
                    ConversationId=savedCon.Id,
                    UserId=loginUserId,
                    JoinedTime=DateTime.Now
                },
                new UserConversation() {
                    ConversationId=savedCon.Id,
                    UserId=userIdPar,
                    JoinedTime=DateTime.Now
                }
                });
                await _context.SaveChangesAsync();
                await Groups.Add(Context.ConnectionId, savedCon.Id);
                var connectionIdOfUserPar = connection.FirstOrDefault(c=>c.UserId==userIdPar)?.ConnectionId;
                if (connectionIdOfUserPar!=null)
                {
                    await Groups.Add(connectionIdOfUserPar, savedCon.Id);
                }
                var sentUserName = _context.Users.FirstOrDefault(u => u.Id == loginUserId)?.UserName;
                var messageModel = new MessageModel()
                {
                    MessageId = savedMes.Id,
                    ConversationId = savedCon.Id,
                    Content = savedMes.Content,
                    SentUserId = savedMes.SentUserId,
                    SentUserName = sentUserName ?? "noname",
                };
                Clients.OthersInGroup(savedCon.Id).getMessage(messageModel);
            }
            else
            {
                    var mes = new Message()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = message,
                        ConversationId = conId,
                        SendTime = DateTime.Now,
                        SentUserId = loginUserId
                    };
                    var savedMes = _context.Messages.Add(mes);
                    await _context.SaveChangesAsync();
                    var sentUserName = _context.Users.FirstOrDefault(u => u.Id == loginUserId)?.UserName;
                    var messageModel = new MessageModel()
                    {
                        MessageId = savedMes.Id,
                        ConversationId = conId,
                        Content = savedMes.Content,
                        SentUserId = savedMes.SentUserId,
                        SentUserName = sentUserName ?? "noname",
                    };
                    Clients.OthersInGroup(conId).getMessage(messageModel);
                                
            }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
    public class Connection{
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }

    public class MessageModel
    {
        public string MessageId { get; set; }
        public string ConversationId { get; set; }
        public string SentUserId { get; set; }
        public string Content { get; set; }
        public string SentUserName { get; set; }
    }

}