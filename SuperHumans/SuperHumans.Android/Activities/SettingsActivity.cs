using Android.App;
using Android.OS;
using SuperHumans.Droid.Fragments;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Activities
{
    [Activity(Label = "Settings")]
    public class SettingsActivity : BaseActivity
    {

        protected override int LayoutResource => Resource.Layout.activity_settings;

        public SettingsViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new SettingsViewModel();

            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, new SettingsFragment())
               .Commit();
        }
    }

}