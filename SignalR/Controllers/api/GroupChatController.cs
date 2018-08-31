using Microsoft.AspNet.Identity;
using SignalR.Entities;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SignalR.Controllers.api
{
    [Authorize]
    public class GroupChatController : ApiController
    {
        private readonly ApplicationDbContext _context;
        public GroupChatController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public IHttpActionResult CheckExistConversation(string conName)
        {          
            try
            {
                var loginUserId = User.Identity.GetUserId();
                var conversation = _context.Conversations.FirstOrDefault(c => c.Name == conName);
                if (conversation!=null)
                {
                    var model = ConversationModel.Create(_context, conversation.Id);
                    return Json(model);
                }
                var newConName = String.Join("_",conName.Split('_').Reverse());
                var newConversation = _context.Conversations.FirstOrDefault(c => c.Name == newConName);
                if (newConversation != null)
                {
                    var model = ConversationModel.Create(_context, newConversation.Id);
                    return Json(model);
                }
                return Json(false);

            }
            catch (Exception ex)
            {               
                return Json("error");
            }
        }

        [HttpGet]
        public IHttpActionResult GetOldMessagesByConversationId(string conId)
        {
            try
            {
                var loginUserId = User.Identity.GetUserId();
                var conversation = _context.Conversations.FirstOrDefault(c => c.Id == conId);
                if (conversation != null)
                {
                    var model = new ConversationModel();
                    model = ConversationModel.Create(_context, conversation.Id);
                    return Json(model);
                }               
                return Json(false);
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        [HttpGet]
        public IHttpActionResult CreateGroup(string name)
        {
            try
            {
                var loginUserId = User.Identity.GetUserId();
                var newCon = new Conversation()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedBy = loginUserId,
                    Name = name,
                    StartTime = DateTime.Now,
                    IsGroup = true
                };
                var savedCon=_context.Conversations.Add(newCon);
                var userCon = new UserConversation()
                {
                    ConversationId = savedCon.Id,
                    UserId = loginUserId,
                    JoinedTime = DateTime.Now
                };
                var savedUserCon = _context.UserConversations.Add(userCon);
                _context.SaveChanges();
                return Json(savedCon.Id);
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

    }
    public class ConversationModel
    {
        public ConversationModel()
        {
            Messages = new List<MessageModel>();
        }
        public string ConversationId { get; set; }
        public List<MessageModel> Messages { get; set; }
        public static ConversationModel Create(ApplicationDbContext _context, string conversationId )
        {
            var model = new ConversationModel();
            model.ConversationId = conversationId;
            model.Messages = _context.Messages.Where(m => m.ConversationId == conversationId).OrderByDescending(m => m.SendTime).Take(100).ToList().Select(m =>
            {
                var message = new MessageModel();
                message.Id = m.Id;
                message.Content = m.Content;
                message.SentUserId = m.SentUserId;
                message.SentUserName = _context.Users.FirstOrDefault(u => u.Id == m.SentUserId)?.UserName;
                return message;
            }).Reverse().ToList();
            return model;
        }
    }
    public class MessageModel
    {
        public string Id { get; set; }
        public string SentUserId { get; set; }
        public string SentUserName { get; set; }
        public string Content { get; set; }
    }
}
