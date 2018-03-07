using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;
using System;

namespace SuperHumans.Droid
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.activity_sign_up;
        TextView logInGoCommand;
        AutoCompleteTextView email, userName;
        TextInputEditText password;
        Button signUpButton;

        public SignUpViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new SignUpViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            logInGoCommand = FindViewById<TextView>(Resource.Id.login_prompt);
            email = FindViewById<AutoCompleteTextView>(Resource.Id.txtEmail);
            userName = FindViewById<AutoCompleteTextView>(Resource.Id.txtUserName);
            password = FindViewById<TextInputEditText>(Resource.Id.txtPassword);
            signUpButton = FindViewById<Button>(Resource.Id.btnConfirm);

            logInGoCommand.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(LoginActivity)); ;
                StartActivity(intent);
            };
            signUpButton.Click += SignUp_Click;
        }

        void SignUp_Click(object sender, EventArgs e)
        {
            var user = new User
            {
                Email = email.Text,
                Username = userName.Text,
                Password = password.Text
            };

            ViewModel.SignUpCommand.Execute(user);
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!ViewModel.CurrentUser)
                return;

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}