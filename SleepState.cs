using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginSDK;

namespace GamerProject
{
    public class SleepState : State
    {
        private int counter;
        private const int THRESHOLD = 2; //4 seconds

        public SleepState()
        {
            counter = 0;
        }

        public override void PerformAction(Gamer gamer)
        {
            counter = ++counter % THRESHOLD;
            if (counter == 0)
            {
                gamer.Speak(gamer.getAIMLResponse("SayStatement I'm awake now", gamer.myUser));
                gamer.NextState = State.GetState(State.WAKE);
                gamer.setChatStatus(ChatStatus.Available);
            }
        }
    }
}
