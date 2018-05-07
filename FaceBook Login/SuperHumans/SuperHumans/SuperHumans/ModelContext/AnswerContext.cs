using Plugin.RestClient;
using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ModelContext
{
    public class AnswerContext
    {
        //public async Task<Answer> GetAnwerAsync(string ControllerName, string _id)
        //{
        //    RestClient<Answer> restClient = new RestClient<Answer>();
        //    var q = await restClient.GetOneObjAsync(ControllerName, _id);
        //    return q;
        //}

        public async Task<List<Answer>> GetAnswersAsync(string ControllerName,string _id)
        {
            RestClient<Answer> restClient = new RestClient<Answer>();
            var q = await restClient.GetListByIDAsync(ControllerName, _id);
            return q;
        }

        public async Task CreateAnswer(Answer answer, string ControllerName)
        {
            RestClient<Answer> restClient = new RestClient<Answer>();
            var q = await restClient.PostAsync(answer, ControllerName);
        }

        public async Task DeleteAnswer(int id, string ControllerName)
        {
            RestClient<Answer> restClient = new RestClient<Answer>();
            var q = await restClient.DeleteAsync(id, ControllerName);
        }

        public async Task EditAnswer(int id, Answer _a, string ControllerName)
        {
            RestClient<Answer> restClient = new RestClient<Answer>();
            var q = await restClient.PutAsync(id, _a, ControllerName);
        }
    }
}
