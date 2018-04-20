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
    public class BasicBrowseAnswersFragment : Android.Support.V4.App.Fragment
    {
        QuestionDetailViewModel ViewModel { get; set; }

        TextView questionTitle, tags, answerCount;
        BrowseAnswerAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public static BasicBrowseAnswersFragment NewInstance()
        {
            var fragment = new BasicBrowseAnswersFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_browse_answers, null);

            Activity.Title = "Browse Responses";

            ViewModel = new QuestionDetailViewModel(Arguments.GetString("questionId"));

            questionTitle = view.FindViewById<TextView>(Resource.Id.text_question_title);
            tags = view.FindViewById<TextView>(Resource.Id.text_question_tags);
            answerCount = view.FindViewById<TextView>(Resource.Id.textAnswerCount);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = false;

            recyclerView.SetAdapter(adapter = new BrowseAnswerAdapter(Activity, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;

            if (ViewModel.Question == null)
                await ViewModel.ExecuteLoadQuestionDetailCommandAsync();

            questionTitle.Text = ViewModel.Question.Title;
            tags.Text = "tags";

            var count = ViewModel.Answers.Count;
            if (count == 0)
            {
                answerCount.Text = "NO RESPONSE YET";
            }
            else if (count == 1)
            {
                answerCount.Text = count + " RESPONSE";
            }
            else
            {
                answerCount.Text = count + " RESPONSES";
            }
        }

        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadQuestionDetailCommand.Execute(null);
            recyclerView.SetAdapter(adapter = new BrowseAnswerAdapter(Activity, ViewModel));
            refresher.Refreshing = false;

            var count = ViewModel.Answers.Count;
            if (count == 0)
            {
                answerCount.Text = "NO RESPONSE YET";
            }
            else if (count == 1)
            {
                answerCount.Text = count + " RESPONSE";
            }
            else
            {
                answerCount.Text = count + " RESPONSES";
            }
        }
    }

    class BrowseAnswerAdapter : BaseRecycleViewAdapter
    {
        QuestionDetailViewModel viewModel;
        Activity activity;

        public BrowseAnswerAdapter(Activity activity, QuestionDetailViewModel viewModel)
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
            var id = Resource.Layout.basic_answer_list_item;
            answerView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new AnswerViewHolder(answerView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var answer = viewModel.Answers[position];

            // Replace the contents of the view with that element
            var myHolder = holder as AnswerViewHolder;
            myHolder.BodyView.Text = answer.Body;
        }

        public override int ItemCount => viewModel.Answers.Count;
    }

    class AnswerViewHolder : RecyclerView.ViewHolder
    {
        public TextView BodyView { get; set; }

        public AnswerViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {

            BodyView = itemView.FindViewById<TextView>(Resource.Id.text_answer_body);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}