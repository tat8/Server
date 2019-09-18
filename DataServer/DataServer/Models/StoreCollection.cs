using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rubius.DevSaunaB.DataServer.Models
{
    /// <summary>
    /// Коллекция хранилищ для всех клиентов-сокетов (Singleton) 
    /// </summary>
    public class StoreCollection
    {
        private ConcurrentDictionary<string, Store> _stores;

        public StoreCollection()
        {
            _stores = new ConcurrentDictionary<string, Store>();
        }
        
        /// <summary>
        /// Устанавливает хранилище заданному сокету
        /// </summary>
        /// <param name="socketId"> id сокета, которому необходимо установить хранилище </param>
        /// <param name="store"> хранилище, которое необходимо установить </param>
        public void SetStore(string socketId, Store store)
        {
            _stores[socketId] = store;
        }

        /// <summary>
        /// Возвращает хранилище заданного сокета
        /// </summary>
        /// <param name="socketId"> id сокета, хранилище которого необходимо получить </param>
        /// <returns> хранилище, принадлежащее сокету с id 'socketId' </returns>
        public Store GetStore(string socketId)
        {
            if (!_stores.ContainsKey(socketId))
            {
                _stores[socketId] = new Store
                {
                    Bindings = new List<Binding>(),
                    Objects = new List<DataObject>()
                };
            }
            return _stores[socketId];
        }

    }
}
