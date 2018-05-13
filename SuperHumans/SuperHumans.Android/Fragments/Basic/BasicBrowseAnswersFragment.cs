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

        TextView questionTitle, questionBody, tags, postedDate, ownerFullName, ownerUsername, answerCount;
        Button helperBtn;
        LinearLayout helpRequestHolder;
        RelativeLayout ownerHolder;
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
            questionBody = view.FindViewById<TextView>(Resource.Id.text_question_body);
            tags = view.FindViewById<TextView>(Resource.Id.text_question_tags);
            answerCount = view.FindViewById<TextView>(Resource.Id.textAnswerCount);
            ownerFullName = view.FindViewById<TextView>(Resource.Id.text_full_name);
            ownerUsername = view.FindViewById<TextView>(Resource.Id.text_username);
            postedDate = view.FindViewById<TextView>(Resource.Id.text_posted_date);
            ownerHolder = view.FindViewById<RelativeLayout>(Resource.Id.owner_holder);
            helperBtn = view.FindViewById<Button>(Resource.Id.btnHelper);
            helpRequestHolder = view.FindViewById<LinearLayout>(Resource.Id.help_request_holder);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = false;

            recyclerView.AddItemDecoration(new DividerItemDecoration(recyclerView.Context, DividerItemDecoration.Vertical));

            recyclerView.SetAdapter(adapter = new BrowseAnswerAdapter(Activity, ViewModel, FragmentManager));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            helperBtn.Click += async (sender, e) =>
            {
                await ViewModel.SendHelperRequest();
                Toast.MakeText(Context, "Your request was sent.", ToastLength.Short).Show();
                helpRequestHolder.Visibility = ViewStates.Gone;
            };
            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;

            if (ViewModel.Question == null)
                await ViewModel.ExecuteLoadQuestionDetailCommandAsync();

            questionTitle.Text = ViewModel.Question.Title;
            questionBody.Text = ViewModel.Question.Body;

            foreach (var topic in ViewModel.Question.Topics)
            {
                tags.Text = tags.Text + topic + " ";
            }

            postedDate.Text = "Posted " + ViewModel.Question.CreatedAt.ToString("d MMMM, yyyy");

            ownerFullName.Text = ViewModel.Question.Owner.FirstName + " " + ViewModel.Question.Owner.LastName;
            ownerUsername.Text = "@" + ViewModel.Question.Owner.Username;

            ownerHolder.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicUserProfileFragment.NewInstance(Newtonsoft.Json.JsonConvert.SerializeObject(ViewModel.Question.Owner)))
                .AddToBackStack(null).Commit();
            };

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
            recyclerView.SetAdapter(adapter = new BrowseAnswerAdapter(Activity, ViewModel, FragmentManager));
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
        Android.Support.V4.App.FragmentManager fragmentManager;

        public BrowseAnswerAdapter(Activity activity, QuestionDetailViewModel viewModel, Android.Support.V4.App.FragmentManager fragmentManager)
        {
            this.viewModel = viewModel;
            this.activity = activity;
            this.fragmentManager = fragmentManager;

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
            myHolder.FullName.Text = answer.CreatedBy.FirstName + " " + answer.CreatedBy.LastName;
            myHolder.TimeAgo.Text = answer.TimeAgo;
            myHolder.BodyView.Text = answer.Body;
            myHolder.FullName.Click += (sender, e) =>
            {
                fragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicUserProfileFragment.NewInstance(Newtonsoft.Json.JsonConvert.SerializeObject(answer.CreatedBy)))
                .AddToBackStack(null).Commit();
            };
        }

        public override int ItemCount => viewModel.Answers.Count;
    }

    class AnswerViewHolder : RecyclerView.ViewHolder
    {
        public TextView FullName { get; set; }
        public TextView TimeAgo { get; set; }
        public TextView BodyView { get; set; }

        public AnswerViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            FullName = itemView.FindViewById<TextView>(Resource.Id.text_full_name);
            TimeAgo = itemView.FindViewById<TextView>(Resource.Id.text_timeago);
            BodyView = itemView.FindViewById<TextView>(Resource.Id.text_body);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}