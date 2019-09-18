using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Rubius.DevSaunaB.DataServer.Models;

namespace Rubius.DevSaunaB.DataServer.Services
{
    public class StoreService: IStoreService
    {
        private StoreCollection _storeCollection;

        public StoreService(StoreCollection storeCollection)
        {
            _storeCollection = storeCollection;
        }

        /// <inheritdoc/>
        public Store GetStore(string socketId)
        {
            return _storeCollection.GetStore(socketId);
        }

        /// <inheritdoc/>
        public void SetStore(string socketId, Store store)
        {
            _storeCollection.SetStore(socketId, store);
        }

        /// <inheritdoc/>
        public JObject GetObjects(string socketId)
        {
            var store = GetStore(socketId);

            var result = new List<JObject>();
            var objects = store.Objects;
            var bindings = store.Bindings;

            foreach (var dataObject in objects)
            {
                var jObject = new JObject
                {
                    { "id", dataObject.Id },
                    {"name", dataObject.Name  }
                };
                var objectBindings = bindings.Where(o => o.Object == dataObject.Id);
                foreach (var objectBinding in objectBindings)
                {
                    var newValue = GenerateValue(objectBinding.Transformation);
                    jObject.Add(objectBinding.Property, newValue);
                }
                result.Add(jObject);
            }

            var jObjectResult = new JObject
            {
                {"objects", JToken.FromObject(result)}
            };

            return jObjectResult;
        }

        /// <summary>
        /// Генерирует новое значение параметра в заданных границах
        /// </summary>
        /// <param name="transformation"> границы изменения параметров (Transformation) </param>
        /// <returns> новое значение параметра </returns>
        private int GenerateValue(Transformation transformation)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            return random.Next(transformation.Min, transformation.Max);
        }
    }
}
