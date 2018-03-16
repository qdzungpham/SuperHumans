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
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Activities
{
    [Activity(Label = "Question")]
    public class BrowseQuestionDetailActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_question_details;
        QuestionDetailViewModel ViewModel { get; set; }
        TextView title, body;
        Button answerBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new QuestionDetailViewModel(Intent.GetStringExtra("objectId"));

            title = FindViewById<TextView>(Resource.Id.question_view_item_title);
            body = FindViewById<TextView>(Resource.Id.question_view_item_body);
            answerBtn = FindViewById<Button>(Resource.Id.btnAnswer);

            answerBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AnswerActivity)); ;
                StartActivity(intent);
            };
        }

        protected override async void OnStart()
        {
            base.OnStart();
            
            if (ViewModel.QuestionDetail == null)
            {
                await ViewModel.ExecuteGetQuestionCommandAsync();
            }

            title.Text = ViewModel.QuestionDetail.Title;
            body.Text = ViewModel.QuestionDetail.Body;
        }


    }
}