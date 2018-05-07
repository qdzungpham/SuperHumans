using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Activities.Advance
{
    [Activity(Label = "Ask Question")]
    public class AnswerActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_answer;

        EditText body;

        public AnswerViewModel ViewModel { get; set; }

        string questionId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            questionId = Intent.GetStringExtra("data");
            ViewModel = new AnswerViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            body = FindViewById<EditText>(Resource.Id.answer_edit_body);
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
                    var answer = new Answer
                    {
                        Body = body.Text,
                        QuestionId = questionId
                    };

                    ViewModel.PostCommand.Execute(answer);

                    //Toast.MakeText(this, "Posted", ToastLength.Short).Show();
                    break;


            }
            return base.OnOptionsItemSelected(item);
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ViewModel.IsPosted && !ViewModel.IsBusy)
            {
                Toast.MakeText(this, "Posted", ToastLength.Short).Show();
                Finish();
            }

        }


    }
}