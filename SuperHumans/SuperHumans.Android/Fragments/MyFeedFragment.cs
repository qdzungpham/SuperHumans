using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments
{
    public class MyFeedFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {
        public static MyFeedFragment NewInstance() =>
            new MyFeedFragment { Arguments = new Bundle() };

        BrowseItemsAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;

        ProgressBar progress;
        public static HomeViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel = new HomeViewModel();

            View view = inflater.Inflate(Resource.Layout.fragment_myfeed, container, false);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter = new BrowseItemsAdapter(Activity, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_loading);
            progress.Visibility = ViewStates.Gone;

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;
            adapter.ItemClick += Adapter_ItemClick;

            if (ViewModel.Questions.Count == 0)
                ViewModel.LoadQuestionsCommand.Execute(null);
        }

        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
            adapter.ItemClick -= Adapter_ItemClick;
        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            var item = ViewModel.Questions[e.Position];
            var intent = new Intent(Activity, typeof(SettingsViewModel));

            intent.PutExtra("data", Newtonsoft.Json.JsonConvert.SerializeObject(item));
            Activity.StartActivity(intent);
        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadQuestionsCommand.Execute(null);
            recyclerView.SetAdapter(adapter = new BrowseItemsAdapter(Activity, ViewModel));
            refresher.Refreshing = false;
        }

        public void BecameVisible()
        {

        }

    }

    class BrowseItemsAdapter : BaseRecycleViewAdapter
    {
        HomeViewModel viewModel;
        Activity activity;

        public BrowseItemsAdapter(Activity activity, HomeViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

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
            var id = Resource.Layout.question_browse;
            questionView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MyViewHolder(questionView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var question = viewModel.Questions[position];

            // Replace the contents of the view with that element
            var myHolder = holder as MyViewHolder;
            myHolder.TextView.Text = question.Title;
            myHolder.DetailTextView.Text = question.Body;
        }

        public override int ItemCount => viewModel.Questions.Count;
    }

    public class MyViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView { get; set; }
        public TextView DetailTextView { get; set; }

        public MyViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            TextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text1);
            DetailTextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text2);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}