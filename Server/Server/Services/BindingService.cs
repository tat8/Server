using System.Net.WebSockets;
using Newtonsoft.Json.Linq;
using Rubius.DevSaunaB.Server.Data;
using Rubius.DevSaunaB.Server.Exceptions;
using Rubius.DevSaunaB.Server.Models;

namespace Rubius.DevSaunaB.Server.Services
{
    public class BindingService: IBindingService
    {
        private ClientWebSocket _client;
        private IConnectionHandler _connectionHandler;

        public BindingService(ClientConnection client, IConnectionHandler connectionHandler)
        {
            _connectionHandler = connectionHandler;
            _client = client.Get();
        }

        /// <inheritdoc/>
        public void AddBinding(Binding binding)
        {
            var message = new JObject
            {
                {"request", Glossary.AddBindingRequest },
                {"params",  BindingToJObject(binding)}
            };

            SendRequest(message);
        }

        /// <inheritdoc/>
        public void ModifyBinding(Binding binding)
        {
            var message = new JObject
            {
                {"request", Glossary.ModifyBindingRequest },
                {"params",  BindingToJObject(binding)}
            };

            SendRequest(message);
        }

        /// <inheritdoc/>
        public void RemoveBinding(Binding binding)
        {
            var message = new JObject
            {
                {"request", Glossary.RemoveBindingRequest },
                {"params",  BindingToJObject(binding)}
            };

            SendRequest(message);
        }

        public void SetEmergencyMode(bool isTrue)
        {
            var bindingRight = new Binding();
            var bindingLeft = new Binding();
            var bindingDown = new Binding();

            if (isTrue)
            {
                bindingRight = new Binding
                {
                    Name = "bindingRight",
                    Object = 1,
                    Property = "rightTemperature",
                    Transformation = new Transformation
                    {
                        Min = 61,
                        Max = 100,
                    }
                };

                bindingLeft = new Binding
                {
                    Name = "bindingLeft",
                    Object = 1,
                    Property = "leftTemperature",
                    Transformation = new Transformation
                    {
                        Min = 80,
                        Max = 100
                    }
                };

                bindingDown = new Binding()
                {
                    Name = "bindingDown",
                    Object = 1,
                    Property = "downTemperature",
                    Transformation = new Transformation
                    {
                        Min = 55,
                        Max = 90
                    }
                };
            }
            else
            {
                bindingRight = new Binding
                {
                    Name = "bindingRight",
                    Object = 1,
                    Property = "rightTemperature",
                    Transformation = new Transformation
                    {
                        Min = 30,
                        Max = 60,
                    }
                };

                bindingLeft = new Binding
                {
                    Name = "bindingLeft",
                    Object = 1,
                    Property = "leftTemperature",
                    Transformation = new Transformation
                    {
                        Min = 40,
                        Max = 60
                    }
                };

                bindingDown = new Binding()
                {
                    Name = "bindingDown",
                    Object = 1,
                    Property = "downTemperature",
                    Transformation = new Transformation
                    {
                        Min = 10,
                        Max = 40
                    }
                };
            }

            ModifyBinding(bindingRight);
            ModifyBinding(bindingLeft);
            ModifyBinding(bindingDown);
        }

        /// <summary>
        /// Отправка запроса к серверу
        /// </summary>
        /// <param name="request"> запрос </param>
        private void SendRequest(JObject request)
        {
            var result = _connectionHandler.SendRequest(_client, request).Result;

            if (result.ContainsKey(Glossary.ErrorMessage))
            {
                throw new BadRequestException(result[Glossary.ErrorMessage].Value<string>());
            }
        }

        /// <summary>
        /// Возвращает объект JObject из Binding
        /// Стандартный JObject.FromObject не подходит, так как он не переводит из паскаль-кейс в кэмел-кейс
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private JObject BindingToJObject(Binding binding)
        {
            return new JObject
            {
                {"name", binding.Name },
                {"object", binding.Object },
                {"property", binding.Property },
                {"transformation", new JObject
                    {
                        {"min", binding.Transformation.Min },
                        {"max", binding.Transformation.Max }
                    }
                }
            };
        }
    }
}
