using Newtonsoft.Json.Linq;

namespace Rubius.DevSaunaB.DataServer.Handlers
{
    /// <summary>
    /// Обрабатывает запрос, вызвывая необходимые сервисы
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Осуществляет вызов сервиса, соответствующего строке 'request'
        /// </summary>
        /// <param name="socketId"> id сокета, от которого пришел запрос </param>
        /// <param name="request"> строка-запрос </param>
        /// <param name="parameters"> параметры запроса </param>
        /// <returns> ответ на запрос </returns>
        object HandleRequest(string socketId, string request, JObject parameters);
    }
}