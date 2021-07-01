using Server.Models;
using Server.Services;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketManager;

namespace Server
{
    public class ChatHandler : WebSocketHandler
    {
        //Recebe por injeção a instância do UserService
        public ChatHandler(UserService service) : base(service){ }

        /// <summary>
        /// Listenner para receber novas conexões
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
        }

        /// <summary>
        /// Listener para desconxão
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override async Task OnDisconnected(WebSocket socket)
        {
            var user = Service.Get(socket);
            if (user.Logged)
            {
                await SendMessageToAllAsync($"{Message.ExitedRoom} {user.Nickname} saiu da sala.");
            }
            await base.OnDisconnected(socket);
        }

        /// <summary>
        /// Listener para receber mensagens de um client
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            //Obter o usuário do socket
            var user = Service.Get(socket);

            //Parse da mensagem para identificar o comando e o conteúdo
            var message = new Message(Encoding.UTF8.GetString(buffer, 0, result.Count));

            User toUser = null;
            if (message.ToNickname != null)
                toUser = Service.Get(message.ToNickname);

            //Tratar a ação com base no comando. Consultar class Message para obter a lista de comandos
            switch (message.Command)
            {
                case Message.Login:
                    if(Service.Login(user.Id, message.Content))
                    {
                        await SendMessageToAllAsync($"{Message.EnteredRoom} {message.Content} entrou na sala #geral.");
                    }
                    else
                    {
                        await SendMessageToOneAsync($"{Message.LoginError} Nome de usuário já existe.", socket);
                    }
                    break;
                case Message.MessageToRoom:
                    if (!string.IsNullOrEmpty(message.ToNickname))
                    {
                        if (toUser == null)
                        {
                            await SendMessageToOneAsync($"{Message.MentionError} {message.ToNickname} não encontrado na sala.", socket);
                        }
                        else
                        {
                            await SendMessageToAllAsync($"{Message.ReceiveMessage} {user.Nickname} diz para {message.ToNickname}: {message.Content}");
                        }
                    }
                    else
                    {
                        await SendMessageToAllAsync($"{Message.ReceiveMessage} {user.Nickname} diz: {message.Content}");
                    }
                    break;
                case Message.MessagePrivate:
                    if (toUser == null)
                    {
                        await SendMessageToOneAsync($"{Message.MentionError} {message.ToNickname} não encontrado na sala.", socket);
                    }
                    else
                    {
                        await SendMessageToOneAsync($"{Message.ReceiveMessagePrivate} {user.Nickname} diz para {message.ToNickname} (privado): {message.Content}", message.ToNickname);
                        await SendMessageToOneAsync($"{Message.ReceiveMessagePrivate} {user.Nickname} diz para {message.ToNickname} (privado): {message.Content}", socket);
                    }
                    break;
            }
        }
    }
}
