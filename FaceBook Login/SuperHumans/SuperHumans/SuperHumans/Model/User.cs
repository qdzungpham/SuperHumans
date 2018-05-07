using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Model
{
    public class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        //public int UserID { get; set; }
        //public string Email { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }

        //public ICollection<SuperHuman> SuperHumans { get; set; }
    }
}
