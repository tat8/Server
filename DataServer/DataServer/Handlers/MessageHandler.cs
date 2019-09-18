using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rubius.DevSaunaB.DataServer.Exceptions;
using Rubius.DevSaunaB.DataServer.Models;

namespace Rubius.DevSaunaB.DataServer.Handlers
{
    /// <inheritdoc/>
    public class MessageHandler : WebSocketHandler
    {
        private WebSocketConnectionManager _webSocketConnectionManager;
        private IRequestHandler _requestHandler;

        public MessageHandler(WebSocketConnectionManager webSocketConnectionManager, IRequestHandler requestHandler) 
            : base(webSocketConnectionManager)
        {
            _requestHandler = requestHandler;
            _webSocketConnectionManager = webSocketConnectionManager;
        }

        /// <summary>
        /// Получает и десериализует сообщение;
        /// передает сообщение в обработчик сообщений
        /// </summary>
        /// <param name="socket"> сокет, от которого пришло сообщение </param>
        /// <param name="result"> результат получения сообщения WebSocketReceiveResult </param>
        /// <param name="buffer"> сообщение в виде byte[] </param>
        /// <returns></returns>
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var jsonMessage = (JObject)JsonConvert.DeserializeObject(message);

            var socketId = _webSocketConnectionManager.GetId(socket);

            try
            {
                var request = jsonMessage["request"];
                if (request == null)
                {
                    throw new BadRequestException("The parameter 'request' is missing");
                }

                var requestValue = request.Value<string>();

                var parameters = jsonMessage["params"];
                if (parameters == null)
                {
                    throw new BadRequestException("The parameter 'params' is missing");
                }

                var parametersValue = parameters.Value<JObject>();

                var requestResult = _requestHandler.HandleRequest(socketId, requestValue, parametersValue);
                if (requestResult is JObject jObject)
                {
                    requestResult = jObject;
                }
                else
                {
                    requestResult = new JObject
                    {
                        {"success", true }
                    };
                }

                var resultString = JsonConvert.SerializeObject(requestResult);
                await SendMessageAsync(socket, resultString);
            }
            catch (BadRequestException e)
            {
                var eMessage = new JObject
                {
                    {Glossary.ErrorMessage, e.Message }
                };
                var eMessageJsonString = JsonConvert.SerializeObject(eMessage);
                await SendMessageAsync(socket, eMessageJsonString);
            }
            
        }
    }
}
