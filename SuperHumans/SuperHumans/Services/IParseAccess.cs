﻿using Parse;
using SuperHumans.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.Services
{
    public interface IParseAccess
    {
        ParseUser CurrentUser();
        Task<int> SignUp(User user);
        Task<int> Login(User user);
        Task<int> SignOut();
        Task<int> CreateObject();
        Task<int> AddQuestion(Question question);
        Task<IEnumerable<ParseObject>> LoadQuestions();
        Task<ParseObject> GetQuestion(string objectId);
        Task<int> AddAnswer(Answer answer);
        Task<IEnumerable<ParseObject>> LoadAnswers(ParseObject question);
        Task<IEnumerable<ParseObject>> LoadUsers();
        Task<ParseObject> GetUser(string userId);
        Task<int> UpdateProfile(User user);
        Task<int> UpdateFollowedOppors(IList<string> ids);
        Task<int> UpdateFollowedUsers(List<string> userIds);
        Task<IEnumerable<ParseObject>> LoadFollowedOppors(IList<string> ids);
        Task<IEnumerable<ParseObject>> LoadFollowedUsers(IList<string> ids);
    }
}
