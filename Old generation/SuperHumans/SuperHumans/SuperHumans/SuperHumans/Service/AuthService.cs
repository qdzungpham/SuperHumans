using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;

namespace SuperHumans.Service
{
    public class AuthService : IAuthService
    {
        public string Password
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService("AutoLogin").FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }
        public string UserName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService("AutoLogin").FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }
        public void SaveCredentials(string UserName, string Password)
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                Account account = new Account
                {
                    Username = UserName
                };
                account.Properties.Add("Password", Password);
                AccountStore.Create().Save(account, "AutoLogin");
                
            }
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService("AutoLogin").FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, "AutoLogin");
            }
        }

    }
}
