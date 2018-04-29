using Acr.UserDialogs;
using SuperHumans.Helpers;
using SuperHumans.Models;
using SuperHumans.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public User UserDetail { get; set; }

        public Command SaveProfileCommand { get; set; }

        public AccountViewModel()
        {
            SaveProfileCommand = new Command(async () => await ExecuteSaveProfileCommandAsync());
        }

        public async Task ExecuteGetUserCommandAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                var user = await ParseAccess.GetUser(ParseAccess.CurrentUser().ObjectId);
                UserDetail = new User
                {
                    Username = user.Get<string>("username"),
                    FirstName = user.Get<string>("firstName"),
                    LastName = user.Get<string>("lastName")
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
                ProgressDialogManager.DisposeProgressDialog();
            }
        }

        public async Task ExecuteClickFirstNameCommandAsync()
        {
            var config = new PromptConfig
            {
                Text = UserDetail.FirstName,
                Title = "First name"
            };
            PromptResult result = await UserDialogs.Instance.PromptAsync(config);
            UserDetail.FirstName = result.Text;
        }

        public async Task ExecuteClickLastNameCommandAsync()
        {
            var config = new PromptConfig
            {
                Text = UserDetail.LastName,
                Title = "Last name"
            };
            PromptResult result = await UserDialogs.Instance.PromptAsync(config);
            UserDetail.LastName = result.Text;
        }

        public async Task ExecuteSaveProfileCommandAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                await ParseAccess.UpdateProfile(UserDetail);
                UserDialogs.Instance.Toast("Profile updated.", TimeSpan.FromSeconds(3));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
                ProgressDialogManager.DisposeProgressDialog();
            }
        }
    }
}
