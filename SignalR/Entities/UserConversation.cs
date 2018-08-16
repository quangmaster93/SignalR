using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalR.Entities
{
    public class UserConversation
    {
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        [Key, Column(Order = 2)]
        public string ConversationId { get; set; }
        public DateTime JoinedTime { get; set; }
    }
}