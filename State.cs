using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamerProject
{
    public abstract class State
    {
        public const string WAKE = "wake";
        public const string SLEEP = "sleep";
        public const string CHITCHAT = "chitchat";

        protected static StateFactory factory = new StateFactory();

        public static State GetState(string key)
        {
            return factory.getState(key);
        }

        public abstract void PerformAction(Gamer gamer);
    }
}
