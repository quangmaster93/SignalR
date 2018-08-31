using Microsoft.AspNet.Identity;
using SignalR.Entities;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalR.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Shape()
        {
            return View();
        }

        public ActionResult StockTicker()
        {
            return View();
        }

        public ActionResult Index()
        {
            var loginUserId = User.Identity.GetUserId();
            var context = new ApplicationDbContext();
            var model = new HomeIndexViewModel();
            foreach (var item in context.Users)
            {
                if (item.Id!= loginUserId)
                {
                    var user = new UserModel()
                    {
                        UserId = item.Id,
                        UserName = item.UserName.Substring(0, item.UserName.IndexOf('@'))
                    };
                    model.UserModels.Add(user);
                }               
            }

            var userCons = context.UserConversations.Where(u => u.UserId == loginUserId).Select(u => u.ConversationId);
            var groups = context.Conversations.Where(c => c.IsGroup == true && userCons.Contains(c.Id));
            foreach (var item in groups)
            {
                    var group = new GroupModel()
                    {
                        Id= item.Id,
                        Name=item.Name
                    };
                    model.GroupModels.Add(group);
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}