using SuperHumans.Model;
using SuperHumans.ModelContext;
using SuperHumans.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SuperHumans.ViewModel
{
    public class Account : BaseViewModel
    {
        public Account()
        {
            User = new User();
            SubmitCommand = new Command(OnSubmit);
        }
        private User _user;
        public ICommand SubmitCommand { get; set; }

        public User User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
            }
        }
        
        public void OnSubmit()
        {
            if (string.IsNullOrEmpty(User.UserName) || (string.IsNullOrEmpty(User.Password)))
            {
                MessagingCenter.Send(this, "ValidateLogin", User.UserName);
            }
            else
            {
                MessagingCenter.Send(this, "SendMssg", User.UserName);
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }

        public Command RegisterCommand
        {
            get
            {
                return new Command( async() =>
                {
                    Message = "First Step succeeds";
                    var UC = new UsersContext();
                    //var newUser = new User { Email = User.Email, UserName = User.UserName, Password = User.Password, ConfirmPassword = User.ConfirmPassword };
                    
                    bool IsSucessful = await UC.CreateUser(User, "UsersWebAPI");
                    if (IsSucessful)
                    {
                        Message = "Good";
                    }
                    else
                    {
                        Message = "bad";
                    }
                });
            }
        }

        public Command LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Message = "First Step succeeds";
                    var UC = new UsersContext();
                    User.ConfirmPassword = User.Password;
                    bool IsSucessful = await UC.CreateUser(User, "UsersWebAPI?type=Login");
                    if (IsSucessful)
                    {
                        Message = "Good";
                        Application.Current.Properties["UserName"] = User.UserName;
                        Message = Application.Current.Properties["UserName"].ToString();
                    }
                    else
                    {
                        Message = "bad";
                    }
                });
            }
        }


        public Command LogoutCommand
        {
            get
            {
                return new Command(() =>
                {
                    Message = "First Step succeeds";
                    var authService = new AuthService();
                    authService.DeleteCredentials();
                    //Message = result.ToString();
                });
            }
        }
    }
}
