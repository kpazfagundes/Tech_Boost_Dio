using Server.Services;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketManager
{
    /// <summary>
    /// Handler abstrato com métodos básicos de manipulação/listener do websocket
    /// </summary>
    public abstract class WebSocketHandler
    {
        /// <summary>
        /// Instância do serviço de Usuários
        /// </summary>
        protected UserService Service { get; set; }

        public WebSocketHandler(UserService Service)
        {
            this.Service = Service;
        }

        /// <summary>
        /// Adiciona o socket ao UserService
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnConnected(WebSocket socket)
        {
            Service.AddSocket(socket);
        }

        /// <summary>
        /// Remove o socket do UserService
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await Service.RemoveSocket(Service.Get(socket).Id);
        }

        /// <summary>
        /// Envia uma mensagem para um socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if(socket.State != WebSocketState.Open)
                return;
            var encoded = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(buffer: new ArraySegment<byte>(array: encoded,
                                                                  offset: 0,
                                                                  count: encoded.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        /// <summary>
        /// Envia uma mensagem para todos os usuários na sala
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToAllAsync(string message)
        {
            foreach(var user in Service.GetAll())
            {
                if(user.Websocket.State == WebSocketState.Open && user.Logged)
                    await SendMessageAsync(user.Websocket, message);
            }
        }

        /// <summary>
        /// Envia uma mensagem para um usuário específico, buscando pelo nickname
        /// </summary>
        /// <param name="message"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public async Task SendMessageToOneAsync(string message, string nickname)
        {
            var user = Service.Get(nickname);
            if(user.Websocket.State == WebSocketState.Open && user.Logged)
                await SendMessageAsync(user.Websocket, message);
        }

        /// <summary>
        /// Envia uma mensagem para um usuário específico, buscando pelo socket
        /// </summary>
        /// <param name="message"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        public async Task SendMessageToOneAsync(string message, WebSocket socket)
        {
            if (socket.State == WebSocketState.Open)
                await SendMessageAsync(socket, message);
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
