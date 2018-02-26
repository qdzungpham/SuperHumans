using SuperHumans.Helpers;
using SuperHumans.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

        private async Task Login(User user)
        {

            try
            {
                await ParseAccess.Login(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERRORR ****************************");
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
