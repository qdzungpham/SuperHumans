using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Activities
{
    [Activity(Label = "Question")]
    public class BrowseQuestionDetailActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_question_details;
        QuestionDetailViewModel ViewModel { get; set; }
        TextView title;
        Button answerBtn;
        BrowseItemsAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new QuestionDetailViewModel(Intent.GetStringExtra("questionId"));

            title = FindViewById<TextView>(Resource.Id.question_view_item_title);
            answerBtn = FindViewById<Button>(Resource.Id.btnAnswer);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = false;

            recyclerView.AddItemDecoration(new DividerItemDecoration(recyclerView.Context, DividerItemDecoration.Vertical));
            recyclerView.SetAdapter(adapter = new BrowseItemsAdapter(this, ViewModel));

            refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            answerBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AnswerActivity));
                intent.PutExtra("data", ViewModel.Question.ObjectId);
                StartActivity(intent);
            };
        }

        protected override async void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;

            if (ViewModel.Question == null)
            {
                await ViewModel.ExecuteLoadQuestionDetailCommandAsync();
            }

            title.Text = ViewModel.Question.Title;
        }

        protected override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadQuestionDetailCommand.Execute(null);
            recyclerView.SetAdapter(adapter = new BrowseItemsAdapter(this, ViewModel));
            refresher.Refreshing = false;
        }


    }

    class BrowseItemsAdapter : BaseRecycleViewAdapter
    {
        QuestionDetailViewModel viewModel;
        Activity activity;

        public BrowseItemsAdapter(Activity activity, QuestionDetailViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

            this.viewModel.Answers.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View answerView = null;
            var id = Resource.Layout.answer_list_item;
            answerView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MyViewHolder(answerView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var answer = viewModel.Answers[position];

            // Replace the contents of the view with that element
            var myHolder = holder as MyViewHolder;
            myHolder.BodyView.Text = answer.Body;
        }

        public override int ItemCount => viewModel.Answers.Count;
    }

    class MyViewHolder : RecyclerView.ViewHolder
    {
        public TextView BodyView { get; set; }

        public MyViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            BodyView = itemView.FindViewById<TextView>(Resource.Id.text_body);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}