using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Activities
{
    [Activity(Label = "LogInActivity")]
    public class LoginActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_log_in;

        AutoCompleteTextView userName;
        TextInputEditText password;
        TextView registerGoCommand;
        TextView forgotPassGoCommand;
        Button loginButton;

        public LoginViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new LoginViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            registerGoCommand = FindViewById<TextView>(Resource.Id.register_prompt);
            forgotPassGoCommand = FindViewById<TextView>(Resource.Id.forgot_password);
            userName = FindViewById<AutoCompleteTextView>(Resource.Id.txtUserName);
            password = FindViewById<TextInputEditText>(Resource.Id.txtPassword);
            loginButton = FindViewById<Button>(Resource.Id.btnConfirm);

            registerGoCommand.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SignUpActivity)); ;
                intent.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent);
                Finish();
            };

            loginButton.Click += Login_Click;

            
        }

        void Login_Click(object sender, EventArgs e)
        {
            var user = new User
            {
                Username = userName.Text,
                Password = password.Text
            };

            ViewModel.LoginCommand.Execute(user);
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ViewModel.CurrentUser == null)
                return;

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.ClearTask);
            StartActivity(intent);
            Finish();
        }
    }
}