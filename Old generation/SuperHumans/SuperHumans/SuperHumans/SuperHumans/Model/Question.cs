using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Model
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Owner { get; set; }
        public string TimeAgo { get; set; }

        //public ICollection<Answer> Answers { get; set; }
    }
}
