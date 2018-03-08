using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid
{
    [Activity(Label = "LogInActivity")]
    public class AskActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_ask;

        EditText title, body;

        public AskViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new AskViewModel();

            title = FindViewById<EditText>(Resource.Id.ask_edit_title);
            body = FindViewById<EditText>(Resource.Id.ask_edit_body);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ask_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var question = new Question
            {
                Title = title.Text,
                Body = body.Text
            };

            ViewModel.PostCommand.Execute(question);

            //Toast.MakeText(this, "Posted", ToastLength.Short).Show();

            Finish();

            return base.OnOptionsItemSelected(item);
        }

    }
}