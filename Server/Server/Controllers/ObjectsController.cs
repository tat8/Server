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
    public class ObjectsController : Controller
    {
        private IDataObjectService _dataObjectService;
        private ILogger _logger;

        public ObjectsController(IDataObjectService dataObjectService, ILoggerFactory loggerFactory)
        {
            _dataObjectService = dataObjectService;
            _logger = loggerFactory.CreateLogger("ObjectsControllerExceptionsLogger");
        }

        /// <summary>
        /// Возвращает текущее состояние всех объектов со всеми параметрами
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            JsonResult result;
            try
            {
                result = Json(Ok(_dataObjectService.GetObjects()));
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
        /// Возвращает набор состояний всех объектов со всеми параметрами за последние 5 минут (или меньше, если еще не накопилось)
        /// с указанием времени для каждого состояния
        /// </summary>
        /// <returns></returns>
        [HttpGet("states")]
        public IActionResult GetStates()
        {
            JsonResult result;
            try
            {
                result = Json(Ok(_dataObjectService.GetObjectsStates()));
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
        /// Возвращает текущее состояние объектов только тех параметров, которые изменились с предыдущего запроса,
        /// или все параметры, если это первый запрос
        /// </summary>
        /// <returns></returns>
        [HttpGet("compact")]
        public IActionResult GetCompactStates()
        {
            JsonResult result;
            try
            {
                result = Json(Ok(_dataObjectService.GetObjectsCompact()));
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
        /// Добавление объекта данных
        /// </summary>
        /// <param name="dataObject"> объект для добавления </param>
        /// <returns></returns>
        [HttpPost("addObject")]
        public IActionResult Add([FromForm] DataObject dataObject)
        {
            JsonResult result;
            try
            {
                var dataObjectResult = _dataObjectService.AddObject(dataObject);
                result = Json(Ok(dataObjectResult));
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
        /// Удаление объекта данных
        /// </summary>
        /// <param name="dataObject"> объект для удаления </param>
        /// <returns></returns>
        [HttpPost("removeObject")]
        public IActionResult Remove([FromForm] DataObject dataObject)
        {
            JsonResult result;
            try
            {
                _dataObjectService.RemoveObject(dataObject);
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