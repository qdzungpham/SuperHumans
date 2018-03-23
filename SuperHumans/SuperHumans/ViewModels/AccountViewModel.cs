using SuperHumans.Helpers;
using SuperHumans.Models;
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

        public AccountViewModel()
        {
            
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
    }
}
