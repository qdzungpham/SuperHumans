using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Fragments.Advance;

namespace SuperHumans.Droid.Activities.Advance
{
    [Activity(Label = "Get Started")]
    public class GetStartedActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_get_started;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Support.V4.App.Fragment fragment = LoginFragment.NewInstance();
            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}