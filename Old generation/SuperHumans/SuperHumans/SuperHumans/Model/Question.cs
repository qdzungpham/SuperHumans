using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Model
{
    public class Question
    {
        public string ObjectId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Owner { get; set; }
        public string TimeAgo { get; set; }
    }
}
