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
using SuperHumans.Droid.Fragments;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Activities
{
    [Activity(Label = "My Profile")]
    public class MyProfileActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.profile_activity;

        TextView firstName, lastName;

        LinearLayout firstNameRow, lastNameRow, aboutMeRow;

        AccountViewModel ViewModel { get; set; }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = AccountFragment.ViewModel;

            firstName = FindViewById<TextView>(Resource.Id.first_name);
            lastName = FindViewById<TextView>(Resource.Id.last_name);

            firstName.Text = ViewModel.UserDetail.FirstName;
            lastName.Text = ViewModel.UserDetail.LastName;

            firstNameRow = FindViewById<LinearLayout>(Resource.Id.first_name_row);
            lastNameRow = FindViewById<LinearLayout>(Resource.Id.last_name_row);
            aboutMeRow = FindViewById<LinearLayout>(Resource.Id.about_me_row);

            firstNameRow.Click += async (sender, e) => 
            {
                await ViewModel.ExecuteClickFirstNameCommandAsync();
                firstName.Text = ViewModel.UserDetail.FirstName;
            };

            lastNameRow.Click += async (sender, e) =>
            {
                await ViewModel.ExecuteClickLastNameCommandAsync();
                lastName.Text = ViewModel.UserDetail.LastName;
            };

            aboutMeRow.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Clicked.", ToastLength.Short).Show();
            };

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.my_profile_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_save:
                    ViewModel.SaveProfileCommand.Execute(null);


                    break;


            }
            return base.OnOptionsItemSelected(item);
        }
    }
}