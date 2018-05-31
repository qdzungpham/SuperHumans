using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
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
    public class BasicBrowseUsersFragment : Android.Support.V4.App.Fragment
    {
        BrowseUsersAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;
        Spinner filterSpinner;
        ProgressBar progress;
        FloatingActionButton fab;

        public static UsersViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;
        }

        public static BasicBrowseUsersFragment NewInstance()
        {
            var fragment = new BasicBrowseUsersFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_browse_users, null);

            Activity.Title = "SuperHumans";

            ViewModel = new UsersViewModel();

            fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;

            recyclerView.SetAdapter(adapter = new BrowseUsersAdapter(Activity, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_loading);
            progress.Visibility = ViewStates.Gone;

            filterSpinner = view.FindViewById<Spinner>(Resource.Id.filter_spinner);
            filterSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var spinnerAdapter = ArrayAdapter.CreateFromResource(
            Activity, Resource.Array.users_spinner_array, Resource.Layout.spinnerLayout);
            spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            filterSpinner.Adapter = spinnerAdapter;

            fab.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicMyConversationsFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

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
            //Toast.MakeText(Context, "Clicked.", ToastLength.Short).Show();
            var user = ViewModel.Users[e.Position];
            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicUserProfileFragment.NewInstance(Newtonsoft.Json.JsonConvert.SerializeObject(user)))
                .AddToBackStack(null).Commit();
        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadUsersCommand.Execute(filterSpinner.SelectedItem.ToString());
            recyclerView.SetAdapter(adapter = new BrowseUsersAdapter(Activity, ViewModel));
            refresher.Refreshing = false;
            adapter.ItemClick += Adapter_ItemClick;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.speak_menu, menu);

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

        public void BecameVisible()
        {

        }

        private async void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            Spinner spinner = (Spinner)sender;

            await ViewModel.ExecuteLoadUsersCommandAsync(spinner.GetItemAtPosition(e.Position).ToString());
            recyclerView.SetAdapter(adapter = new BrowseUsersAdapter(Activity, ViewModel));
            adapter.ItemClick += Adapter_ItemClick;
        }


    }

    class BrowseUsersAdapter : BaseRecycleViewAdapter
    {
        UsersViewModel viewModel;
        Activity activity;

        public BrowseUsersAdapter(Activity activity, UsersViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

            this.viewModel.Users.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.basic_user_list_item;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new UserViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var user = viewModel.Users[position];

            // Replace the contents of the view with that element
            var myHolder = holder as UserViewHolder;
            myHolder.FullNameView.Text = user.FirstName + " " + user.LastName;
            myHolder.UsernameTextView.Text = "@" + user.Username;

            string topics = "";

            foreach (var topic in user.FollowedTopics)
            {
                topics += topic + " ";
            }

            myHolder.FollowedTopicsTextView.Text = topics;
            if (user.IsFollowed)
            {
                myHolder.FollowIcon.SetColorFilter(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));
                myHolder.FollowText.Text = "Following";
                myHolder.FollowText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));
            }

            myHolder.FollowButton.Click += async (sender, e) =>
            {

                if (!user.IsFollowed)
                {
                    myHolder.FollowIcon.SetColorFilter(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));
                    myHolder.FollowText.Text = "Following";
                    myHolder.FollowText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.alert_green)));

                    await viewModel.ExecuteFollowUsers(user.ObjectId);

                    viewModel.Users[position].IsFollowed = true;
                }
                else
                {
                    myHolder.FollowIcon.SetColorFilter(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.secondaryText)));
                    myHolder.FollowText.Text = "Follow";
                    myHolder.FollowText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(activity, Resource.Color.secondaryText)));

                    await viewModel.ExecuteUnfollowUsers(user.ObjectId);

                    viewModel.Users[position].IsFollowed = false;
                }

            };

        }

        public override int ItemCount => viewModel.Users.Count;
    }

    public class UserViewHolder : RecyclerView.ViewHolder
    {
        public TextView FullNameView { get; set; }
        public TextView UsernameTextView { get; set; }
        public TextView FollowedTopicsTextView { get; set; }
        public RelativeLayout FollowButton { get; set; }
        public ImageView FollowIcon { get; set; }
        public TextView FollowText { get; set; }

        public UserViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            FullNameView = itemView.FindViewById<TextView>(Resource.Id.text_full_name);
            UsernameTextView = itemView.FindViewById<TextView>(Resource.Id.text_username);
            FollowedTopicsTextView = itemView.FindViewById<TextView>(Resource.Id.text_followed_topics);
            FollowButton = itemView.FindViewById<RelativeLayout>(Resource.Id.followBtn);
            FollowIcon = itemView.FindViewById<ImageView>(Resource.Id.followIcon);
            FollowText = itemView.FindViewById<TextView>(Resource.Id.followText);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}