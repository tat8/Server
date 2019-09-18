using System.Linq;
using Newtonsoft.Json.Linq;
using Rubius.DevSaunaB.DataServer.Exceptions;
using Rubius.DevSaunaB.DataServer.Models;
using Rubius.DevSaunaB.DataServer.Services;

namespace Rubius.DevSaunaB.DataServer.Handlers
{
    public class RequestHandler : IRequestHandler
    {
        private IBindingService _bindingService;
        private IDataObjectService _dataObjectService;
        private IStoreService _storeService;

        public RequestHandler(IBindingService bindingService, IDataObjectService dataObjectService, IStoreService storeService)
        {
            _bindingService = bindingService;
            _dataObjectService = dataObjectService;
            _storeService = storeService;
        }

        /// <inheritdoc/>
        public object HandleRequest(string socketId, string request, JObject parameters)
        {
            switch (request)
            {
                case Glossary.AddBindingRequest: return AddBinding(parameters, socketId);
                case Glossary.AddObjectRequest: return AddObject(parameters, socketId);
                case Glossary.GetObjectsRequest: return GetObjects(socketId);
                case Glossary.ModifyBindingRequest: return ModifyBinding(parameters, socketId);
                case Glossary.RemoveBindingRequest: return RemoveBinding(parameters, socketId);
                case Glossary.RemoveObjectRequest: return RemoveObject(parameters, socketId);
                default: throw new BadRequestException("Unknown request");
            }
        }

        /// <summary>
        /// Вызывает сервис, который возвращает все объекты и их параметры, которые принадлежат сокету с заданным id
        /// </summary>
        /// <param name="socketId"> id сокета, от которого пришел запрос </param>
        /// <returns> все объекты и их параметры в виде JObject </returns>
        private JObject GetObjects(string socketId)
        {
            return _storeService.GetObjects(socketId);
        }

        /// <summary>
        /// Подготавливает данные для сервиса, который добавляет привязку параметра к объекту в хранилище;
        /// вызывает этот сервис;
        /// сохраняет новое состояние хранилища
        /// </summary>
        /// <param name="parameters"> параметры, характеризующие новую привязку (Binding), в виде JObject </param>
        /// <param name="socketId"> id сокета, от которого пришел запрос </param>
        /// <returns></returns>
        private bool AddBinding(JObject parameters, string socketId)
        {
            var store = _storeService.GetStore(socketId);
            var binding = JObjectToBinding(parameters);
            store = _bindingService.AddBinding(binding, store);
            _storeService.SetStore(socketId, store);
            return true;
        }

        /// <summary>
        /// Подготавливает данные для сервиса, который добавляет объект в хранилище;
        /// вызывает этот сервис;
        /// сохраняет новое состояние хранилища
        /// </summary>
        /// <param name="parameters"> параметры, характеризующие новый объект (DataObject), в виде JObject </param>
        /// <param name="socketId"> id сокета, от которого пришел запрос </param>
        /// <returns></returns>
        private bool AddObject(JObject parameters, string socketId)
        {
            var store = _storeService.GetStore(socketId);
            var dataObject = JObjectToDataObject(parameters);
            store = _dataObjectService.AddObject(dataObject, store);
            _storeService.SetStore(socketId, store);
            return true;
        }

        /// <summary>
        /// Подготавливает данные для сервиса, который изменяет привязку параметра в хранилище;
        /// вызывает этот сервис;
        /// сохраняет новое состояние хранилища
        /// </summary>
        /// <param name="parameters"> параметры, характеризующие новую привязку (Binding), в виде JObject </param>
        /// <param name="socketId"> id сокета, от которого пришел запрос </param>
        /// <returns></returns>
        private bool ModifyBinding(JObject parameters, string socketId)
        {
            var store = _storeService.GetStore(socketId);
            var binding = JObjectToBinding(parameters);
            store = _bindingService.ModifyBinding(binding, store);
            _storeService.SetStore(socketId, store);
            return true;
        }

        /// <summary>
        /// Подготавливает данные для сервиса, который удаляет привязку параметра из хранилища;
        /// вызывает этот сервис;
        /// сохраняет новое состояние хранилища
        /// </summary>
        /// <param name="parameters"> параметры, характеризующие привязку (Binding) для удаления, в виде JObject </param>
        /// <param name="socketId"> id сокета, от которого пришел запрос </param>
        /// <returns></returns>
        private bool RemoveBinding(JObject parameters, string socketId)
        {
            var store = _storeService.GetStore(socketId);
            var binding = JObjectToBinding(parameters);
            store = _bindingService.RemoveBinding(binding.Name, store);
            _storeService.SetStore(socketId, store);
            return true;
        }

        /// <summary>
        /// Подготавливает данные для сервиса, который удаляет объект из хранилища;
        /// вызывает этот сервис;
        /// сохраняет новое состояние хранилища
        /// </summary>
        /// <param name="parameters"> параметры, характеризующие объект (DataObject) для удаления, в виде JObject </param>
        /// <param name="socketId"> id сокета, от которого пришел запрос </param>
        /// <returns></returns>
        private bool RemoveObject(JObject parameters, string socketId)
        {
            var store = _storeService.GetStore(socketId);
            var dataObject = JObjectToDataObject(parameters);

            // удаляем привязки
            var bindings = store.Bindings.Where(o => o.Object == dataObject.Id);
            foreach (var binding in bindings)
            {
                store = _bindingService.RemoveBinding(binding.Name, store);
            }

            // удаляем объект
            store = _dataObjectService.RemoveObject(dataObject.Id, store);

            _storeService.SetStore(socketId, store);
            return true;
        }


        /// <summary>
        /// Преобразует JObject в DataObject
        /// </summary>
        /// <param name="jObject"> объект JObject, который требуется преобразовать в DataObject </param>
        /// <returns> преобразованный объект DataObject </returns>
        private DataObject JObjectToDataObject(JObject jObject)
        {
            return new DataObject()
            {
                Id = jObject["id"]?.Value<long>() ?? throw new BadRequestException("The parameter 'id' is missing"),
                Name = jObject["name"]?.Value<string>() ?? throw new BadRequestException("The parameter 'name' is missing")
            };
        }

        /// <summary>
        /// Преобразует JObject в Binding
        /// </summary>
        /// <param name="jObject"> объект JObject, который требуется преобразовать в Binding </param>
        /// <returns> преобразованный объект Binding </returns>
        private Binding JObjectToBinding(JObject jObject)
        {
            var transformationParameters = jObject["transformation"] != null? jObject["transformation"].Value<JObject>() : throw new BadRequestException("The parameter 'transformation' is missing");
            return new Binding()
            {
                Name = jObject["name"]?.Value<string>() ?? throw new BadRequestException("The parameter 'name' is missing"),
                Object = jObject["object"]?.Value<long>() ?? throw new BadRequestException("The parameter 'object' is missing"),
                Property = jObject["property"]?.Value<string>() ?? throw new BadRequestException("The parameter 'property' is missing"),
                Transformation = new Transformation()
                {
                    Max = transformationParameters["max"]?.Value<int>() ?? throw new BadRequestException("The parameter 'transformation.max' is missing"),
                    Min = transformationParameters["min"]?.Value<int>() ?? throw new BadRequestException("The parameter 'transformation.min' is missing")
                }
            };
        }
        
    }
}
