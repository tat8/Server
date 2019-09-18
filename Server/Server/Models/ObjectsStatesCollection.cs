using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Rubius.DevSaunaB.Server.Models
{
    /// <summary>
    /// Хранит состояния объектов за последние 5 минут (Singleton)
    /// </summary>
    public class ObjectsStatesCollection
    {
        private List<JObject> _states;

        /// <summary>
        /// id последнего добавленного объекта
        /// </summary>
        private long _lastid;


        public ObjectsStatesCollection()
        {
            _states = new List<JObject>();
            _lastid = 0;
        }
        
        /// <summary>
        /// Добавляет объект в коллекцию, записывая время, в которое объект был добавлен
        /// </summary>
        /// <param name="jObject"> объект, который необходимо добавить </param>
        public void Add(JObject jObject)
        {
            // var st = DateTime.UtcNow.ToString("HH:mm:ss dd-MM-yyyy");
            // JObject.FromObject(DateTime.UtcNow.ToString("HH:mm:ss dd-MM-yyyy"))
            var state = new JObject
            {
                {"utcTime", DateTime.UtcNow},
                {"state", jObject}
            };
            _states.Add(state);
            RemoveOldStates();
        }

        /// <summary>
        /// Возвращает все состояния объектов и время, когда они были получены, за последние 5 минут
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JObject> Get()
        {
            RemoveOldStates();
            return _states;
        }

        public long GetNextObjectId()
        {
            _lastid += 1;
            return _lastid;
        }

        /// <summary>
        /// Удаляет состояния, которые были получены более 5 минут назад
        /// </summary>
        private void RemoveOldStates()
        {
            var time = DateTime.UtcNow;

            foreach (var state in _states.ToList())
            {
                if (state["utcTime"].Value<DateTime>().AddMinutes(5) < time)
                {
                    _states.Remove(state);
                }
            }
        }

    }
}
