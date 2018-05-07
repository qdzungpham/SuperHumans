using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Service
{
    public interface IAuthService
    {
        void SaveCredentials(string ServiceName,string UserName, string Password);
        string UserName { get; }
        string Password { get; }
        //void SetServiceName(string name);
    }
}
