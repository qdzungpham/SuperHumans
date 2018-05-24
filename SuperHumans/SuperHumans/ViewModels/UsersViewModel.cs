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
    public class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public Command LoadUsersCommand { get; set; }
        private List<string> followedUsers;

        public UsersViewModel()
        {
            Users = new ObservableCollection<User>();

            followedUsers = new List<string>();

            LoadUsersCommand = new Command<string>(async (string filter) => await ExecuteLoadUsersCommandAsync(filter));
        }

        public async Task ExecuteLoadUsersCommandAsync(string filter)
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");
            try
            {
                IsBusy = true;
                Users.Clear();


                followedUsers.Clear();

                try
                {
                    followedUsers.AddRange((await ParseAccess.GetUser(ParseAccess.CurrentUser().ObjectId)).Get<IList<string>>("followedUsers"));
                }
                catch (Exception e)
                {

                    Debug.WriteLine(e);
                }

                IEnumerable<Parse.ParseObject> users;
                if (filter == "Followed Users")
                {
                    users = await ParseAccess.LoadFollowedUsers(followedUsers);
                }
                else
                {
                    users = await ParseAccess.LoadUsers();
                }

                foreach (var user in users)
                {
                    var isFollowed = false;

                    var topicObjects = user.Get<IList<ParseObject>>("followedTopics");
                    var topics = new List<string>();
                    foreach (var topic in topicObjects)
                    {
                        topics.Add(topic.Get<string>("topicText"));
                    }
                    if (followedUsers.Contains(user.ObjectId))
                    {
                        isFollowed = true;
                    }
                    var u = new User
                    {
                        Username = user.Get<string>("username"),
                        FirstName = user.Get<string>("firstName"),
                        LastName = user.Get<string>("lastName"),
                        ObjectId = user.ObjectId,
                        IsFollowed = isFollowed,
                        FollowedTopics = topics
                    };
                    Users.Add(u);
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

        public async Task ExecuteFollowUsers(string id)
        {
            followedUsers.Add(id);

            await ParseAccess.UpdateFollowedUsers(followedUsers);
        }

        public async Task ExecuteUnfollowUsers(string id)
        {
            followedUsers.Remove(id);

            await ParseAccess.UpdateFollowedUsers(followedUsers);
        }
    }
}
