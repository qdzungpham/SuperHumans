using SuperHumans.Helpers;
using SuperHumans.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<Question> Questions { get; set; }
        public Command LoadQuestionsCommand { get; set; }

        public HomeViewModel()
        {
            Questions = new ObservableCollection<Question>();
            LoadQuestionsCommand = new Command(async () => await ExecuteLoadQuestionsCommand());
        }

        private async Task ExecuteLoadQuestionsCommand()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                Questions.Clear();
                var questions = await ParseAccess.LoadQuestions();
                foreach (var question in questions)
                {
                    var q = new Question
                    {
                        Title = question.Get<string>("title"),
                        Body = question.Get<string>("body"),
                        Owner = question.Get<string>("owner"),
                        Time = getTimeDiff(question.Get<DateTime>("createdDate"), DateTime.Now)
                        //Time = getTimeDiff(question.CreatedAt.Value, DateTime.Now)
                    };
                    Questions.Add(q);
                }
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string getTimeDiff(DateTime questionCreatedDate, DateTime now)
        {
            string result = "";
            TimeSpan diff = now - questionCreatedDate;
            Console.WriteLine(diff);

            if (diff.Days != 0)
            {
                if (diff.Days == 1)
                {
                    result = "Yesterday";
                } else
                {
                    result = diff.Days.ToString() + " days ago";
                }
            } else if (diff.Hours != 0)
            {
                result = diff.Hours.ToString() + " hours ago";
            } else if (diff.Minutes != 0)
            {
                if (diff.Minutes == 1)
                {
                    result = "1 minute ago";
                } else
                {
                    result = diff.Minutes.ToString() + " minutes ago";
                }
            } else
            {
                result = diff.Seconds.ToString() + " seconds ago";
            }

            return result;
        }
    }
}
