using Plugin.RestClient;
using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ModelContext
{
    public class QuestionContext
    {
        public async Task<List<Question>> GetQuestionsAsync(string ControllerName)
        {
            RestClient<Question> restClient = new RestClient<Question>();
            var q = await restClient.GetAsync(ControllerName);
            return q;
        }

        public async Task CreateQuestion(Question question, string ControllerName)
        {
            RestClient<Question> restClient = new RestClient<Question>();
            var q = await restClient.PostAsync(question, ControllerName);
        }

        public async Task DeleteQuestion(int id, string ControllerName)
        {
            RestClient<Question> restClient = new RestClient<Question>();
            var q = await restClient.DeleteAsync(id, ControllerName);
        }

        public async Task EditQuestion(int id, Question _q, string ControllerName)
        {
            RestClient<Question> restClient = new RestClient<Question>();
            var q = await restClient.PutAsync(id, _q, ControllerName);
        }

    }
}
