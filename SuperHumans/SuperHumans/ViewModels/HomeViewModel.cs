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
                        Owner = question.Get<string>("owner")
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
    }
}
