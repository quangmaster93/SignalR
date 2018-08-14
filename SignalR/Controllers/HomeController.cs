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
            var context = new ApplicationDbContext();
            var model = new List<UserModel>();
            foreach (var item in context.Users)
            {
                var user = new UserModel() {
                    UserId=item.Id,
                    UserName=item.UserName
                };
                model.Add(user);
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