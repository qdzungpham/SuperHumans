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
using WebSocket4Net;

namespace SuperHumans.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        private string otherUserId;
        private static string conversationId;
        public ObservableCollection<UserMessage> Messages { get; set; }

        public Command LoadMessagesCommand { get; set; }

        public Command GetConversationIdCommand { get; set; }

        public MessageViewModel(string otherUserId)
        {
            this.otherUserId = otherUserId;
            Messages = new ObservableCollection<UserMessage>();

            LoadMessagesCommand = new Command(async () => await LoadMessagesAsync());

           

        }

        //public void EnableLiveQuery()
        //{
        //    _ws = new WebSocket("ws://ec2-52-63-31-67.ap-southeast-2.compute.amazonaws.com:1337/");

        //    _ws.Open();
        //    _ws.Opened += (sender, e) =>
        //    {
        //        _ws.Send(Newtonsoft.Json.JsonConvert.SerializeObject(new { op = "connect", applicationId = "SuperMen", windowsKey = "rickpham" }));

        //    };
            
        //    _ws.MessageReceived += (sender, e) =>
        //    {
        //        Debug.WriteLine(e.Message);


        //        Debug.WriteLine(_ws.State);
        //    };
        //    _ws.Error += (sender, e) =>
        //    {
        //        Debug.WriteLine(e.Exception);


        //    };

        //    Debug.WriteLine(_ws.State);
        //}

        //public void Subscribe()
        //{
        //    _ws.Send(Newtonsoft.Json.JsonConvert.SerializeObject(new { op = "subscribe", requestId = 1, query = new { className = "Message", where = new { conversationId } } }));
        //}


        public async Task GetConversationIdAsync()
        {
            try
            {
                conversationId = await ParseAccess.GetConversationId(otherUserId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task SendMessageAsync(string body)
        {
            try
            {
                await ParseAccess.SendMessage(conversationId, body);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task LoadMessagesAsync()
        {
            try
            {
                
                var messages = await ParseAccess.LoadMessages(conversationId);
                
                foreach (var message in messages)
                {

                    var isMe = false;
                    if (message.Get<string>("author") == ParseAccess.CurrentUser().ObjectId)
                    {
                        isMe = true;
                    }

                    var newMessage = new UserMessage
                    {
                        ObjectId = message.ObjectId,
                        Body = message.Get<string>("body"),
                        IsMe = isMe,
                        CreatedAt = message.CreatedAt.Value + RestService.TimeDiff
                    };

                    if (!Messages.Contains(newMessage))
                    {
                        Messages.Add(newMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
