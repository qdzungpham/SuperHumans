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
    public class AnswerActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_answer;

        EditText body;

        //public AskViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            body = FindViewById<EditText>(Resource.Id.answer_edit_body);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ask_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }


    }
}