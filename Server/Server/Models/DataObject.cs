using System.ComponentModel.DataAnnotations;

namespace Rubius.DevSaunaB.Server.Models
{
    public class DataObject
    {
        /// <summary>
        /// Id объекта
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Название объекта
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
