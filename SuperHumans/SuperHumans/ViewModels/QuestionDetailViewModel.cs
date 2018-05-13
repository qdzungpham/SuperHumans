using Acr.UserDialogs;
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

                var topicObjects = question.Get<IList<ParseObject>>("topics");
                var topics = new List<string>();
                foreach (var topic in topicObjects)
                {
                    topics.Add(topic.Get<string>("topicText"));
                }

                var ownerObject = question.Get<ParseObject>("createdBy");
                
                var owner = new User
                {
                    ObjectId = ownerObject.ObjectId,
                    FirstName = ownerObject.Get<string>("firstName"),
                    LastName = ownerObject.Get<string>("lastName"),
                    Username = ownerObject.Get<string>("username")
                };
                Question = new Question()
                {
                    Title = question.Get<string>("title"),
                    Body = question.Get<string>("body"),
                    ObjectId = question.ObjectId,
                    Topics = topics,
                    CreatedAt = question.CreatedAt.Value + RestService.TimeDiff,
                    Owner = owner
                };

                Answers.Clear();
                var answers = await ParseAccess.LoadAnswers(question);
                foreach (var answer in answers)
                {

                    ParseUser userObject = answer.Get<ParseUser>("createdBy");
                    var user = new User
                    {
                        ObjectId = userObject.ObjectId,
                        FirstName = userObject.Get<string>("firstName"),
                        LastName = userObject.Get<string>("lastName"),
                        Username = userObject.Get<string>("username")
                    };

                    DateTime now = RestService.GetServerTime();
                    var a = new Answer
                    {
                        Body = answer.Get<string>("body"),
                        CreatedBy = user,
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

        public async Task SendHelperRequest()
        {
            if (IsBusy) return;

            try
            {
                await ParseAccess.AddRequestedHelper(questionId);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        
    }
}
