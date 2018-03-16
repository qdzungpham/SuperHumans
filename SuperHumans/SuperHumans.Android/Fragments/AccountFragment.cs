using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;

namespace SuperHumans.Droid.Fragments
{
    public class AccountFragment : Fragment
    {
        Button settingsBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
 
            HasOptionsMenu = true;
        }

        public static AccountFragment NewInstance()
        {
            var accountFragment = new AccountFragment { Arguments = new Bundle() };
            return accountFragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_account, container, false);

            settingsBtn = view.FindViewById<Button>(Resource.Id.settings);

            settingsBtn.Click += (sender, e) =>
            {
                var intent = new Intent(Activity, typeof(SettingsActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                Activity.StartActivity(intent);
            };


            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.account_top_menus, menu);
            
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }
    }
}