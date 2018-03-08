using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using SuperHumans.Droid.Fragments;

namespace SuperHumans.Droid
{
	[Activity(Label = "@string/app_name", Icon = "@mipmap/icon",
		LaunchMode = LaunchMode.SingleInstance,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : BaseActivity
	{
		protected override int LayoutResource => Resource.Layout.activity_main;

        BottomNavigationView bottomNavigation;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
       

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;
            LoadFragment(Resource.Id.menu_home);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.menu_home:
                    fragment = HomeFragment.NewInstance();
                    break;
                case Resource.Id.menu_users:
                    fragment = UsersFragment.NewInstance();
                    break;
                case Resource.Id.menu_account:
                    fragment = AccountFragment.NewInstance();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }
    }

	
}
