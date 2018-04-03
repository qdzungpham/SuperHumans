using Android.App;
using Android.OS;
using SuperHumans.Droid.Fragments.Basic;

namespace SuperHumans.Droid.Activities
{
    [Activity(Label = "BasicMode")]
    public class BasicMainActivity : BasicBaseActivity
    {
       
        protected override int LayoutResource => Resource.Layout.basic_activity_main;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Support.V4.App.Fragment fragment = BasicFirstViewFragment.NewInstance();
            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}