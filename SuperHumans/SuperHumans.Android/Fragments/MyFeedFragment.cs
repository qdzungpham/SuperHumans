using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;
using SuperHumans.ViewModels;
using static Android.Resource;

namespace SuperHumans.Droid.Fragments
{
    public class MyFeedFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {
        public static MyFeedFragment NewInstance() =>
            new MyFeedFragment { Arguments = new Bundle() };

        BrowseQuestionsAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;

        ProgressBar progress;
        public static BrowseQuestionsViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_myfeed, null);

            ViewModel = new BrowseQuestionsViewModel();

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
        
            recyclerView.AddItemDecoration(new DividerItemDecoration(recyclerView.Context, DividerItemDecoration.Vertical));
            recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_loading);
            progress.Visibility = ViewStates.Gone;

            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;
            adapter.ItemClick += Adapter_ItemClick;

            if (ViewModel.Questions.Count == 0)
                await ViewModel.ExecuteLoadQuestionsCommandAsync("All Topics");
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
            var intent = new Intent(Activity, typeof(BrowseQuestionDetailActivity));
            intent.PutExtra("questionId", item.ObjectId);
            Activity.StartActivity(intent);
            //Toast.MakeText(Context, item.Title, ToastLength.Short).Show();
        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadQuestionsCommand.Execute(null);
            recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, ViewModel));
            refresher.Refreshing = false;
            adapter.ItemClick += Adapter_ItemClick;
        }

        public void BecameVisible()
        {

        }

    }

    class BrowseQuestionsAdapter : BaseRecycleViewAdapter
    {
        BrowseQuestionsViewModel viewModel;
        Activity activity;

        public BrowseQuestionsAdapter(Activity activity, BrowseQuestionsViewModel viewModel)
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
            var id = Resource.Layout.question_list_item;
            questionView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new QuestionViewHolder(questionView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var question = viewModel.Questions[position];

            // Replace the contents of the view with that element
            var myHolder = holder as QuestionViewHolder;
            myHolder.TextView.Text = question.Title;
            myHolder.DetailTextView.Text = question.Body;
            myHolder.TimeView.Text = question.TimeAgo;
        }

        public override int ItemCount => viewModel.Questions.Count;
    }

    class QuestionViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView { get; set; }
        public TextView DetailTextView { get; set; }
        public TextView TimeView { get; set; }

        public QuestionViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            TextView = itemView.FindViewById<TextView>(Resource.Id.question_list_item_title);
            DetailTextView = itemView.FindViewById<TextView>(Resource.Id.question_list_item_detail);
            TimeView = itemView.FindViewById<TextView>(Resource.Id.question_list_item_time);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}