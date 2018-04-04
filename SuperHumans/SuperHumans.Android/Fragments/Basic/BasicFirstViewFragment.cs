using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicFirstViewFragment : Fragment
    {
        Button QA, superHumans;

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

            QA = view.FindViewById<Button>(Resource.Id.qa);
            superHumans = view.FindViewById<Button>(Resource.Id.superhumans);

            QA.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicQAFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };
            
            superHumans.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicBrowseUsersFragment.NewInstance())
                .AddToBackStack(null).Commit();
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

                    ((BasicMainActivity)Activity).Speak("Hello, how are you today?");

                    break;


            }
            return base.OnOptionsItemSelected(item);
        }

    }
}