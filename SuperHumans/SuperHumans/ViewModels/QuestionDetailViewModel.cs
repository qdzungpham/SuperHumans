using Parse;
using SuperHumans.Helpers;
using SuperHumans.Models;
using SuperHumans.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class QuestionDetailViewModel : BaseViewModel
    {
        public ObservableCollection<Answer> Answers { get; set; }

        public Command LoadQuestionDetailCommand { get; private set; }

        public Question Question { get; private set; }

        string questionId;

        public QuestionDetailViewModel(string questionId)
        {
            this.questionId = questionId;
            Answers = new ObservableCollection<Answer>();
            LoadQuestionDetailCommand = new Command(async () => await ExecuteLoadQuestionDetailCommandAsync());
        }

        public async Task ExecuteLoadQuestionDetailCommandAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                var question = await ParseAccess.GetQuestion(questionId);
                Question = new Question()
                {
                    Title = question.Get<string>("title"),
                    Body = question.Get<string>("body"),
                    ObjectId = question.ObjectId
                };

                Answers.Clear();
                var answers = await ParseAccess.LoadAnswers(question);
                foreach (var answer in answers)
                {

                    ParseUser user = answer.Get<ParseUser>("createdBy");
                    DateTime now = RestService.GetServerTime();
                    var a = new Answer
                    {
                        Body = answer.Get<string>("body"),
                        CreatedBy = user.Username,
                        TimeAgo = HelperFunctions.TimeAgo(answer.UpdatedAt.Value, now)
                    };
                    Answers.Add(a);
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
