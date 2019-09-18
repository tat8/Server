using Rubius.DevSaunaB.DataServer.Models;

namespace Rubius.DevSaunaB.DataServer.Services
{
    public interface IBindingService
    {
        /// <summary>
        /// Добавляет привязку параметра к объекту в хранилище
        /// </summary>
        /// <param name="binding"> привязка к добавлению </param>
        /// <param name="store"> хранилище, в которое необходимо добавить привязку </param>
        /// <returns> новое состояние хранилища </returns>
        Store AddBinding(Binding binding, Store store);

        /// <summary>
        /// Удаляет привязку параметра к объекту в хранилище
        /// </summary>
        /// <param name="name"> название привязки (Binding.Name) к удалению </param>
        /// <param name="store"> хранилище, из которого необходимо удалить привязку </param>
        /// <returns> новое состояние хранилища </returns>
        Store RemoveBinding(string name, Store store);

        /// <summary>
        /// Изменяет привязку параметра к объекту в хранилище
        /// </summary>
        /// <param name="binding"> привязка к изменение </param>
        /// <param name="store"> хранилище, в котором необходимо изменить привязку </param>
        /// <returns> новое состояние хранилища </returns>
        Store ModifyBinding(Binding binding, Store store);
    }
}
