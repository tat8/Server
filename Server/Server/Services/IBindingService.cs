using Rubius.DevSaunaB.Server.Models;

namespace Rubius.DevSaunaB.Server.Services
{
    public interface IBindingService
    {
        /// <summary>
        /// Добавление привязки параметра к объекту
        /// </summary>
        /// <param name="binding"> привязка для добавления </param>
        void AddBinding(Binding binding);

        /// <summary>
        /// Изменение привязки параметра к объекту
        /// </summary>
        /// <param name="binding"> новая привязка параметра </param>
        void ModifyBinding(Binding binding);

        /// <summary>
        /// Удаление привязки параметра к объекту
        /// </summary>
        /// <param name="binding"> привязка для удаления </param>
        void RemoveBinding(Binding binding);

        void SetEmergencyMode(bool isTrue);
    }
}
