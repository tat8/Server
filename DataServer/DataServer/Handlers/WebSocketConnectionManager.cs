using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Rubius.DevSaunaB.DataServer.Services;

namespace Rubius.DevSaunaB.DataServer.Handlers
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private SocketStateHolder _socketStateHolder;

        public WebSocketConnectionManager(SocketStateHolder socketStateHolder)
        {
            _socketStateHolder = socketStateHolder;
        }

        /// <summary>
        /// Возвращает сокет по заданному id
        /// </summary>
        /// <param name="id"> id сокета </param>
        /// <returns> сокет с заданным id </returns>
        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        /// <summary>
        /// Возвращает все подключенные сокеты
        /// </summary>
        /// <returns> все подключенные сокеты </returns>
        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        /// <summary>
        /// Возврщает id сокета
        /// </summary>
        /// <param name="socket"> сокет, id которого необходимо узнать </param>
        /// <returns> id сокета </returns>
        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        /// <summary>
        /// Добавляет новый сокет в список подлюченных сокетов
        /// </summary>
        /// <param name="socket"> новый сокет </param>
        public void AddSocket(WebSocket socket)
        {
            _socketStateHolder.AddState("Trying to add new socket");
            _sockets.TryAdd(CreateConnectionId(), socket);
            _socketStateHolder.AddState("Added new socket");
        }

        /// <summary>
        /// Удаляет сокет из списка подключенных сокетов
        /// </summary>
        /// <param name="id"> id сокета для удаления </param>
        /// <returns></returns>
        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out var socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by the WebSocketManager",
                cancellationToken: CancellationToken.None);
        }

        /// <summary>
        /// Создает новый уникальный идентификатор
        /// </summary>
        /// <returns> уникальный идентификатор </returns>
        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    
    }
}
