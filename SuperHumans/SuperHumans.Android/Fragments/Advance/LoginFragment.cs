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
using SuperHumans.Droid.Activities.Basic;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Advance
{
    public class LoginFragment : Fragment
    {
        AutoCompleteTextView userName;
        TextInputEditText password;
        TextView registerGoCommand;
        TextView forgotPassGoCommand;
        Button loginButton;

        public LoginViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;



        }

        public static LoginFragment NewInstance()
        {
            var fragment = new LoginFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_login, null);

            ViewModel = new LoginViewModel();

            registerGoCommand = view.FindViewById<TextView>(Resource.Id.register_prompt);
            forgotPassGoCommand = view.FindViewById<TextView>(Resource.Id.forgot_password);
            userName = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtUserName);
            password = view.FindViewById<TextInputEditText>(Resource.Id.txtPassword);
            loginButton = view.FindViewById<Button>(Resource.Id.btnConfirm);

            registerGoCommand.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, SignUpFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            loginButton.Click += async (sender, e) =>
            {
                var user = new User
                {
                    Username = userName.Text,
                    Password = password.Text
                };

                await ViewModel.Login(user);

                if (ViewModel.CurrentUser == null)
                    return;

                var intent = new Intent(Activity, typeof(BasicMainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.AddFlags(ActivityFlags.ClearTask);
                StartActivity(intent);
                Activity.Finish();
            };

            return view;
        }

    }
}