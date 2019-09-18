using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Rubius.DevSaunaB.Server.Models;

namespace Rubius.DevSaunaB.Server.Services
{
    public interface IDataObjectService
    {
        /// <summary>
        /// Добавление объекта данных
        /// </summary>
        /// <param name="dataObject"> объект для добавления </param>
        /// <returns> добавленный объект с установленным id </returns>
        DataObject AddObject(DataObject dataObject);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="dataObject"> объект для удаления </param>
        void RemoveObject(DataObject dataObject);

        /// <summary>
        /// Возвращает текущее состояние всех объектов со всеми параметрами
        /// </summary>
        /// <returns></returns>
        JObject GetObjects();

        /// <summary>
        /// Возвращает текущее состояние объектов только тех параметров, которые изменились с предыдущего запроса
        /// </summary>
        /// <returns></returns>
        JObject GetObjectsCompact();

        /// <summary>
        /// Возвращает набор состояний всех объектов со всеми параметрами за последние 5 минут (или меньше, если еще не накопилось)
        /// с указанием времени для каждого состояния
        /// </summary>
        /// <returns></returns>
        IEnumerable<JObject> GetObjectsStates();
    }
}
