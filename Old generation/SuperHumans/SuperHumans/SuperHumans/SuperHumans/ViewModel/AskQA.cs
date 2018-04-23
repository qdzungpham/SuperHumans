using SuperHumans.Model;
using SuperHumans.ModelContext;
using SuperHumans.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SuperHumans.ViewModel
{
    public class AskQA : BaseViewModel
    {
        public Question _question;
        
        public AskQA()
        {
            Question = new Question();
        }

        public Question Question
        {
            get { return _question; }
            set
            {
                SetProperty(ref _question, value);
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }

        //private string _qHelper;
        //public string QuestionAsked
        //{
        //    get { return _qHelper; }
        //    set
        //    {
        //        SetProperty(ref _qHelper, value);
        //    }
        //}

        public Command AskCommand
        {
            get
            {
                return new Command(async (x) =>
                {
                    Message = Question.Body + " Asked!";

                    var authService = new AuthService();
                    Question.Owner = authService.UserName;  // Application.Current.Properties["UserName"].ToString();

                    Question.Title = "Who am I";
                    Question.TimeAgo = "Now";                    
                    var QC = new QuestionContext();
                    await QC.CreateQuestion(Question, "QuestionsWebAPI");
                    MessagingCenter.Send(this, "AskQMssg", Question.Body);
                });
            }
        }

    }
}
