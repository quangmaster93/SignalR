using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.Entities
{
    public class UserConversation
    {
        public string UserId { get; set; }
        public string ConversationId { get; set; }
        public DateTime JoinedTime { get; set; }
    }
}