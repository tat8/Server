using Rubius.DevSaunaB.Server.Models;

namespace Rubius.DevSaunaB.Server.Services
{
    /// <summary>
    /// Создает объект 3D-модели и добавляет ей параметры. (Singleton)
    /// Вызывается при запуске Сервера
    /// </summary>
    public class ObjectsCreatorService
    {
        private IDataObjectService _dataObjectService;
        private IBindingService _bindingService;
        
        public ObjectsCreatorService( IDataObjectService dataObjectService, IBindingService bindingService)
        {
            _dataObjectService = dataObjectService;
            _bindingService = bindingService;
            CreateModel();
        }
        
        /// <summary>
        /// Создает объект Модели и два параметра, связанных с ней
        /// </summary>
        private void CreateModel()
        {
            var dataObject = new DataObject
            {
                Id = 0,
                Name = "Model"
            };

            _dataObjectService.AddObject(dataObject);
            
            var bindingRight = new Binding
            {
                Name = "bindingRight",
                Object = (long)dataObject.Id,
                Property = "rightTemperature",
                Transformation = new Transformation
                {
                    Min = 30,
                    Max = 60,
                }
            };

            var bindingLeft = new Binding
            {
                Name = "bindingLeft",
                Object = (long)dataObject.Id,
                Property = "leftTemperature",
                Transformation = new Transformation
                {
                    Min = 40,
                    Max = 60
                }
            };

            var bindingDown = new Binding()
            {
                Name = "bindingDown",
                Object = (long) dataObject.Id,
                Property = "downTemperature",
                Transformation = new Transformation
                {
                    Min = 10,
                    Max = 40
                }
            };

            _bindingService.AddBinding(bindingRight);
            _bindingService.AddBinding(bindingLeft);
            _bindingService.AddBinding(bindingDown);
        }
    }
}
