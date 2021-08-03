using System;
using TwitchLib.Communication.Interfaces;

namespace TwitchChatBot_Ver
{
    class Program
    {
        private static string _botName = "bot_name";
        private static string _broadcasterName = "yuzuirotori";
        private static string _twitchOAuth = Properties.Resources.Oauth_Key; // get chat bot's oauth from www.twitchapps.com/tmi/

        static void Main(string[] args)
        {
            // Initialize and connect to Twitch chat
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667,
                            _botName, _twitchOAuth, _broadcasterName);

            // Ping to the server to make sure this bot stays connected to the chat
            // Server will respond back to this bot with a PONG (without quotes):
            // Example: ":tmi.twitch.tv PONG tmi.twitch.tv :irc.twitch.tv"
            PingSender ping = new PingSender(irc);
            ping.Start();
        }
    }
}
