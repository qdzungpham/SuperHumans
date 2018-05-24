using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities.Advance;
using SuperHumans.Droid.Activities.Basic;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Advance
{
    public class GetStaredChooseTopicsFragment : ListFragment
    {
        private List<int> selectedTopicIndices;
        public ChooseTopicsViewModel ViewModel { get; private set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;

           

        }

        public static GetStaredChooseTopicsFragment NewInstance()
        {
            var fragment = new GetStaredChooseTopicsFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_get_stared_topics, null);

            ViewModel = new ChooseTopicsViewModel();


            return view;
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var topics = new string[] { "12", "123", "123", "245325" };

            await ViewModel.LoadTopics();

           

            ListAdapter = new MyAdapter(Activity, ViewModel.TopicStrings);

            ListView.TextFilterEnabled = true;

            selectedTopicIndices = new List<int>();

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                if (!selectedTopicIndices.Contains(args.Position))
                {
                    selectedTopicIndices.Add(args.Position);
                }
                else
                {
                    selectedTopicIndices.Remove(args.Position);
                }
            };


        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.choose_topics_top_menu, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_done:
                    if (selectedTopicIndices.Count < 3)
                    {
                        Toast.MakeText(Context, "Please choose at least 3 topics.", ToastLength.Short).Show();
                    }
                    else
                    {
                        ViewModel.SaveFollowedTopicsCommand.Execute(selectedTopicIndices);

                        var intent = new Intent(Activity, typeof(BasicMainActivity));
                        intent.AddFlags(ActivityFlags.ClearTop);
                        intent.AddFlags(ActivityFlags.ClearTask);
                        StartActivity(intent);
                        Activity.Finish();
                    }
                    
                    break;



            }
            return base.OnOptionsItemSelected(item);
        }


    }

    class MyAdapter : BaseAdapter
    {
        Context context;

        string[] topics;

        public MyAdapter(Context c, string[] topics)
        {
            context = c;
            this.topics = topics;
        }

        public override int Count
        {
            get
            {
                return topics.Length;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return topics[position];
        }

        public override long GetItemId(int position)
        {
            return topics[position].GetHashCode();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
                convertView = LayoutInflater.From(context).Inflate(Resource.Layout.topic_item, parent, false);

            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = ((string)GetItem(position));
            return convertView;
        }
    }

}