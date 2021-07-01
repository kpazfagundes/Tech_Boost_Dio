using System;
using System.Net.WebSockets;

namespace Server.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public bool Logged { get; set; } = false;
        public WebSocket Websocket { get; set; }

        /// <summary>
        /// Configure a GUID to Id
        /// </summary>
        /// <param name="websocket"></param>
        public User(WebSocket websocket)
        {
            Id = Guid.NewGuid().ToString();
            Logged = false;
            Websocket = websocket;
        }

        /// <summary>
        /// Set a nickname to user and set user as logged
        /// </summary>
        /// <param name="nickname"></param>
        public void Login(string nickname)
        {
            this.Nickname = nickname;
            this.Logged = true;
        }
    }
}
