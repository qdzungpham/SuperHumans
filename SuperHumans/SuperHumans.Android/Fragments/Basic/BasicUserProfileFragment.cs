using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SuperHumans.Models;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicUserProfileFragment : Android.Support.V4.App.Fragment
    {
        TextView fullName, userName;
        Button messageBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;
        }

        public static BasicUserProfileFragment NewInstance(string user)
        {
            var bundle = new Bundle();
            bundle.PutString("userData", user);
            var fragment = new BasicUserProfileFragment { Arguments = bundle };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_user_profile, null);

            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(Arguments.GetString("userData"));
            Activity.Title = "User Profile";

            fullName = view.FindViewById<TextView>(Resource.Id.text_full_name);
            userName = view.FindViewById<TextView>(Resource.Id.text_username);
            messageBtn = view.FindViewById<Button>(Resource.Id.btn_message);

            fullName.Text = user.FirstName + " " + user.LastName;
            userName.Text = "@" + user.Username;

            messageBtn.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicMessageFragment.NewInstance(Newtonsoft.Json.JsonConvert.SerializeObject(user)))
                .AddToBackStack(null).Commit();
            };

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.speak_menu, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_speak:

                    ((Activities.Basic.BasicMainActivity)Activity).Speak("Hello, this is a test.");

                    break;


            }
            return base.OnOptionsItemSelected(item);
        }
    }
}