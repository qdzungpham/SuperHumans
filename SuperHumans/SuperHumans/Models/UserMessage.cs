using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Models
{
    public class UserMessage : IEquatable<UserMessage>
    {
        public string ObjectId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Sender { get; set; }
        public bool IsMe { get; set; }


        public bool Equals(UserMessage other)
        {
            if (other == null)
                return false;

            if (this.ObjectId == other.ObjectId)
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            UserMessage messageObj = obj as UserMessage;
            if (messageObj == null)
                return false;
            else
                return Equals(messageObj);
        }

        public override int GetHashCode()
        {
            return this.ObjectId.GetHashCode();
        }
    }
}
