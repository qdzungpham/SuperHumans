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
            LoadQuestionsCommand = new Command(async () => await ExecuteLoadQuestionsCommandAsync());
        }

        public async Task ExecuteLoadQuestionsCommandAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                Questions.Clear();
                DateTime now = RestService.GetServerTime();
                var questions = await ParseAccess.LoadQuestions();
                foreach (var question in questions)
                {
                    var q = new Question
                    {
                        ObjectId = question.ObjectId,
                        Title = question.Get<string>("title"),
                        Body = question.Get<string>("body"),
                        TimeAgo = HelperFunctions.TimeAgo(question.UpdatedAt.Value, now)
                    };
                    Questions.Add(q);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
                ProgressDialogManager.DisposeProgressDialog();
            }
        }     
    }
}
