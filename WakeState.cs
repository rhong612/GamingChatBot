using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginSDK;

namespace GamerProject
{
    public class WakeState : State
    {
        private int counter;
        private const int THRESHOLD = 2; // 4 seconds

        public WakeState() {
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
        }
    }
}
