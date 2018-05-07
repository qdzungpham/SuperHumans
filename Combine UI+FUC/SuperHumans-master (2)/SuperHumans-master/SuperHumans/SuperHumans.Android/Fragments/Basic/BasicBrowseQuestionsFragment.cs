using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;
using SuperHumans.Droid.Activities.Basic;
using SuperHumans.ViewModels;
using System;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicBrowseQuestionsFragment : Android.Support.V4.App.Fragment
    {
        BrowseQuestionsAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;
        Spinner filterSpinner;
        ProgressBar progress;
        public static BrowseQuestionsViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;
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

            Activity.Title = "Find Opportunities";

            ViewModel = new BrowseQuestionsViewModel();

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = false;

            recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, this, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_loading);
            progress.Visibility = ViewStates.Gone;

            filterSpinner = view.FindViewById<Spinner>(Resource.Id.filter_spinner);
            filterSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var spinnerAdapter = ArrayAdapter.CreateFromResource(
            Activity, Resource.Array.opportunity_spinner_array, Resource.Layout.spinnerLayout);
            spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            filterSpinner.Adapter = spinnerAdapter;

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;
            adapter.ItemClick += Adapter_ItemClick;

            
        }

        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
            adapter.ItemClick -= Adapter_ItemClick;
        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            var question = ViewModel.Questions[e.Position];
            ((BasicMainActivity)Activity).Speak(question.Title);

        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadQuestionsCommand.Execute(filterSpinner.SelectedItem.ToString());
            recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, this, ViewModel));
            refresher.Refreshing = false;
            adapter.ItemClick += Adapter_ItemClick;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.basic_UI_menu, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_speak:

                    ((BasicMainActivity)Activity).Speak("Hello, how are you today?");

                    break;


            }
            return base.OnOptionsItemSelected(item);
        }

        private async void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
                
            Spinner spinner = (Spinner)sender;

            await ViewModel.ExecuteLoadQuestionsCommandAsync(spinner.GetItemAtPosition(e.Position).ToString());
            recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, this, ViewModel));
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

            if (question.IsFollowed)
            {
                myHolder.FollowIcon.SetColorFilter(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));
                myHolder.FollowText.Text = "FOLLOWING";
                myHolder.FollowText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));
            }

            myHolder.AnswersButton.Click += (sender, e) =>
            {
                var newFragment = BasicBrowseAnswersFragment.NewInstance();
                var bundle = new Bundle();
                bundle.PutString("questionId", question.ObjectId);
                newFragment.Arguments = bundle;
                fragment.FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, newFragment)
                .AddToBackStack(null).Commit();
            };

            myHolder.FollowButton.Click += async (sender, e) =>
            {
                
                if (!question.IsFollowed)
                {
                    myHolder.FollowIcon.SetColorFilter(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));
                    myHolder.FollowText.Text = "FOLLOWING";
                    myHolder.FollowText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));

                    await viewModel.ExecuteFollowOppors(question.ObjectId);

                    viewModel.Questions[position].IsFollowed = true;
                }
                else
                {
                    myHolder.FollowIcon.SetColorFilter(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.secondaryText)));
                    myHolder.FollowText.Text = "FOLLOW";
                    myHolder.FollowText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.secondaryText)));

                    await viewModel.ExecuteUnfollowOppors(question.ObjectId);

                    viewModel.Questions[position].IsFollowed = false;
                }
                
            };
        }

        public override int ItemCount => viewModel.Questions.Count;
    }

    class QuestionViewHolder : RecyclerView.ViewHolder
    {
        public TextView BodyView { get; set; }
        public RelativeLayout AnswersButton { get; set;}
        public RelativeLayout FollowButton { get; set; }
        public ImageView FollowIcon { get; set; }
        public TextView FollowText { get; set; }

        public QuestionViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            BodyView = itemView.FindViewById<TextView>(Resource.Id.text_question_body);
            AnswersButton = itemView.FindViewById<RelativeLayout>(Resource.Id.answerBtn);
            FollowButton = itemView.FindViewById<RelativeLayout>(Resource.Id.followBtn);
            FollowIcon = itemView.FindViewById<ImageView>(Resource.Id.followIcon);
            FollowText = itemView.FindViewById<TextView>(Resource.Id.followText);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

}