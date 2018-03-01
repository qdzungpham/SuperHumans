using SuperHumans.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperHumans.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        public Command SignOutCommand { get; set; }

        public SettingsViewModel()
        {
            //Title = "";
            SignOutCommand = new Command(async () => await SignOut());
        }

        private async Task SignOut()
        {

            await ParseAccess.SignOut();
        }
    }
}
