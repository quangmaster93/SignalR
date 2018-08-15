using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalR.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public Conversation Conversation { get; set; }
        public string SentUserId { get; set; }
        public DateTime SendTime { get; set; }

    }
}