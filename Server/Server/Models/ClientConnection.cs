using System.Net.WebSockets;
using Rubius.DevSaunaB.Server.Data;

namespace Rubius.DevSaunaB.Server.Models
{
    /// <summary>
    /// Представляет собой клиента ClientWebSocket, который подключается к Серверу Данных (Singleton)
    /// </summary>
    public class ClientConnection
    {
        private ClientWebSocket _client;
        private IConnectionHandler _connectionHandler;

        public ClientConnection(IConnectionHandler connectionHandler)
        {
            _connectionHandler = connectionHandler;
            _client = Connect();
        }

        public ClientWebSocket Get()
        {
            return _client;
        }

        private ClientWebSocket Connect()
        {
            return _connectionHandler.Connect(Glossary.DataServerConnectionString).Result;
        }

    }
}
