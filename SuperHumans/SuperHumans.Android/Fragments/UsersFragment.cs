using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.App;
using Android.Content;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments
{
    public class UsersFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {
        BrowseUsersAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;

        ProgressBar progress;

        public static UsersViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

            
        }

        public static UsersFragment NewInstance()
        {
            var usersFragment = new UsersFragment { Arguments = new Bundle() };
            return usersFragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_myfeed, null);

            ViewModel = new UsersViewModel();

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;

            recyclerView.SetAdapter(adapter = new BrowseUsersAdapter(Activity, ViewModel));

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

            if (ViewModel.Users.Count == 0)
                await ViewModel.ExecuteLoadUsersCommandAsync("Discover");
        }

        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
            adapter.ItemClick -= Adapter_ItemClick;
        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            Toast.MakeText(Context, "Clicked.", ToastLength.Short).Show();
        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadUsersCommand.Execute(null);
            recyclerView.SetAdapter(adapter = new BrowseUsersAdapter(Activity, ViewModel));
            refresher.Refreshing = false;
            adapter.ItemClick += Adapter_ItemClick;
        }

       

        public void BecameVisible()
        {
            
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
            var id = Resource.Layout.user_browse;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new UserViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = viewModel.Users[position];

            // Replace the contents of the view with that element
            var myHolder = holder as UserViewHolder;
            myHolder.FullNameView.Text = item.FirstName + " " + item.LastName;
            myHolder.UsernameTextView.Text = "@" + item.Username;
        }

        public override int ItemCount => viewModel.Users.Count;
    }

    public class UserViewHolder : RecyclerView.ViewHolder
    {
        public TextView FullNameView { get; set; }
        public TextView UsernameTextView { get; set; }

        public UserViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            FullNameView = itemView.FindViewById<TextView>(Resource.Id.text_full_name);
            UsernameTextView = itemView.FindViewById<TextView>(Resource.Id.text_username);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

}