using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SuperHumans.ViewModels;
using System;

namespace SuperHumans.Droid.Activities
{
    [Activity(Label = "LogInActivity")]
    public class SettingsActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_settings;

        Button signOut;
        Button basicModeSwitch;

        public SettingsViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new SettingsViewModel();

            signOut = FindViewById<Button>(Resource.Id.signOut);
            basicModeSwitch = FindViewById<Button>(Resource.Id.basicMode);

            signOut.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(LoginActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                ViewModel.SignOutCommand.Execute(null);
                MainActivity.activity.Finish();
                StartActivity(intent);
                Finish();
            };

            basicModeSwitch.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(BasicFirstActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                MainActivity.activity.Finish();
                StartActivity(intent);
                Finish();
            };
        }
    }
}