﻿using Parse;
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
    public class BrowseQuestionsViewModel : BaseViewModel
    {

        public ObservableCollection<Question> Questions { get; set; }
        public Command LoadQuestionsCommand { get; set; }
        private List<string> followedOppors;

        public BrowseQuestionsViewModel()
        {
            Questions = new ObservableCollection<Question>();

            followedOppors = new List<string>();

            LoadQuestionsCommand = new Command<string>(async (string filter) => await ExecuteLoadQuestionsCommandAsync(filter));
        }


        public async Task ExecuteLoadQuestionsCommandAsync(string filter)
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                Questions.Clear();
                DateTime now = RestService.GetServerTime();

                followedOppors.Clear();

                try
                {
                    followedOppors.AddRange((await ParseAccess.GetUser(ParseAccess.CurrentUser().ObjectId)).Get<IList<string>>("followedOppors"));
                }
                catch (Exception e)
                {

                    Debug.WriteLine(e);
                }

                IEnumerable<Parse.ParseObject> questions;
                if (filter == "Followed Tasks")
                {
                    questions = await ParseAccess.LoadFollowedOppors(followedOppors);
                }
                else if (filter == "Followed Topics")
                {
                    questions = await ParseAccess.LoadOpporsBasedOnTopics();
                }
                else if (filter == "My Requests")
                {
                    questions = await ParseAccess.LoadMyRequests();
                }
                else 
                {
                    questions = await ParseAccess.LoadQuestions();
                }

                foreach (var question in questions)
                {
                    var isFollowed = false;
                    var state = "Closed";
                    var stateFlag = question.Get<int>("stateFlag");

                    var topicObjects = question.Get<IList<ParseObject>>("topics");
                    var topics = new List<string>();
                    foreach (var topic in topicObjects)
                    {
                        topics.Add(topic.Get<string>("topicText"));
                    }

                    if (stateFlag == (int)RequestState.Active)
                    {
                        state = "Active";
                    } else if (stateFlag == (int)RequestState.InProgress)
                    {
                        state = "In Progress";
                    }

                    if (followedOppors.Contains(question.ObjectId))
                    {
                        isFollowed = true;
                    }
                    var q = new Question
                    {
                        ObjectId = question.ObjectId,
                        Title = question.Get<string>("title"),
                        Body = question.Get<string>("body"),
                        TimeAgo = HelperFunctions.TimeAgo(question.UpdatedAt.Value, now),
                        IsFollowed = isFollowed,
                        State = state,
                        Topics = topics
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
        
        public async Task ExecuteFollowOppors(string id)
        {
            followedOppors.Add(id);

            await ParseAccess.UpdateFollowedOppors(followedOppors);
        }

        public async Task ExecuteUnfollowOppors(string id)
        {
            followedOppors.Remove(id);

            await ParseAccess.UpdateFollowedOppors(followedOppors);
        }
    }
}
