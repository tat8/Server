using System.ComponentModel.DataAnnotations;

namespace Rubius.DevSaunaB.Server.Models
{
    /// <summary>
    /// Привязка параметра к объекту (DataObject)
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// Название привязки
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Id объекта (DataObject), к которому относится привязка
        /// </summary>
        [Required]
        public long Object { get; set; }

        /// <summary>
        /// Название параметра
        /// </summary>
        [Required]
        public string Property { get; set; }

        /// <summary>
        /// Границы изменения значений параметра
        /// </summary>
        [Required]
        public Transformation Transformation { get; set; }
    }
}
