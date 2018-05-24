using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicMyConversationsFragment : Android.Support.V4.App.Fragment
    {
        RecyclerView recyclerView;
        ConversationsAdapter adapter;

        public MyConversationsViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;
        }

        public static BasicMyConversationsFragment NewInstance()
        {
            var fragment = new BasicMyConversationsFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_my_coversations, null);

            Activity.Title = "Conversations";

            ViewModel = new MyConversationsViewModel();

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
            recyclerView.AddItemDecoration(new DividerItemDecoration(recyclerView.Context, DividerItemDecoration.Vertical));
            recyclerView.SetAdapter(adapter = new ConversationsAdapter(Activity, ViewModel));

            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();

            adapter.ItemClick += Adapter_ItemClick;

            await ViewModel.LoadMyConversationsAsync();

        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            //Toast.MakeText(Context, "Clicked.", ToastLength.Short).Show();
            var user = ViewModel.Conversations[e.Position].OtherUser;
            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicMessageFragment.NewInstance(Newtonsoft.Json.JsonConvert.SerializeObject(user)))
                .AddToBackStack(null).Commit();
        }
    }

    class ConversationsAdapter : BaseRecycleViewAdapter
    {
        MyConversationsViewModel viewModel;
        Activity activity;

        public ConversationsAdapter(Activity activity, MyConversationsViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

            this.viewModel.Conversations.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.basic_conversation_list_item;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new ConversationViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var conversation = viewModel.Conversations[position];

            // Replace the contents of the view with that element
            var myHolder = holder as ConversationViewHolder;

            myHolder.FullNameText.Text = conversation.OtherUser.FirstName + " " + conversation.OtherUser.LastName;
            myHolder.LastMessageText.Text = conversation.LastMessage.Body;

            var messageDate = conversation.LastMessage.CreatedAt;

            if (messageDate.DayOfYear == DateTime.Now.DayOfYear)
            {
                myHolder.TimeText.Text = messageDate.ToString("h:mm");

            } else
            {
                myHolder.TimeText.Text = messageDate.ToString("d MMMM");
            }


        }

        public override int ItemCount => viewModel.Conversations.Count;
    }

    public class ConversationViewHolder : RecyclerView.ViewHolder
    {
        public TextView FullNameText { get; set; }
        public TextView LastMessageText { get; set; }
        public TextView TimeText { get; set; }

        public ConversationViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            FullNameText = itemView.FindViewById<TextView>(Resource.Id.text_full_name);
            LastMessageText = itemView.FindViewById<TextView>(Resource.Id.text_last_message);
            TimeText = itemView.FindViewById<TextView>(Resource.Id.text_time);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}