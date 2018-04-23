using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Model
{

    public enum Major
    {
        Health,
        Food,
        Cooking,
        Gardening,
        Writing,
        Drivers_Education,
        Technology,
        Math,
        Coffe
    }


    public class SuperHuman
    {
        public int SuperHumanID { get; set; }
        public Major Major { get; set; }
        public int Rating { get; set; }
        public string SelfIntro { get; set; }

        public int UserID { get; set; }
        //public User User { get; set; }
    }
}
