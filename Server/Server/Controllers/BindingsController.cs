using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubius.DevSaunaB.Server.Exceptions;
using Rubius.DevSaunaB.Server.Models;
using Rubius.DevSaunaB.Server.Services;

namespace Rubius.DevSaunaB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindingsController : Controller
    {
        private IBindingService _bindingService;
        private ILogger _logger;

        public BindingsController(IBindingService bindingService, ILoggerFactory loggerFactory)
        {
            _bindingService = bindingService;
            _logger = loggerFactory.CreateLogger("BindingsControllerExceptionsLogger");
        }


        [HttpPost("mode")]
        public IActionResult SetEmergencyMode([FromForm] Mode mode)
        {
            JsonResult result;
            try
            {
                _bindingService.SetEmergencyMode(mode.IsEmergencyMode);
                result = Json(Ok());
            }
            catch (BadRequestException e)
            {
                result = Json(BadRequest(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(BadRequest());
            }

            return result;
        }


        /// <summary>
        /// Добавление привязки параметра к объекту
        /// </summary>
        /// <param name="binding"> привязка для добавления </param>
        /// <returns></returns>
        [HttpPost("addBinding")]
        public IActionResult Add([FromForm] Binding binding)
        {
            JsonResult result;
            try
            {
                _bindingService.AddBinding(binding);
                result = Json(Ok());
            }
            catch (BadRequestException e)
            {
                result = Json(BadRequest(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(BadRequest());
            }

            return result;
        }

        /// <summary>
        /// Изменение привязки параметра к объекту
        /// </summary>
        /// <param name="binding"> новая привзяка параметра к объекту </param>
        /// <returns></returns>
        [HttpPost("modifyBinding")]
        public IActionResult Modify([FromForm] Binding binding)
        {
            JsonResult result;
            try
            {
                _bindingService.ModifyBinding(binding);
                result = Json(Ok());
            }
            catch (BadRequestException e)
            {
                result = Json(BadRequest(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(BadRequest());
            }

            return result;
        }

        /// <summary>
        /// Удаление привязки параметра к объекту
        /// </summary>
        /// <param name="binding"> привязка для удаления </param>
        /// <returns></returns>
        [HttpPost("removeBinding")]
        public IActionResult Remove([FromForm] Binding binding)
        {
            JsonResult result;
            try
            {
                _bindingService.RemoveBinding(binding);
                result = Json(Ok());
            }
            catch (BadRequestException e)
            {
                result = Json(BadRequest(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(BadRequest());
            }

            return result;
        }
    }
}