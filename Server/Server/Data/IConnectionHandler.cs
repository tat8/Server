using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Rubius.DevSaunaB.Server.Data
{
    /// <summary>
    /// Осуществляет работу с Сервером Данных
    /// </summary>
    public interface IConnectionHandler
    {
        /// <summary>
        /// Выполняет подключение к Серверу Данных
        /// </summary>
        /// <param name="uri"> адрес Сервера Данных </param>
        /// <returns> клиент, который соединен с Сервером Данных </returns>
        Task<ClientWebSocket> Connect(string uri);

        /// <summary>
        /// Посылает запрос и ждет ответа от Сервера Данных
        /// </summary>
        /// <param name="client"> клиент, который соединен с Сервером Данных </param>
        /// <param name="jObject"> запрос к Серверу Данных </param>
        /// <returns> ответ на запрос </returns>
        Task<JObject> SendRequest(ClientWebSocket client, JObject jObject);

        /// <summary>
        /// Выполняет завершения сеанса с Сервером Данных для клиента
        /// </summary>
        /// <param name="client"> клиент, который соединен с Сервером Данных </param>
        /// <returns></returns>
        Task Close(ClientWebSocket client);
    }
}
