using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities.Advance;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Advance
{
    public class SignUpFragment : Fragment
    {
        TextView logInGoCommand;
        AutoCompleteTextView email, userName;
        TextInputEditText password;
        Button signUpButton;

        public static SignUpViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;



        }

        public static SignUpFragment NewInstance()
        {
            var fragment = new SignUpFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_signup, null);

            ViewModel = new SignUpViewModel();


            logInGoCommand = view.FindViewById<TextView>(Resource.Id.login_prompt);
            email = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtEmail);
            userName = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtUserName);
            password = view.FindViewById<TextInputEditText>(Resource.Id.txtPassword);
            signUpButton = view.FindViewById<Button>(Resource.Id.btnConfirm);

            logInGoCommand.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, LoginFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            signUpButton.Click += async (sender, e) =>
            {
                var user = new User
                {
                    Email = email.Text,
                    Username = userName.Text,
                    Password = password.Text
                };

                await ViewModel.SignUp(user);

                if (ViewModel.CurrentUser == null)
                    return;

                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, UserInfoFragment.NewInstance())
                    .AddToBackStack(null).Commit();
            };


            return view;
        }

    }
}