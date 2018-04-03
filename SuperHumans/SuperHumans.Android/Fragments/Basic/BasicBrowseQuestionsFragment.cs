using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.ViewModels;
using System;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicBrowseQuestionsFragment : Android.Support.V4.App.Fragment
    {
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

        public static BasicBrowseQuestionsFragment NewInstance()
        {
            var fragment = new BasicBrowseQuestionsFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_browse_questions, null);

            ViewModel = new BrowseQuestionsViewModel();

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = false;

            recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, this, ViewModel));

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
                await ViewModel.ExecuteLoadQuestionsCommandAsync();
        }

        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
            adapter.ItemClick -= Adapter_ItemClick;
        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {

        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadQuestionsCommand.Execute(null);
            recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, this, ViewModel));
            refresher.Refreshing = false;
            adapter.ItemClick += Adapter_ItemClick;
        }
    }

    class BrowseQuestionsAdapter : BaseRecycleViewAdapter
    {
        BrowseQuestionsViewModel viewModel;
        Activity activity;
        Android.Support.V4.App.Fragment fragment;

        public BrowseQuestionsAdapter(Activity activity, Android.Support.V4.App.Fragment fragment, BrowseQuestionsViewModel viewModel)
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
            var id = Resource.Layout.basic_question_list_item;
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
            myHolder.BodyView.Text = question.Title;
            myHolder.AnswersButton.Click += (sender, e) =>
            {
                var newFragment = BasicBrowseAnswersFragment.NewInstance();
                var bundle = new Bundle();
                bundle.PutString("questionId", question.ObjectId);
                newFragment.Arguments = bundle;
                fragment.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, newFragment)
                .AddToBackStack(null).Commit();
            };
        }

        public override int ItemCount => viewModel.Questions.Count;
    }

    class QuestionViewHolder : RecyclerView.ViewHolder
    {
        public TextView BodyView { get; set; }
        public RelativeLayout AnswersButton { get; set;}

        public QuestionViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            BodyView = itemView.FindViewById<TextView>(Resource.Id.text_question_body);
            AnswersButton = itemView.FindViewById<RelativeLayout>(Resource.Id.answerBtn);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

}