using SuperHumans.Helpers;
using SuperHumans.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class QuestionDetailViewModel : BaseViewModel
    {
        public Command GetQuestionCommand { get; private set; }

        public Question QuestionDetail { get; set; }

        string questionId;

        public QuestionDetailViewModel(string questionId)
        {
            this.questionId = questionId;
            GetQuestionCommand = new Command(async () => await ExecuteGetQuestionCommandAsync());
        }

        public async Task ExecuteGetQuestionCommandAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                var question = await ParseAccess.GetQuestion(questionId);
                QuestionDetail = new Question()
                {
                    Title = question.Get<string>("title"),
                    Body = question.Get<string>("body")
                };
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
