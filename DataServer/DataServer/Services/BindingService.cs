using System.Linq;
using Rubius.DevSaunaB.DataServer.Exceptions;
using Rubius.DevSaunaB.DataServer.Models;

namespace Rubius.DevSaunaB.DataServer.Services
{
    public class BindingService: IBindingService
    {
        /// <inheritdoc/>
        public Store AddBinding(Binding binding, Store store)
        {
            var alreadyExists = store.Bindings.FirstOrDefault(o => o.Name == binding.Name);

            if (alreadyExists != null)
            {
                throw new BadRequestException("Binding with such name already exists.");
            }
            
            var dataObject = store.Objects.FirstOrDefault(o => o.Id == binding.Object);
            if (dataObject == null)
            {
                throw new BadRequestException("No object with such id found for this binding");
            }

            var propertyExists = store.Bindings.Where(o => o.Object == dataObject.Id)
                .FirstOrDefault(o => o.Property == binding.Property);

            if (propertyExists != null)
            {
                throw new BadRequestException("Binding with such 'property' and 'object' id already exist");
            }
            
            var bindings = store.Bindings.ToList();
            bindings.Add(binding);
            store.Bindings = bindings;

            return store;
        }

        /// <inheritdoc/>
        public Store RemoveBinding(string name, Store store)
        {
            var bindings = store.Bindings.ToList();
            var binding = store.Bindings.FirstOrDefault(o => o.Name == name);
            if (binding != null)
            {
                bindings.Remove(binding);
                store.Bindings = bindings;

                return store;
            }
            else
            {
                throw new BadRequestException("There is no binding with such name");
            }
        }

        /// <inheritdoc/>
        public Store ModifyBinding(Binding binding, Store store)
        {
            store = RemoveBinding(binding.Name, store);
            store = AddBinding(binding, store);

            return store;
        }
    }
}
