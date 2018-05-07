//using Plugin.AutoLogin;
using SuperHumans.ModelContext;
using SuperHumans.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SuperHumans.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public SuperHumans.ViewModel.Account ACCVM;

        public Login()
        {
            InitializeComponent();
            ACCVM = new SuperHumans.ViewModel.Account();
            this.BindingContext = new SuperHumans.ViewModel.Account();

            MessagingCenter.Subscribe<SuperHumans.ViewModel.Account, string>(this, "ErrorMssg", (sender, username) =>
             {
                 DisplayAlert("Title", "Username/Password should be required. Please Input it.", "OK");
             });
            MessagingCenter.Subscribe<SuperHumans.ViewModel.Account, string>(this, "CorrectMssg", async(sender, username) =>
            {
                sender.IsBusy = true;
                //sender.Message = "First Step succeeds";
                var UC = new UsersContext();
                sender.User.ConfirmPassword = sender.User.Password;
                bool IsSucessful = await UC.CreateUser(sender.User, "UsersWebAPI?type=Login");
                if (IsSucessful)
                {
                    //sender.Message = "Good";                    
                    var AuthService = DependencyService.Get<IAuthService>();
                    AuthService.SaveCredentials("Local",UserNameEntry.Text, PasswordEntry.Text);
                    Application.Current.Properties["UserName"] = sender.User.UserName; //sender.Message = Application.Current.Properties["UserName"].ToString();
                    await Navigation.PushAsync(new MainNavi());
                }
                else
                {
                    //sender.Message = "bad";
                    await DisplayAlert("Title", "Your input is probably wrong. Please Input it again.", "OK");
                    await Navigation.PushAsync(new Login());
                }
                sender.IsBusy = false;
            });

            UserNameEntry.Completed += (object sender, EventArgs e) =>
            {
                PasswordEntry.Focus();
            };
            PasswordEntry.Completed += (object sender, EventArgs e) =>
            {
                ACCVM.SubmitCommand.Execute(null);
            };
        }

        private async void RegisterButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewUsersRegister());
        }
        
        //private void SubmitButton(object sender, EventArgs e)
        //{
        //    var AuthService = DependencyService.Get<IAuthService>();
        //    AuthService.SaveCredentials(UserNameEntry.Text, PasswordEntry.Text);            
        //}
        
        private async void FaceBookButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FacebookProfilePage());
        }
        
        private async void GoogleButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoogleProfilePage());
        }
        //private async void LoginButton(object sender, EventArgs e)
        //{
        //    // Login Button....
        //    var ACCVM = BindingContext as SuperHumans.ViewModel.Account;
        //    if (ACCVM != null)
        //    {
        //        ACCVM.Message = "First Step succeeds";
        //        var UC = new UsersContext();
        //        ACCVM.User.ConfirmPassword = ACCVM.User.Password;

        //        ACCVM.IsBusy = true;
        //        bool IsSucessful = await UC.CreateUser(ACCVM.User, "UsersWebAPI?type=Login");
        //        if (IsSucessful)
        //        {
        //            ACCVM.Message = "Good";
        //            Application.Current.Properties["UserName"] = ACCVM.User.UserName;
        //            ACCVM.Message = Application.Current.Properties["UserName"].ToString();

        //            await Navigation.PushAsync(new MainNavi());
        //        }
        //        else
        //        {
        //            ACCVM.Message = "bad";
        //            await Navigation.PushAsync(new NewUsersRegister());
        //        }
        //        ACCVM.IsBusy = false;
        //    }
        //}
    }
}