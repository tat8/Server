using Newtonsoft.Json.Linq;
using Rubius.DevSaunaB.DataServer.Models;

namespace Rubius.DevSaunaB.DataServer.Services
{
    public interface IStoreService
    {
        /// <summary>
        /// Возвращает хранилище для указанного сокета
        /// </summary>
        /// <param name="socketId"> id сокета, хранилище которого необходимо получить </param>
        /// <returns> хранилище указанного сокета </returns>
        Store GetStore(string socketId);

        /// <summary>
        /// Устанавливает хранилище указанному сокету
        /// </summary>
        /// <param name="socketId"> id сокета, хранилище которому необходимо установить </param>
        /// <param name="store"> хранилище к установке </param>
        void SetStore(string socketId, Store store);

        /// <summary>
        /// Возвращает все объекты и их параметры, которые принадлежат сокету с заданным id
        /// </summary>
        /// <param name="socketId"> id сокета, объекты и параметры которого необходимо получить </param>
        /// <returns> все объекты и их параметры заданного сокета </returns>
        JObject GetObjects(string socketId);
    }
}
