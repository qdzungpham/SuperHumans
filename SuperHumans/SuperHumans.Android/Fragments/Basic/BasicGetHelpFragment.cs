using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities.Basic;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicGetHelpFragment : Android.Support.V4.App.Fragment
    {
        Button addRequest;
        MyCurrentRequestAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;
        public BrowseQuestionsViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;



        }

        public static BasicGetHelpFragment NewInstance()
        {
            var fragment = new BasicGetHelpFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_get_help, null);

            addRequest = view.FindViewById<Button>(Resource.Id.btnAdd);

            Activity.Title = "Find Opportunities";

            ViewModel = new BrowseQuestionsViewModel();

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = false;
            recyclerView.AddItemDecoration(new DividerItemDecoration(recyclerView.Context, DividerItemDecoration.Vertical));
            recyclerView.SetAdapter(adapter = new MyCurrentRequestAdapter(Activity, this, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            addRequest.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicAskQuestionFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;
            adapter.ItemLongClick += Adapter_ItemLongClick;
            adapter.ItemClick += Adapter_ItemClick;
            ViewModel.LoadQuestionsCommand.Execute("My Requests");

        }

        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
            adapter.ItemLongClick -= Adapter_ItemLongClick;
            adapter.ItemClick -= Adapter_ItemClick;
        }

        void Adapter_ItemLongClick(object sender, RecyclerClickEventArgs e)
        {
            var question = ViewModel.Questions[e.Position];
            ((BasicMainActivity)Activity).Speak(question.Title);

        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            var question = ViewModel.Questions[e.Position];
            var newFragment = BasicMyRequestDetailFragment.NewInstance();
            var bundle = new Bundle();
            bundle.PutString("questionId", question.ObjectId);
            newFragment.Arguments = bundle;
            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, newFragment)
            .AddToBackStack(null).Commit();

        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadQuestionsCommand.Execute("My Requests");
            recyclerView.SetAdapter(adapter = new MyCurrentRequestAdapter(Activity, this, ViewModel));
            adapter.ItemLongClick += Adapter_ItemLongClick;
            adapter.ItemClick += Adapter_ItemClick;
            refresher.Refreshing = false;
            
        }
    }

    class MyCurrentRequestAdapter : BaseRecycleViewAdapter
    {
        BrowseQuestionsViewModel viewModel;
        Activity activity;
        Android.Support.V4.App.Fragment fragment;

        public MyCurrentRequestAdapter(Activity activity, Android.Support.V4.App.Fragment fragment, BrowseQuestionsViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;
            this.fragment = fragment;

            this.viewModel.Questions.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View questionView = null;
            var id = Resource.Layout.basic_current_request_list_item;
            questionView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MyCurrentRequestViewHolder(questionView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var question = viewModel.Questions[position];

            // Replace the contents of the view with that element
            var myHolder = holder as MyCurrentRequestViewHolder;
            myHolder.TitleText.Text = question.Title;
            myHolder.StateText.Text = question.State;
            
            if (question.State == "Active")
            {
                myHolder.StateText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));
            } else if (question.State == "In Progress")
            {
                myHolder.StateText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_yellow)));
            } else if (question.State == "Closed")
            {
                myHolder.StateText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_red)));
            }


            
        }

        public override int ItemCount => viewModel.Questions.Count;
    }

    class MyCurrentRequestViewHolder : RecyclerView.ViewHolder
    {
        public TextView TitleText { get; set; }
        public TextView StateText { get; set; }

        public MyCurrentRequestViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            TitleText = itemView.FindViewById<TextView>(Resource.Id.text_title);
            StateText = itemView.FindViewById<TextView>(Resource.Id.state_text);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}