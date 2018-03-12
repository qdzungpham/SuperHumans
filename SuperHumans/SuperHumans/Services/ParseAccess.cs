using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parse;
using SuperHumans.Models;

namespace SuperHumans.Services
{
    public class ParseAccess : IParseAccess
    {
        public async Task<int> CreateObject()
        {
            ParseObject gameScore = new ParseObject("GameScore");
            gameScore["score"] = 1337;
            gameScore["playerName"] = "Sean Plott";
            await gameScore.SaveAsync();
            return 1;
        }

        public bool CurrentUser()
        {
            if (ParseUser.CurrentUser != null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<int> SignUp(User _user)
        {
            var user = new ParseUser()
            {
                Username = _user.Username,
                Password = _user.Password,
                Email = _user.Email
            };

            try
            {
                await user.SignUpAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return 1;
        }

        public async Task<int> Login(User user)
        {
            try
            {
                await ParseUser.LogInAsync(user.Username, user.Password);
            }
            catch (Exception e)
            {
                throw e;
            }

            return 1;
        }

        public async Task<int> SignOut()
        {
            await ParseUser.LogOutAsync();

            return 1;
        }

        public async Task<int> AddQuestion(Question _question)
        {
            ParseObject question = new ParseObject("Questions");
            question["title"] = _question.Title;
            question["body"] = _question.Body;
            question["owner"] = ParseUser.CurrentUser.Username;
            await question.SaveAsync();
            return 1;
        }

        public async Task<IEnumerable<ParseObject>> LoadQuestions()
        {
            var query = ParseObject.GetQuery("Questions").OrderByDescending("updatedAt");
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
            
        }

        public async Task<string> GetServerTime()
        {
            //var result = DateTime.Now;
            string hi = "hi";
            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "User", "rick"}
            };
            await ParseCloud.CallFunctionAsync<string>("hello", null).ContinueWith(t =>
            {
                hi = t.Result;
                System.Diagnostics.Debug.WriteLine(hi);
            });

            return hi;
            
        }
    }
}
