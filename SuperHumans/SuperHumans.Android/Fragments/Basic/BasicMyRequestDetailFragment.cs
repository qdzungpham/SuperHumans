using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicMyRequestDetailFragment : Android.Support.V4.App.Fragment
    {
        TextView questionTitle, questionBody, tags, postedDate, spinnerText;
        HelpersAdapter adapter;
        SwipeRefreshLayout refresher;
        RecyclerView recyclerView;
        Spinner statusSpinner;

        MyRequestDetailViewModel ViewModel { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;



        }

        public static BasicMyRequestDetailFragment NewInstance()
        {
            var fragment = new BasicMyRequestDetailFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_my_request_detail, null);

            Activity.Title = "My Request Detail";

            ViewModel = new MyRequestDetailViewModel(Arguments.GetString("questionId"));

            questionTitle = view.FindViewById<TextView>(Resource.Id.text_question_title);
            questionBody = view.FindViewById<TextView>(Resource.Id.text_question_body);
            tags = view.FindViewById<TextView>(Resource.Id.text_question_tags);
            postedDate = view.FindViewById<TextView>(Resource.Id.text_posted_date);
            

            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = false;

            recyclerView.SetAdapter(adapter = new HelpersAdapter(Activity, this, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            statusSpinner = view.FindViewById<Spinner>(Resource.Id.status_spinner);
            statusSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var spinnerAdapter = ArrayAdapter.CreateFromResource(
            Activity, Resource.Array.request_status_array, Resource.Layout.statusSpinnerLayout);
            spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            statusSpinner.Adapter = spinnerAdapter;

            //spinnerText = (TextView) statusSpinner.GetChildAt(0);
            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();

            //refresher.Refresh += Refresher_Refresh;
            adapter.ItemClick += Adapter_ItemClick;

            if (ViewModel.Request == null)
                await ViewModel.LoadMyRequestDetailAsync();

            questionTitle.Text = ViewModel.Request.Title;
            questionBody.Text = ViewModel.Request.Body;

            foreach (var topic in ViewModel.Request.Topics)
            {
                tags.Text = tags.Text + topic + " ";
            }

            postedDate.Text = "Posted " + ViewModel.Request.CreatedAt.ToString("d MMMM, yyyy");



        }

        public override void OnStop()
        {
            base.OnStop();
            //refresher.Refresh -= Refresher_Refresh;
            adapter.ItemClick -= Adapter_ItemClick;
        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            var helper = ViewModel.Helpers[e.Position];
            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicUserProfileFragment.NewInstance(Newtonsoft.Json.JsonConvert.SerializeObject(helper)))
                .AddToBackStack(null).Commit();

        }
        //void Refresher_Refresh(object sender, EventArgs e)
        //{
        //    ViewModel.LoadMyRequestDetailAsync.Execute();
        //    recyclerView.SetAdapter(adapter = new BrowseQuestionsAdapter(Activity, this, ViewModel));
        //    refresher.Refreshing = false;
        //    adapter.ItemLongClick += Adapter_ItemLongClick;
        //}

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            Spinner spinner = (Spinner)sender;


            //if (e.Position == 0)
            //{
            //    spinnerText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(Activity, Resource.Color.alert_green)));
            //}
            //else if (e.Position == 1)
            //{
            //    spinnerText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(Activity, Resource.Color.alert_yellow)));
            //}
            //else if (e.Position == 2)
            //{
            //    spinnerText.SetTextColor(new Color(Android.Support.V4.Content.ContextCompat.GetColor(Activity, Resource.Color.alert_red)));
            //}


        }
    }

    class HelpersAdapter : BaseRecycleViewAdapter
    {
        MyRequestDetailViewModel viewModel;
        Activity activity;
        Android.Support.V4.App.Fragment fragment;

        public HelpersAdapter(Activity activity, Android.Support.V4.App.Fragment fragment, MyRequestDetailViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;
            this.fragment = fragment;

            this.viewModel.Helpers.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View view = null;
            var id = Resource.Layout.basic_helper_list_item;
            view = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new HelperViewHolder(view, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var helper = viewModel.Helpers[position];

            // Replace the contents of the view with that element
            var myHolder = holder as HelperViewHolder;

            myHolder.FullNameText.Text = helper.FirstName + " " + helper.LastName;
            myHolder.UsernameText.Text = "@" + helper.Username;

            
        }

        public override int ItemCount => viewModel.Helpers.Count;
    }

    class HelperViewHolder : RecyclerView.ViewHolder
    {
        public TextView FullNameText { get; set; }
        public TextView UsernameText { get; set; }

        public HelperViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            FullNameText = itemView.FindViewById<TextView>(Resource.Id.text_full_name);
            UsernameText = itemView.FindViewById<TextView>(Resource.Id.text_username);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}