using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Model
{
    public class Answer
    {
        public int AnswerID { get; set; }

        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public string TimeAgo { get; set; }

        //FK
        //public Question Question { get; set; }
        public int QuestionID { get; set; }
    }
}
