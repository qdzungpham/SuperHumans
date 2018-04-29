using Acr.UserDialogs;
using SuperHumans.Helpers;
using SuperHumans.Models;
using SuperHumans.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; private set; }

        public LoginViewModel()
        {
            Title = "Login";
            LoginCommand = new Command<User>(async (User user) => await Login(user));
        }

        public async Task Login(User user)
        {
            try
            {
                IsBusy = true;
                await ParseAccess.Login(user);
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message, "ERROR SIGNING IN", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
