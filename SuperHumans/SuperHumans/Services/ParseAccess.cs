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
            question["ownerId"] = ParseUser.CurrentUser.ObjectId;
            await question.SaveAsync();
            return 1;
        }
    }
}
