using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    /// <summary>
    /// Service para acesso aos dados dos usuários conectados
    /// Dados dos usuários estão em memória. Alterar esta classe para persistir os dados ou enviar para uma API
    /// </summary>
    public class UserService
    {
        private List<User> Users = new List<User>();

        /// <summary>
        /// Obtém o Websocket através do Id do usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WebSocket GetSocketById(string id)
        {
            return Users.FirstOrDefault(u => u.Id == id).Websocket;
        }

        /// <summary>
        /// Obtém o User através do nickname do usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User Get(string nickname)
        {
            return Users.Where(u => u.Nickname!=null && u.Nickname.ToLower() == nickname.ToLower())?.FirstOrDefault();
        }

        /// <summary>
        /// Obtém o User através do socket do usuário
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public User Get(WebSocket socket)
        {
            return Users.Where(u => u.Websocket == socket)?.FirstOrDefault();
        }

        /// <summary>
        /// Obtém todos os usuários
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll()
        {
            return Users;
        }

        /// <summary>
        /// Adiciona um User na lista de usuários
        /// </summary>
        /// <param name="socket"></param>
        public void AddSocket(WebSocket socket)
        {
            Users.Add(new User(socket));
        }

        /// <summary>
        /// Configura o Nickname do usuário e define seu Login para TRUE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public bool Login(string id, string nickname)
        {
            var userByNickname = Users.Where(u => u.Nickname!=null && u.Nickname?.ToLower() == nickname.ToLower()).FirstOrDefault();
            if (userByNickname != null)
                return false;

            Users.FirstOrDefault(u => u.Id == id)?.Login(nickname);
            return true;
        }

        /// <summary>
        /// Remove o usuário e fecha socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveSocket(string id)
        {
            var user = Users.Where(u => u.Id == id).First();
            Users.Remove(user);
            await user.Websocket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the Service",
                                    cancellationToken: CancellationToken.None);
        }
    }
}
