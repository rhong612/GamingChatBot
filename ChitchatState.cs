using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginSDK;

namespace GamerProject
{
    public class ChitchatState : State
    {
        private int counter;
        private const int THRESHOLD = 10; //20 seconds

        public ChitchatState()
        {
            counter = 0;
        }

        public override void PerformAction(Gamer gamer)
        {
            counter = ++counter % THRESHOLD;
            if (counter == 0)
            {
                gamer.Speak(gamer.getAIMLResponse("SayStatementPattern sleepy", gamer.myUser));
                gamer.NextState = State.GetState(State.SLEEP);
                gamer.setChatStatus(ChatStatus.Offline);
            }
            else if (counter == 1)
            {
                gamer.Speak(gamer.getAIMLResponse("SayStatement Do you want to play a game?", gamer.myUser));
            }
        }
    }
}
