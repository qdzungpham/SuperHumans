using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments
{
    public class SettingsFragment : PreferenceFragment
    {
        public SettingsViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            AddPreferencesFromResource(Resource.Layout.preferences);

            ViewModel = new SettingsViewModel();

            FindPreference("log_out").PreferenceClick += (sender, args) =>
            {
                var intent = new Intent(Activity, typeof(LoginActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                ViewModel.SignOutCommand.Execute(null);
                MainActivity.activity.Finish();
                StartActivity(intent);
                Activity.Finish();
            };

            FindPreference("switch_UI").PreferenceClick += (sender, args) =>
            {
                var intent = new Intent(Activity, typeof(BasicMainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                MainActivity.activity.Finish();
                StartActivity(intent);
                Activity.Finish();
            };
        }

        public static SettingsFragment NewInstance()
        {
            var fragment = new SettingsFragment { Arguments = new Bundle() };
            return fragment;
        }


    }
}