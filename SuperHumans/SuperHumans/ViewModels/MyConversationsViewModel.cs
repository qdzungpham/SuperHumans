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
    public class MyConversationsViewModel : BaseViewModel
    {
        public ObservableCollection<Conversation> Conversations { get; set; }

        public MyConversationsViewModel()
        {
            Conversations = new ObservableCollection<Conversation>();
        }

        public async Task LoadMyConversationsAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");
            try
            {
                IsBusy = true;

                var conversations = await ParseAccess.LoadMyConversations();

                foreach (var conversation in conversations)
                {
                    var members = conversation.Get<IList<ParseObject>>("members");

                    var otherUser = new User
                    {
                        ObjectId = members[0].ObjectId,
                        Username = members[0].Get<string>("username"),
                        FirstName = members[0].Get<string>("firstName"),
                        LastName = members[0].Get<string>("lastName")
                    };

                    var messageObject = await ParseAccess.GetLastMessage(conversation.ObjectId);

                    if (messageObject != null)
                    {
                        var lastMessage = new UserMessage
                        {
                            Body = messageObject.Get<string>("body"),
                            CreatedAt = messageObject.CreatedAt.Value + RestService.TimeDiff
                        };

                        var conv = new Conversation
                        {
                            OtherUser = otherUser,
                            LastMessage = lastMessage
                        };

                        Conversations.Add(conv); 
                    }
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
