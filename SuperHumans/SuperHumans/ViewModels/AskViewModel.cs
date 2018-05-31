using Acr.UserDialogs;
using Parse;
using SuperHumans.Helpers;
using SuperHumans.Models;
using SuperHumans.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class AskViewModel : BaseViewModel
    {
        public static List<ParseObject> Topics { get; private set; }

        public Command PostCommand { get; private set; }

        bool isPosted = false;
        public bool IsPosted
        {
            get { return isPosted; }
            set { SetProperty(ref isPosted, value); }
        }

        public AskViewModel()
        {
            Title = "Ask Question";
            Topics = new List<ParseObject>();
            PostCommand = new Command<Question>(async (Question question) => await ExecutePostQuestionAsync(question));
        }

        public async Task ExecutePostQuestionAsync(Question question)
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                await ParseAccess.AddQuestion(question, Topics);
                
                //UserDialogs.Instance.Toast("Posted.", TimeSpan.FromSeconds(3));

                IsPosted = true;
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message, "ERROR", "OK");
            }
            finally
            {
                IsBusy = false;

                ProgressDialogManager.DisposeProgressDialog();
            }
        }

        public string GetChoosedTopicsString()
        {
            if (Topics.Count == 0) return "Topics";
            var topicsString = "";
            foreach (var topic in Topics)
            {
                topicsString = topicsString + topic.Get<string>("topicText") + ", ";
            }
            return topicsString;
        }
    }
}
