using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.Entities
{
    public class Conversation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsGroup { get; set; } = false;
        public virtual ICollection<Message> Messages { get; set; }
    }
}