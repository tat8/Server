using System;

namespace Rubius.DevSaunaB.Server.Exceptions
{
    /// <summary>
    /// Для обработки внутренних ошибок на сервере и ответов с ошибками от Сервера Данных
    /// </summary>
    public class BadRequestException: Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
