using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Advance
{
    public class UserInfoFragment: Fragment
    {
        AutoCompleteTextView firstName, lastName, postCode;
        Button nextBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;
        }

        public static UserInfoFragment NewInstance()
        {
            var fragment = new UserInfoFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_personal_info, null);

            firstName = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtFirstName);
            lastName = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtLastName);
            postCode = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtPostcode);
            nextBtn = view.FindViewById<Button>(Resource.Id.btnNext);

            nextBtn.Click += async (sender, e) =>
            {
                var user = new User
                {
                    FirstName = firstName.Text,
                    LastName = lastName.Text,
                    Postcode = postCode.Text
                };

                await SignUpFragment.ViewModel.UpdateInfo(user);

                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, ChooseTopicsFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            return view;
        }
    }
}