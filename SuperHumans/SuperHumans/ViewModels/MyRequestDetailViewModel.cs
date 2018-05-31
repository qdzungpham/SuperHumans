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
    public enum RequestState{
        InProgress = 1, 
        Active = 2, 
        Closed = 3
    }
    public class MyRequestDetailViewModel : BaseViewModel
    {
        public Question Request { get; private set; }
        public ObservableCollection<User> Helpers { get; set; }

        string requestId;

        public MyRequestDetailViewModel(string requestId)
        {
            this.requestId = requestId;

            Helpers = new ObservableCollection<User>();
            
        }

        public async Task LoadMyRequestDetailAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                var request = await ParseAccess.GetMyRequest(requestId);
                var state = "Closed";
                var stateFlag = request.Get<int>("stateFlag");

                if (stateFlag == (int)RequestState.Active)
                {
                    state = "Active";
                }
                else if (stateFlag == (int)RequestState.InProgress)
                {
                    state = "In Progress";
                }


                var topicObjects = request.Get<IList<ParseObject>>("topics");
                var topics = new List<string>();
                foreach (var topic in topicObjects)
                {
                    topics.Add(topic.Get<string>("topicText"));
                }

                var helpers = request.Get<IList<ParseUser>>("requestedHelpers");
                foreach (var helper in helpers)
                {
                    var user = new User
                    {
                        ObjectId = helper.ObjectId,
                        FirstName = helper.Get<string>("firstName"),
                        LastName = helper.Get<string>("lastName"),
                        Username = helper.Get<string>("username")
                    };

                    Helpers.Add(user);
                }
                Request = new Question()
                {
                    Title = request.Get<string>("title"),
                    Body = request.Get<string>("body"),
                    ObjectId = request.ObjectId,
                    Topics = topics,
                    CreatedAt = request.CreatedAt.Value + RestService.TimeDiff,
                    State = state
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
