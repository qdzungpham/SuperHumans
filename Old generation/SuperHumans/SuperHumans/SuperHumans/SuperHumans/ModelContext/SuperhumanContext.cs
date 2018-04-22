using Plugin.RestClient;
using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ModelContext
{
    public class SuperhumanContext
    {
        public async Task<List<SuperHuman>> GetSuperHumansAsync(string ControllerName)
        {
            RestClient<SuperHuman> restClient = new RestClient<SuperHuman>();
            var s = await restClient.GetAsync(ControllerName);
            return s;
        }

        //public async Task CreateQuestion(Question question, string ControllerName)
        //{
        //    RestClient<Question> restClient = new RestClient<Question>();
        //    var q = await restClient.PostAsync(question, ControllerName);
        //}

        //public async Task DeleteQuestion(int id, string ControllerName)
        //{
        //    RestClient<Question> restClient = new RestClient<Question>();
        //    var q = await restClient.DeleteAsync(id, ControllerName);
        //}

        //public async Task EditQuestion(int id, Question _q, string ControllerName)
        //{
        //    RestClient<Question> restClient = new RestClient<Question>();
        //    var q = await restClient.PutAsync(id, _q, ControllerName);
        //}
    }
}
