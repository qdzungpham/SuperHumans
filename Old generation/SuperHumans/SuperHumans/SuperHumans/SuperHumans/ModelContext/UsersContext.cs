using Plugin.RestClient;
using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ModelContext
{
    public class UsersContext
    {
        public async Task<bool> CreateUser(User user, string ControllerName)
        {
            RestClient<User> restClient = new RestClient<User>();
            var q = await restClient.PostAsync(user, ControllerName);
            return q;
        }

    }
}
