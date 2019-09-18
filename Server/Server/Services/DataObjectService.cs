using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Newtonsoft.Json.Linq;
using Rubius.DevSaunaB.Server.Data;
using Rubius.DevSaunaB.Server.Exceptions;
using Rubius.DevSaunaB.Server.Models;

namespace Rubius.DevSaunaB.Server.Services
{
    public class DataObjectService: IDataObjectService
    {
        private ClientWebSocket _client;
        private IConnectionHandler _connectionHandler;
        private ObjectsStatesCollection _objectsStatesCollection;

        public DataObjectService(ClientConnection client, IConnectionHandler connectionHandler, ObjectsStatesCollection statesCollection)
        {
            _connectionHandler = connectionHandler;
            _client = client.Get();
            _objectsStatesCollection = statesCollection;
        }

        /// <inheritdoc/>
        public DataObject AddObject(DataObject dataObject)
        {
            dataObject.Id = _objectsStatesCollection.GetNextObjectId();
            
            var message = new JObject
            {
                {"request", Glossary.AddObjectRequest },
                {"params",  DataObjectToJObject(dataObject)}
            };

            SendRequest(message);
            return dataObject;
        }

        /// <inheritdoc/>
        public void RemoveObject(DataObject dataObject)
        {
            if (dataObject.Id == null)
            {
                throw new BadRequestException("The parameter 'id' is required");
            }

            var message = new JObject
            {
                {"request", Glossary.RemoveObjectRequest },
                {"params",  DataObjectToJObject(dataObject)}
            };

            SendRequest(message);
        }

        /// <inheritdoc/>
        public JObject GetObjects()
        {
            var messageParams = new JObject
            {
                {"x", "0" }
            };

            var message = new JObject
            {
                {"request", Glossary.GetObjectsRequest },
                {"params", messageParams }
            };

            var state = _connectionHandler.SendRequest(_client, message);

            if (state.Result.ContainsKey(Glossary.ErrorMessage))
            {
                throw new BadRequestException(state.Result[Glossary.ErrorMessage].Value<string>());
            }

            SaveObjectsState(state.Result);

            return state.Result;
        }

        /// <inheritdoc/>
        public JObject GetObjectsCompact()
        {
            var statesCount = _objectsStatesCollection.Get().Count();
            if (statesCount == 0)
            {
                return GetObjects();
            }

            var prevState = _objectsStatesCollection.Get().Last()["state"];
            var currentState = GetObjects();

            var prevObjects = prevState["objects"].Value<JArray>();
            var currentObjects = currentState["objects"].Value<JArray>();
            var compactObjects = new JObject
            {
                { "objects", new JArray() },
                { "removedObjects", new JArray() },
                { "removedBindings", new JArray() },
                { "addedObjects", new JArray() },
                { "addedBindings", new JArray() }
            };

            var equals = JToken.DeepEquals(prevObjects, currentObjects);

            if (equals)
            {
                return compactObjects;
            }

            // Находим измененные и удаленные объекты и привязки
            foreach (var prevObject in prevObjects.Children())
            {
                var prevItemId = prevObject["id"].Value<long>();
                var isItemFound = false;
                foreach (var currentObject in currentObjects.Children())
                {
                    if (currentObject["id"].Value<long>() == prevItemId)
                    {
                        isItemFound = true;
                        var bindingsDiffer = false;
                        var newBinding = new JObject
                        {
                            { "id", prevItemId },
                            { "name",  prevObject["name"].Value<string>() }
                        };

                        foreach (var jToken in prevObject.Children())
                        {
                            var prevItemProperty = (JProperty) jToken;
                            var propertyName = prevItemProperty.Name;
                            if (currentObject[propertyName] == null)
                            {
                                var removed = new JObject
                                {
                                    { "object", prevItemId },
                                    { "name", propertyName }
                                };
                                compactObjects["removedBindings"].Value<JArray>().Add(removed);
                            }
                            else if(!JToken.DeepEquals(currentObject[propertyName], prevObject[propertyName]))
                            {
                                bindingsDiffer = true;
                                newBinding.Add(propertyName, currentObject[propertyName]);
                            }
                        }

                        if (bindingsDiffer)
                        {
                            compactObjects["objects"].Value<JArray>().Add(newBinding);
                        }
                    }
                }

                if (!isItemFound)
                {
                    compactObjects["removedObjects"].Value<JArray>().Add(prevObject);
                }
            }

            // Находим новые объекты и привязки
            foreach (var currentObject in currentObjects.Children())
            {
                var currentItemId = currentObject["id"].Value<long>();
                var isItemFound = false;
                foreach (var prevObject in prevObjects.Children())
                {
                    if (prevObject["id"].Value<long>() == currentItemId)
                    {
                        isItemFound = true;

                        foreach (var jToken in currentObject.Children())
                        {
                            var currentItemProperty = (JProperty) jToken;
                            var propertyName = currentItemProperty.Name;
                            if (prevObject[propertyName] == null)
                            {
                                var added = new JObject
                                {
                                    { "object", currentItemId },
                                    { "name", propertyName }
                                };
                                compactObjects["addedBindings"].Value<JArray>().Add(added);
                            }
                        }
                    }
                }

                if (!isItemFound)
                {
                    compactObjects["addedObjects"].Value<JArray>().Add(currentObject);
                }
            }

            return compactObjects;
        }

        /// <inheritdoc/>
        public IEnumerable<JObject> GetObjectsStates()
        {
            return _objectsStatesCollection.Get();
        }

        /// <summary>
        /// Сохраняет состояние объектов в коллекцию состояний ObjectsStatesCollection
        /// </summary>
        /// <param name="state"></param>
        private void SaveObjectsState(JObject state)
        {
            _objectsStatesCollection.Add(state);
        }

        /// <summary>
        /// Отправка запроса к серверу
        /// </summary>
        /// <param name="request"> запрос </param>
        private void SendRequest(JObject request)
        {
            var result = _connectionHandler.SendRequest(_client, request).Result;

            if (result.ContainsKey(Glossary.ErrorMessage))
            {
                throw new BadRequestException(result[Glossary.ErrorMessage].Value<string>());
            }
        }

        /// <summary>
        /// Возвращает объект JObject из DataObject
        /// Стандартный JObject.FromObject не подходит, так как он не переводит из паскаль-кейс в кэмел-кейс
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        private JObject DataObjectToJObject(DataObject dataObject)
        {
            return new JObject
            {
                {"id", dataObject.Id },
                {"name", dataObject.Name }
            };
        }
    }
}
