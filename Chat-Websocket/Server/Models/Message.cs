namespace Server.Models
{
    public class Message
    {
        //Comandos utilizados para comunicação com o Client
        public const string Login = "/l";
        public const string MessageToRoom = "/mr";
        public const string MessagePrivate = "/mp";
        public const string EnteredRoom = "/er";
        public const string ExitedRoom = "/xr";
        public const string ReceiveMessage = "/rm";
        public const string ReceiveMessagePrivate = "/rmp";

        //Comandos de erro
        public const string LoginError = "/lerr";
        public const string MentionError = "/merr";

        public string Command { get; set; }
        public string Content { get; set; }
        public string ToNickname { get; set; }

        /// <summary>
        /// Faz o parse de uma string para o formato de Message
        /// </summary>
        /// <param name="text"></param>
        public Message(string text)
        {
            this.Command = text.Substring(0, text.IndexOf(' '));
            this.Content = text.Substring(text.IndexOf(' ') + 1);

            if(this.Content.Contains("@"))
            {
                this.ToNickname = this.Content.Substring(0, this.Content.IndexOf(' ')).Replace("@","");
                this.Content = this.Content.Substring(this.Content.IndexOf(' ') + 1);
            }
        }
    }}
