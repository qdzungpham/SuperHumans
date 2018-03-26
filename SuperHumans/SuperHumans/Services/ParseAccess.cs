﻿using System;
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

        public ParseUser CurrentUser()
        {
            return ParseUser.CurrentUser;
        }

        public async Task<int> SignUp(User _user)
        {
            var user = new ParseUser()
            {
                Username = _user.Username,
                Password = _user.Password,
                Email = _user.Email
            };
            user["firstName"] = "";
            user["lastName"] = "";

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
            question["createdBy"] = ParseUser.CurrentUser;
            await question.SaveAsync();
            return 1;
        }

        public async Task<IEnumerable<ParseObject>> LoadQuestions()
        {
            var query = ParseObject.GetQuery("Questions").OrderByDescending("updatedAt");
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
            
        }

        public async Task<ParseObject> GetQuestion(string objectId)
        {
            ParseQuery<ParseObject> query = ParseObject.GetQuery("Questions");
            ParseObject question = await query.GetAsync(objectId);
            return question;
        }

        public async Task<int> AddAnswer(Answer _answer)
        {
            ParseQuery<ParseObject> questionQuery = ParseObject.GetQuery("Questions");

            ParseObject answer = new ParseObject("Answers");
            answer["body"] = _answer.Body;
            answer["createdBy"] = ParseUser.CurrentUser;
            answer["isAnswerOf"] = await questionQuery.GetAsync(_answer.QuestionId);
            await answer.SaveAsync();
            return 1;
        }

        public async Task<IEnumerable<ParseObject>> LoadAnswers(ParseObject question)
        {
            var query = ParseObject.GetQuery("Answers").WhereEqualTo("isAnswerOf", question);
            query = query.Include("createdBy");
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
        }

        public async Task<IEnumerable<ParseObject>> LoadUsers()
        {
            var query = ParseUser.Query;
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
        }

        public async Task<ParseObject> GetUser(string userId)
        {
            var query = ParseUser.Query;
            return await query.GetAsync(userId);
        }

        public async Task<int> UpdateProfile(User user)
        {
            var u = ParseUser.CurrentUser;

            u["lastName"] = user.LastName;
            u["firstName"] = user.FirstName;

            await u.SaveAsync();

            return 1;
        }
    }
}
