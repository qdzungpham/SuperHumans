using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SuperHumans.Droid
{
    [Activity(Label = "LogInActivity")]
    public class LogInActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_log_in;
        TextView registerGoCommand;
        TextView forgotPassGoCommand;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            registerGoCommand = FindViewById<TextView>(Resource.Id.register_prompt);
            forgotPassGoCommand = FindViewById<TextView>(Resource.Id.forgot_password);

            registerGoCommand.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SignUpActivity)); ;
                StartActivity(intent);
            };
        }
    }
}