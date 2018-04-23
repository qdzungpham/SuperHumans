using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;

namespace SuperHumans.Service
{
    public class AuthService : IAuthService
    {
        private string ServiceName;
        private List<string> ServiceNames;

        public AuthService()
        {
            ServiceNames = new List<String> { "Local","FaceBook","Google" };
        }

        //public AuthService(string name)
        //{
        //    ServiceName = name;
        //}

        //public void SetServiceName(string name)
        //{
        //    ServiceName = name;
        //}

        public string Password
        {
            get
            {

                var account = AccountStore.Create().FindAccountsForService(ServiceName).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }
        public string UserName
        {
            get
            {
                foreach(var name in ServiceNames)
                {
                    var account = AccountStore.Create().FindAccountsForService(name).FirstOrDefault();
                    
                    if(account != null)
                    {
                        ServiceName = name;
                        return account.Username;
                    }
                    //return (account != null) ? account.Username : null;
                }
                return null;
            }
        }
        public void SaveCredentials(string ServiceName, string UserName, string Password)
        {
            DeleteCredentials();

            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                Account account = new Account
                {
                    Username = UserName
                };
                account.Properties.Add("Password", Password);
                AccountStore.Create().Save(account, ServiceName);
                this.ServiceName = ServiceName;
                
            }
        }

        public void DeleteCredentials()
        {
            foreach (var name in ServiceNames)
            {
                var account = AccountStore.Create().FindAccountsForService(name).FirstOrDefault();
                if (account != null)
                {
                    AccountStore.Create().Delete(account, name);
                }
            }
        }

    }
}
