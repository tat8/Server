namespace Rubius.DevSaunaB.DataServer.Models
{
    /// <summary>
    /// Привязка параметра к объекту (DataObject)
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// Название привязки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id объекта (DataObject), к которому относится привязка
        /// </summary>
        public long Object { get; set; }

        /// <summary>
        /// Название параметра
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Границы изменения параметра
        /// </summary>
        public Transformation Transformation { get; set; }
    }
}
