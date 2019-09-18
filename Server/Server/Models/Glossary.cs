namespace Rubius.DevSaunaB.Server.Models
{
    /// <summary>
    /// Словарь, который хранит константные значения
    /// </summary>
    public class Glossary
    {
        public const string DataServerConnectionString = "wss://dataserver20190527122040.azurewebsites.net/messages";
        public const string GetObjectsRequest = "getObjects";
        public const string AddObjectRequest = "addObject";
        public const string RemoveObjectRequest = "removeObject";
        public const string AddBindingRequest = "addBinding";
        public const string RemoveBindingRequest = "removeBinding";
        public const string ModifyBindingRequest = "modifyBinding";
        public const string ErrorMessage = "errorMessage";
    }
}
