using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Service
{
    public interface IAuthService
    {
        void SaveCredentials(string UserName, string Password);
        string UserName { get; }
        string Password { get; }

    }
}
