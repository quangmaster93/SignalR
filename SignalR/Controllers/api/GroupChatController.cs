using Microsoft.AspNet.Identity;
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
                    return Json(conversation.Id);
                }
                var newConName = String.Join("_",conName.Split('_').Reverse());
                var newConversation = _context.Conversations.FirstOrDefault(c => c.Name == newConName);
                if (newConversation != null)
                {
                    return Json(newConversation.Id);
                }

                return Json(false);

            }
            catch (Exception ex)
            {               
                return Json("error");
            }
        }
    }
}
