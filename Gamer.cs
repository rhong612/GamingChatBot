using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Drawing;
using System.Windows;
using System.ComponentModel;
using AIMLbot;
using PluginSDK;

namespace GamerProject
{
    public enum BotStatus {  Available, Sleep };
    public class Gamer : Bot, IFollowerPlugin
    {
        private Timer stateTicker;
        private SortedList<string, UserAccount> accounts;

        public SortedList<string, User> Users { get; private set; }
        public User myUser { get; private set; }

        

        class ConcreteProfile : IProfile
        {
            public ConcreteProfile()
            {
                Name = "Rob";
            }

            public string Name
            {
                get;
                private set;
            }
        }

        public Gamer()
        {
            myUser = new User("Some id", this);

            Users = new SortedList<string, User>();
            OutgoingMessage = new Queue<ChatMessage>();
            accounts = new SortedList<string, UserAccount>();
            CurrentState = State.GetState(State.WAKE);
            NextState = State.GetState(State.WAKE);
            AuthorName = "Raymond Hong";
            PluginName = "Gamer Bot";
            PluginVersion = "1.0.0";
            FontColor = Color.Blue;
            Nickname = "Random nickname 2000";
            Profile = new ConcreteProfile();
            Role = "Gamer";


            stateTicker = new Timer();
            stateTicker.Interval = 2000; // 2 seconds
            stateTicker.Elapsed += new ElapsedEventHandler(stateTimer_Elapsed);

            Sources = new SortedList<string, IChatSource>();
            initialize();
        }

        public SortedList<string, IChatSource> Sources { get; private set; }

        public IChatSource PSource { get; private set; }

        public void initialize()
        {
            try
            {
                string path = Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "GamerSettings.xml"));
                loadSettings(path);
                isAcceptingUserInput = false;
                loadAIMLFromFiles();
                isAcceptingUserInput = true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("file not found exception was thrown in Gamer.cs");
            }
        }

        private void stateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CurrentState = NextState;
            CurrentState.PerformAction(this);
        }

        public String getAIMLResponse(String input, User user)
        {
            Request r = new Request(input, user, this);
            Result res = Chat(r);
            return res.Output;
        }

        public State CurrentState { get; private set; }

        public void setChatStatus(ChatStatus status)
        {
            foreach (UserAccount account in accounts.Values)
            {
                account.Status = status;
            }
        }

        public State NextState
        {
            get;
            set;
        }


        public void Speak(string statement)
        {
            lock (OutgoingMessage)
            {
                OutgoingMessage.Enqueue(new ChatMessage(PSource, retrieve(PSource)[0], PSource.MasterAccount, new TextMessage(statement)));
            }
        }

        public string AuthorName { get; private set; }

        public string PluginName { get; private set; }

        public string PluginVersion { get; private set; }



        public string Nickname
        {
            get;
            private set;
        }

        public string Role
        {
            get;
            private set;
        }

        public Queue<ChatMessage> OutgoingMessage
        {
            get;
            private set;
        }

        public void start()
        {
            stateTicker.Start();
        }

        public void listen(ChatMessage message)
        {
            //Speak(getAIMLResponse(message.Content.Message, myUser));
            string response;

            //New Chat Sender?
            if (message.Sender.Owner != null)
            {
                if (!Users.ContainsKey(message.Sender.Owner.Profile.Name))
                    Users.Add(message.Sender.Owner.Profile.Name, new User(message.Sender.Owner.Profile.Name, this));
                response = getAIMLResponse(message.Content.Message, Users[message.Sender.Owner.Profile.Name]);
            }
            else
            {
                if (!Users.ContainsKey(message.Sender.Username))
                    Users.Add(message.Sender.Username, new User(message.Sender.Username, this));
                response = getAIMLResponse(message.Content.Message, Users[message.Sender.Username]);
            }

            if (!response.Equals(""))
            {
                lock (OutgoingMessage)
                {
                    OutgoingMessage.Enqueue(new ChatMessage(message.Source, message.Recipient, message.Sender, new TextMessage(response)));
                }
            }
        }

        public void receive(ChatMessage message)
        {
                string response;

                //New Chat Sender?
                if (message.Sender.Owner != null)
                {
                    if (!Users.ContainsKey(message.Sender.Owner.Profile.Name))
                        Users.Add(message.Sender.Owner.Profile.Name, new User(message.Sender.Owner.Profile.Name, this));
                    response = getAIMLResponse(message.Content.Message, Users[message.Sender.Owner.Profile.Name]);
                }
                else
                {
                    if (!Users.ContainsKey(message.Sender.Username))
                        Users.Add(message.Sender.Username, new User(message.Sender.Username, this));
                    response = getAIMLResponse(message.Content.Message, Users[message.Sender.Username]);
                }

                if (!response.Equals(""))
                {
                    lock (OutgoingMessage)
                    {
                        OutgoingMessage.Enqueue(new ChatMessage(message.Source, message.Recipient, message.Sender, new TextMessage(response)));
                    }
                }
            
        }

        public void join(IChatSource source)
        {
            if (!Sources.ContainsKey(source.SourceName))
            {
                if (PSource == null)
                    PSource = source;
                Sources.Add(source.SourceName, source);
            }
             
        }

        public Color FontColor
        {
            get;
            private set;
        }

        public IProfile Profile
        {
            get;
            private set;
        }

        public void add(UserAccount account)
        {
            accounts.Add(account.SourceName, account);
        }

        public IList<UserAccount> retrieve(IChatSource source)
        {
            List<UserAccount> list = new List<UserAccount>();
            list.Add(accounts[source.SourceName]);
            return list;
        }

        public void loadDatabase()
        {
            //TODO
        }
    }
}
