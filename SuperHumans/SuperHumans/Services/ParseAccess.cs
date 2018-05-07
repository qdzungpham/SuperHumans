using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parse;
using SuperHumans.Models;

namespace SuperHumans.Services
{
    public static class ParseAccess 
    {



        public static async Task<int> CreateObject()
        {
            ParseObject gameScore = new ParseObject("GameScore");
            gameScore["score"] = 1337;
            gameScore["playerName"] = "Sean Plott";
            
            await gameScore.SaveAsync();
            return 1;
        }

        public static ParseUser CurrentUser()
        {
            return ParseUser.CurrentUser;
        }

        public static async Task<int> SignUp(User _user)
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

        public static async Task<int> Login(User user)
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

        public static async Task<int> SignOut()
        {
            await ParseUser.LogOutAsync();

            return 1;
        }

        public static async Task<int> AddQuestion(Question _question)
        {
            ParseObject question = new ParseObject("Questions");
            question["title"] = _question.Title;
            question["body"] = _question.Body;
            question["createdBy"] = ParseUser.CurrentUser;
            await question.SaveAsync();
            return 1;
        }

        public static async Task<IEnumerable<ParseObject>> LoadQuestions()
        {
            var query = ParseObject.GetQuery("Questions").OrderByDescending("updatedAt");
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
            
        }


        public static async Task<ParseObject> GetQuestion(string objectId)
        {
            ParseQuery<ParseObject> query = ParseObject.GetQuery("Questions").Include("topics").Include("createdBy");
            ParseObject question = await query.GetAsync(objectId);
            return question;
        }

        public static async Task<int> AddAnswer(Answer _answer)
        {
            ParseQuery<ParseObject> questionQuery = ParseObject.GetQuery("Questions");

            ParseObject answer = new ParseObject("Answers");
            answer["body"] = _answer.Body;
            answer["createdBy"] = ParseUser.CurrentUser;
            answer["isAnswerOf"] = await questionQuery.GetAsync(_answer.QuestionId);
            await answer.SaveAsync();
            return 1;
        }

        public static async Task<IEnumerable<ParseObject>> LoadAnswers(ParseObject question)
        {
            var query = ParseObject.GetQuery("Answers").WhereEqualTo("isAnswerOf", question);
            query = query.Include("createdBy");
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
        }

        public static async Task<IEnumerable<ParseObject>> LoadUsers()
        {
            var query = ParseUser.Query;
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
        }

        public static async Task<ParseObject> GetUser(string userId)
        {
            var query = ParseUser.Query;
            return await query.GetAsync(userId);
        }

        public static async Task<int> UpdateProfile(User user)
        {

            ParseUser.CurrentUser["lastName"] = user.LastName;
            ParseUser.CurrentUser["firstName"] = user.FirstName;
            ParseUser.CurrentUser["postcode"] = user.Postcode;

            await ParseUser.CurrentUser.SaveAsync();

            return 1;
        }

        public static async Task<int> UpdateFollowedOppors(IList<string> opporIds)
        {
            ParseUser.CurrentUser["followedOppors"] = opporIds;

            await ParseUser.CurrentUser.SaveAsync();

            return 1;
        }

        public static async Task<int> UpdateFollowedUsers(List<string> userIds)
        {
            ParseUser.CurrentUser["followedUsers"] = userIds;

            await ParseUser.CurrentUser.SaveAsync();

            return 1;
        }

        public static async Task<IEnumerable<ParseObject>> LoadFollowedOppors(IList<string> ids)
        {
            var query = ParseObject.GetQuery("Questions").OrderByDescending("updatedAt").WhereContainedIn("objectId", ids);
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;


        }

        public static async Task<IEnumerable<ParseObject>> LoadFollowedUsers(IList<string> ids)
        {
            var query = ParseUser.Query.WhereContainedIn("objectId", ids);
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;

            //var query = ParseObject.GetQuery("Users").OrderByDescending("updatedAt").WhereContainedIn("objectId", ids);
            //IEnumerable<ParseObject> results = await query.FindAsync();
            //return results;
        }

        public static async Task<IEnumerable<ParseObject>> LoadTopics()
        {
            var query = ParseObject.GetQuery("Topic");
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;
        }

        public static async Task<int> UpdateFollowedTopics(List<ParseObject> topicIds)
        {
            ParseUser.CurrentUser["followedTopics"] = topicIds;

            await ParseUser.CurrentUser.SaveAsync();

            return 1;
        }

        public static async Task<IEnumerable<ParseObject>> LoadOpporsBasedOnTopics()
        {
            var followedTopics = ParseUser.CurrentUser.Get<IList<ParseObject>>("followedTopics");
            var query = ParseObject.GetQuery("Questions").OrderByDescending("updatedAt").WhereContainedIn("topics", followedTopics);
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;

        }


        public static async Task<string> GetConversationId(string otherUserId)
        {
            var members = new List<string>
            {
                otherUserId,
                ParseUser.CurrentUser.ObjectId
            };
            var query = ParseObject.GetQuery("Conversation").WhereContainsAll("members", members);

            ParseObject conversation = await query.FirstOrDefaultAsync();

            if (conversation != null)
            {
                return conversation.ObjectId;
            }
            else
            {
                ParseObject newConversation = new ParseObject("Conversation");
                newConversation["members"] = members;
                await newConversation.SaveAsync();

                return newConversation.ObjectId;
            }
        }

        public static async Task<int> SendMessage(string conversationId, string body)
        {
            ParseObject message = new ParseObject("Message");
            message["conversationId"] = conversationId;
            message["author"] = ParseUser.CurrentUser.ObjectId;
            message["body"] = body;

            await message.SaveAsync();
            return 1;
        }

        public static async Task<IEnumerable<ParseObject>> LoadMessages(string conversationId)
        {

            var query = ParseObject.GetQuery("Message").OrderBy("updatedAt").WhereEqualTo("conversationId", conversationId);
            IEnumerable<ParseObject> results = await query.FindAsync();
            return results;

        }

    }
}
