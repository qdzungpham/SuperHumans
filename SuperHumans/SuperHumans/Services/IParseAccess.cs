using SuperHumans.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.Services
{
    public interface IParseAccess
    {
        bool CurrentUser();
        Task<int> SignUp(User user);
        Task<int> CreateObject();
    }
}
