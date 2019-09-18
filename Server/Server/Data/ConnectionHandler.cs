using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rubius.DevSaunaB.Server.Data
{
    /// <inheritdoc/>
    public class ConnectionHandler: IConnectionHandler
    {
        /// <inheritdoc/>
        public async Task<ClientWebSocket> Connect(string uri)
        {
            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(uri), CancellationToken.None);
            return client;
        }

        /// <inheritdoc/>
        public async Task Close(ClientWebSocket client)
        {
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "End of session.", CancellationToken.None);
        }

        /// <inheritdoc/>
        public async Task<JObject> SendRequest(ClientWebSocket client, JObject jObject)
        {
            var message = JsonConvert.SerializeObject(jObject);
            var bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            var result = ReceiveMessage(client).Result;
            
            return result;
        }

        /// <summary>
        /// Ожидает получение сообщения от Сервера Данных
        /// </summary>
        /// <param name="client"> клиент, который соединен с Сервером Данных </param>
        /// <returns> сообщение от Сервера Данных </returns>
        private async Task<JObject> ReceiveMessage(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    // TODO: Проверять, это результат запроса или сообщение об ошибке
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var jObject = JsonConvert.DeserializeObject(message) as JObject;
                    return jObject;
                }
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
            return new JObject();
        }
    }
}
