using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;
using static Android.Views.View;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicMessageFragment : Android.Support.V4.App.Fragment
    {
        Button sendBtn;
        EditText chatbox;
        CancellationTokenSource cts;
        MessageListAdapter adapter;
        RecyclerView recyclerView;
        MessageViewModel viewModel;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;

            


        }

        public static BasicMessageFragment NewInstance(string user)
        {
            var bundle = new Bundle();
            bundle.PutString("userData", user);
            var fragment = new BasicMessageFragment { Arguments = bundle };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_message_list, null);

            
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(Arguments.GetString("userData"));
            Activity.Title = user.FirstName + " " + user.LastName;

            viewModel = new MessageViewModel(user.ObjectId);

            sendBtn = view.FindViewById<Button>(Resource.Id.button_chatbox_send);
            chatbox = view.FindViewById<EditText>(Resource.Id.edittext_chatbox);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.reyclerview_message_list);
            recyclerView.SetAdapter(adapter = new MessageListAdapter(Activity, viewModel));
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));

            recyclerView.ScrollToPosition(viewModel.Messages.Count - 1);

            viewModel.Messages.CollectionChanged += (sender, args) =>
            {
                recyclerView.SmoothScrollToPosition(viewModel.Messages.Count - 1);
            };



            sendBtn.Click += async (sender, e) =>
            {
                await viewModel.SendMessageAsync(chatbox.Text);

                InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                var currentFocus = Activity.CurrentFocus;
                if (currentFocus != null)
                {
                    inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                }
            };
            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();

            await viewModel.GetConversationIdAsync();

            cts = new CancellationTokenSource();


            ThreadPool.QueueUserWorkItem(new WaitCallback(PeriodicLoadMessages), cts.Token);

        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            cts.Cancel();

            cts.Dispose();
        }

        private async void PeriodicLoadMessages(object obj)
        {
            CancellationToken token = (CancellationToken)obj;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    
                    break;
                }
                // load messages every 
                await viewModel.LoadMessagesAsync();

                await Task.Delay(TimeSpan.FromMilliseconds(1000));
                
            }
        }
    }

    class MessageListAdapter : BaseRecycleViewAdapter
    {
        private const int VIEW_TYPE_MESSAGE_SENT = 1;
        private const int VIEW_TYPE_MESSAGE_RECEIVED = 2;
        MessageViewModel viewModel;
        Activity activity;

        public MessageListAdapter(Activity activity, MessageViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

            this.viewModel.Messages.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        public override int GetItemViewType(int position)
        {
            UserMessage message = viewModel.Messages[position];

            if (message.IsMe)
            {
                return VIEW_TYPE_MESSAGE_SENT;
            }
            else
            {
                return VIEW_TYPE_MESSAGE_RECEIVED;
            }
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;

            if (viewType == VIEW_TYPE_MESSAGE_SENT)
            {
                var id = Resource.Layout.sent_message_item;
                itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

                var vh = new SentMessageViewHolder(itemView, OnClick, OnLongClick);
                return vh;
            } else
            {
                var id = Resource.Layout.received_message_item;
                itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

                var vh = new ReceivedMessageViewHolder(itemView, OnClick, OnLongClick);
                return vh;
            }
            
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var message = viewModel.Messages[position];

            // Replace the contents of the view with that element


            switch (holder.ItemViewType)
            {
                case VIEW_TYPE_MESSAGE_SENT:

                    var sentHolder = holder as SentMessageViewHolder;

                    sentHolder.MessageText.Text = message.Body;
                    sentHolder.TimeText.Text = message.CreatedAt.ToShortTimeString();

                    break;
                case VIEW_TYPE_MESSAGE_RECEIVED:

                    var receivedHolder = holder as ReceivedMessageViewHolder;

                    receivedHolder.MessageText.Text = message.Body;
                    receivedHolder.TimeText.Text = message.CreatedAt.ToString();

                    break;
            }
        }

        public override int ItemCount => viewModel.Messages.Count;
    }

    class SentMessageViewHolder : RecyclerView.ViewHolder
    {
        public TextView MessageText{ get; set; }
        public TextView TimeText { get; set; }

        public SentMessageViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            MessageText = itemView.FindViewById<TextView>(Resource.Id.text_message_body);
            TimeText = itemView.FindViewById<TextView>(Resource.Id.text_message_time);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    class ReceivedMessageViewHolder : RecyclerView.ViewHolder
    {
        public TextView MessageText { get; set; }
        public TextView TimeText { get; set; }
        public ImageView ProfileImage { get; set; }

        public ReceivedMessageViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            MessageText = itemView.FindViewById<TextView>(Resource.Id.text_message_body);
            TimeText = itemView.FindViewById<TextView>(Resource.Id.text_message_time);
            ProfileImage = itemView.FindViewById<ImageView>(Resource.Id.image_message_profile);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}