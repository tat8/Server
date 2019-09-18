using Rubius.DevSaunaB.DataServer.Models;

namespace Rubius.DevSaunaB.DataServer.Services
{
    public interface IDataObjectService
    {
        /// <summary>
        /// Добавляет объект в хранилище
        /// </summary>
        /// <param name="dataObject"> объект к добавлению </param>
        /// <param name="store"> хранилище, в которое необходимо добавить объект </param>
        /// <returns> новое состояние хранилища </returns>
        Store AddObject(DataObject dataObject, Store store);

        /// <summary>
        /// Удаляет объект из хранилища
        /// </summary>
        /// <param name="id"> id объекта, который необходимо удалить </param>
        /// <param name="store"> хранилище, из которого необходимо удалить объект </param>
        /// <returns> новое состояние хранилища </returns>
        Store RemoveObject(long id, Store store);
    }
}
