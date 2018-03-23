using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments
{
    public class AccountFragment : Fragment
    {
        
        public AccountViewModel ViewModel { get; set; }

        TextView fullName, username;

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
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_account, null);

            ViewModel = new AccountViewModel();
            fullName = view.FindViewById<TextView>(Resource.Id.display_name);
            username = view.FindViewById<TextView>(Resource.Id.username);

            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();

            await ViewModel.ExecuteGetUserCommandAsync();

            fullName.Text = ViewModel.UserDetail.FirstName + " " + ViewModel.UserDetail.LastName;
            username.Text = "@" + ViewModel.UserDetail.Username;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.account_top_menus, menu);
            
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    var intent = new Intent(Activity, typeof(SettingsActivity));
                    StartActivity(intent);


                    break;


            }
            return base.OnOptionsItemSelected(item);
        }
    }
}