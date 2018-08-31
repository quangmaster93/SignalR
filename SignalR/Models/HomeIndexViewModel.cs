using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.Models
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            UserModels = new List<UserModel>();
            GroupModels = new List<GroupModel>();
        }
        public List<UserModel> UserModels { get; set; }
        public List<GroupModel> GroupModels { get; set; }
    }
}