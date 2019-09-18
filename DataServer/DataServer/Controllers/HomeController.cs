using Microsoft.AspNetCore.Mvc;
using Rubius.DevSaunaB.DataServer.Services;

namespace Rubius.DevSaunaB.DataServer.Controllers
{
    public class HomeController : Controller
    {
        private SocketStateHolder _socketStateHolder;

        public HomeController(SocketStateHolder socketStateHolder)
        {
            _socketStateHolder = socketStateHolder;
        }

        public IActionResult Index()
        {
            return View(_socketStateHolder.GetStates());
        }
    }
}