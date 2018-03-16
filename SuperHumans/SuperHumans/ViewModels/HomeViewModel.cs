﻿using SuperHumans.Helpers;
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
                        TimeAgo = getTimeDiff(question.UpdatedAt.Value, now)
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

        private string getTimeDiff(DateTime questionCreatedDate, DateTime now)
        {
            string result = "";
            TimeSpan diff = now - questionCreatedDate;

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
