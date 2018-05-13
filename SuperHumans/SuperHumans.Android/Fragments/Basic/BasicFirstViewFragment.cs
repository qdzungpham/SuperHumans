using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;
using SuperHumans.Droid.Activities.Advance;
using SuperHumans.Droid.Activities.Basic;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicFirstViewFragment : Fragment
    {
        Button giveHelp, getHelp, superHumans;

        public SettingsViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;

          

        }

        public static BasicFirstViewFragment NewInstance()
        {
            var fragment = new BasicFirstViewFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_first_view, null);

            ViewModel = new SettingsViewModel();

            giveHelp = view.FindViewById<Button>(Resource.Id.give_help);
            getHelp = view.FindViewById<Button>(Resource.Id.get_help);
            superHumans = view.FindViewById<Button>(Resource.Id.superhumans);

            giveHelp.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicBrowseQuestionsFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            giveHelp.LongClick += (sender, e) =>
            {
                ((BasicMainActivity)Activity).Speak("Give Help");
            };

            getHelp.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicGetHelpFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            getHelp.LongClick += (sender, e) =>
            {
                ((BasicMainActivity)Activity).Speak("Get Help");
            };

            superHumans.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicBrowseUsersFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            superHumans.LongClick += (sender, e) =>
            {
                ((BasicMainActivity)Activity).Speak("Superhumans");
            };


            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.basic_first_view_menu, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_switch:
                    ViewModel.SwitchToAdvancedUI();
                    var intent = new Intent(Activity, typeof(MainActivity));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                    Activity.Finish();
                    break;
                case Resource.Id.action_speak:

                    ((BasicMainActivity)Activity).Speak("You are in first screen. Press opportunities to view available opportunities, " +
                        "or press superhumans to view all superhumans around you");

                    break;


            }
            return base.OnOptionsItemSelected(item);
        }

    }
}