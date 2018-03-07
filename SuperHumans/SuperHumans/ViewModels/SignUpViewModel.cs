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
    public class SignUpViewModel : BaseViewModel
    {
        public Command SignUpCommand { get; private set; }

        public SignUpViewModel()
        {
            Title = "Sign Up";
            SignUpCommand = new Command<User>(async (User user) => await SignUp(user));

        }

        private async Task SignUp(User _user)
        {
            User user = new User()
            {
                Email = _user.Email,
                Username = _user.Username,
                Password = _user.Password
            };
            try
            {
                IsBusy = true;
                await ParseAccess.SignUp(user);
                IsBusy = false;
            } catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message, "ERROR SIGNING UP", "OK");
            }
        }
    }
}
