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
    [Activity(Label = "Ask Question")]
    public class AskActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_ask;

        EditText title, body;

        public AskViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new AskViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

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
            switch (item.ItemId)
            {
                case Resource.Id.action_post:
                    var question = new Question
                    {
                        Title = title.Text,
                        Body = body.Text
                    };

                    ViewModel.PostCommand.Execute(question);

                    //Toast.MakeText(this, "Posted", ToastLength.Short).Show();


                    break;
                

            }
            return base.OnOptionsItemSelected(item);
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!ViewModel.IsPosted && !ViewModel.IsBusy)
                return;
            Toast.MakeText(this, "Posted", ToastLength.Short).Show();
            Finish();

            
        }

    }
}