using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.Repository
{
    public interface QuestionRepository
    {
        Task<IEnumerable<Question>> GetQuestionsAsync();

        Task<Question> GetQuestionByIdAsync(int id);

        Task<bool> AddQuestionAsync(Question product);

        Task<bool> UpdateQuestionAsync(Question product);

        Task<bool> RemoveQuestionAsync(int id);

        Task<IEnumerable<Question>> QueryProductsAsync(Func<Question, bool> predicate);
    }

    public class QuestionsRepository : QuestionRepository
    {
        public Task<bool> AddQuestionAsync(Question product)
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetQuestionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> QueryProductsAsync(Func<Question, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveQuestionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateQuestionAsync(Question product)
        {
            throw new NotImplementedException();
        }
    }
}
