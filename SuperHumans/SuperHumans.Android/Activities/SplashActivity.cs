using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid
{
	[Activity(Label = "@string/app_name", Theme = "@style/SplashTheme", MainLauncher = true)]
	public class SplashActivity : AppCompatActivity
	{

        public BaseViewModel ViewModel { get; set; }
		protected override void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);

            ViewModel = new BaseViewModel();

            Intent newIntent;

            if (ViewModel.CurrentUser)
            {
                newIntent = new Intent(this, typeof(MainActivity));
            } else
            {
                newIntent = new Intent(this, typeof(LoginActivity));
            }

			newIntent.AddFlags(ActivityFlags.ClearTop);
			newIntent.AddFlags(ActivityFlags.SingleTop);       
			StartActivity(newIntent);
			Finish();
		}
	}
}
