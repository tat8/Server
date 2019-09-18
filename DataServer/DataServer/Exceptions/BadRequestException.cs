using System;

namespace Rubius.DevSaunaB.DataServer.Exceptions
{
    /// <summary>
    /// Для отслеживания некорректных запросов (неправильная строка 'request' или недостающие параметры в 'params')
    /// </summary>
    [Serializable]
    public class BadRequestException: Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
