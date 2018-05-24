using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Models
{
    public class Conversation
    {
        public string ObjectId { get; set; }
        public User OtherUser { get; set; }
        public UserMessage LastMessage { get; set; }
    }
}
