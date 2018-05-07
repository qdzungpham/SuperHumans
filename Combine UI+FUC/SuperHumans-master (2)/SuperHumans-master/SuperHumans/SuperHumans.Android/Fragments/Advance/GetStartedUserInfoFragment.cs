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
    public class GetStartedUserInfoFragment: Fragment
    {
        AutoCompleteTextView firstName, lastName, postCode;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;
        }

        public static GetStartedUserInfoFragment NewInstance()
        {
            var fragment = new GetStartedUserInfoFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_personal_info, null);

            firstName = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtFirstName);
            lastName = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtLastName);
            postCode = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtPostcode);


            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.get_started_user_info_menu, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_next:
                    var user = new User
                    {
                        FirstName = firstName.Text,
                        LastName = lastName.Text,
                        Postcode = postCode.Text
                    };

                    SignUpFragment.ViewModel.UpdateInfoCommand.Execute(user);

                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, GetStaredChooseTopicsFragment.NewInstance())
                    .AddToBackStack(null).Commit();

                    break;



            }
            return base.OnOptionsItemSelected(item);
        }
    }
}