using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid
{
    [Activity(Label = "BasicActivity")]
    public class BasicFirstActivity : BasicBaseActivity
    {
        protected override int LayoutResource => Resource.Layout.basic_activity_first_view;

        Button qaBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);

            qaBtn = FindViewById<Button>(Resource.Id.qa);

            qaBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(BasicQAActivity)); ;
                StartActivity(intent);
            };
        }

        
    }
}