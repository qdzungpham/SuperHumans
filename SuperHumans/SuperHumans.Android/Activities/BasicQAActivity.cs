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
    [Activity(Label = "Q&A")]
    public class BasicQAActivity : BasicBaseActivity
    {
        protected override int LayoutResource => Resource.Layout.basic_activity_QA;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }


    }
}