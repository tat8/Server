using System.Collections.Generic;

namespace Rubius.DevSaunaB.DataServer.Models
{
    /// <summary>
    /// хранилище объектов и привязок
    /// </summary>
    public class Store
    {
        public IEnumerable<DataObject> Objects { get; set; }
        public IEnumerable<Binding> Bindings { get; set; }

    }
}
