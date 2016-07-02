using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamerProject
{
    public class StateFactory
    {
        private SortedList<string, State> stateList;

        public StateFactory()
        {
            stateList = new SortedList<string, State>();
        }

        public State getState(string key)
        {
            State aState = null;
            if (stateList.ContainsKey(key))
            {
                aState = stateList[key];
            }
            else
            {
                switch (key)
                {
                    case State.WAKE:
                        aState = new WakeState();
                        break;
                    case State.SLEEP:
                        aState = new SleepState();
                        break;
                    case State.CHITCHAT:
                        aState = new ChitchatState();
                        break;
                    default:
                        break;
                }
                stateList.Add(key, aState);
            }
            return aState;
        }
    }
}
