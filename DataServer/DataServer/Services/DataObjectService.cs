using System.Linq;
using Rubius.DevSaunaB.DataServer.Exceptions;
using Rubius.DevSaunaB.DataServer.Models;

namespace Rubius.DevSaunaB.DataServer.Services
{
    public class DataObjectService: IDataObjectService
    {
        /// <inheritdoc/>
        public Store AddObject(DataObject dataObject, Store store)
        {
            var alreadyExists = store.Objects.FirstOrDefault(o => o.Id == dataObject.Id);
            if (alreadyExists != null)
            {
                throw new BadRequestException("Object with such id already exists.");
            }

            var objects = store.Objects.ToList();
            objects.Add(dataObject);
            store.Objects = objects;

            return store;
        }

        /// <inheritdoc/>
        public Store RemoveObject(long id, Store store)
        {
            var objects = store.Objects.ToList();
            var dataObject = store.Objects.FirstOrDefault(o => o.Id == id);
            if (dataObject != null)
            {
                objects.Remove(dataObject);
                store.Objects = objects;

                return store;
            }
            else
            {
                throw new BadRequestException("There is no object with such id");
            }
        }
    }
}
