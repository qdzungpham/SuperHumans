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

        public Command SignOutCommand { get; private set; }


        public SettingsViewModel()
        {
            //Title = "";
            SignOutCommand = new Command(async () => await SignOut());
        }

        private async Task SignOut()
        {

            await ParseAccess.SignOut();
        }

        public void SwitchToBasicUI()
        {
            Settings.UIMode = "basic";
        }

        public void SwitchToAdvancedUI()
        {
            Settings.UIMode = "advanced";
        }
    }
}
