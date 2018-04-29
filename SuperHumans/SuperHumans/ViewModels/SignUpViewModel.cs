using Acr.UserDialogs;
using Parse;
using SuperHumans.Helpers;
using SuperHumans.Models;
using SuperHumans.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public Command SignUpCommand { get; private set; }
        public Command UpdateInfoCommand { get; private set; }

        public SignUpViewModel()
        {
            Title = "Sign Up";
            
     
            SignUpCommand = new Command<User>(async (User user) => await SignUp(user));
            UpdateInfoCommand = new Command<User>(async (User user) => await UpdateInfo(user));

        }

        public async Task SignUp(User user)
        {
            try
            {
                await ParseAccess.SignUp(user);
                IsAsyncFinished = true;
            } catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message, "ERROR SIGNING UP", "OK");
            }
        }

        public async Task UpdateInfo(User user)
        {
            try
            {
                await ParseAccess.UpdateProfile(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

      
    }
}
