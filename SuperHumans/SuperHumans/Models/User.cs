﻿

using System.Collections.Generic;

namespace SuperHumans.Models
{
    public class User
    {
        public string ObjectId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Postcode { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public List<string> FollowedTopics { get; set; }
        public bool IsFollowed { get; set; }
    }
}
