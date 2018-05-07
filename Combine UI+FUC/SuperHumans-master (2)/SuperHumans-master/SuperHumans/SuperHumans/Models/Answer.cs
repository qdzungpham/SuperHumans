using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Models
{
    public class Answer
    {
        public string ObjectId { get; set; }
        public string QuestionId { get; set; }
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public string TimeAgo { get; set; }
    }
}
