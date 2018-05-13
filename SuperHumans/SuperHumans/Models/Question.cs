

using System;
using System.Collections.Generic;

namespace SuperHumans.Models
{
    public class Question
    {
        public string ObjectId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public User Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TimeAgo { get; set; }
        public List<string> Topics { get; set; }
        public bool IsFollowed { get; set; }
        public string State { get; set; }
    }
}
