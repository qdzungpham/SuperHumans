using SuperHumans.Helpers;
using SuperHumans.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public Command LoadUsersCommand { get; set; }

        public UsersViewModel()
        {
            Users = new ObservableCollection<User>();
            LoadUsersCommand = new Command(async () => await ExecuteLoadUsersCommandAsync());
        }

        public async Task ExecuteLoadUsersCommandAsync()
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                Users.Clear();
                var users = await ParseAccess.LoadUsers();
                foreach (var user in users)
                {
                    var u = new User
                    {
                        FullName = user.Get<string>("username"),
                        Username = user.Get<string>("username")
                    };
                    Users.Add(u);
                }
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
