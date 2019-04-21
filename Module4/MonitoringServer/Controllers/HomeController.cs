using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MonitoringServer.Infastructure;
using MonitoringServer.Models;

namespace MonitoringServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var handlers = HandlerHelper.GetAll();
            var generators = GeneratorHelper.GetAll();

            var objects = handlers.Select(HandlerHelper.GetHandlerViewModel).ToList();
            objects.AddRange(GeneratorHelper.GetGeneratorViewModels(generators));

            return View(objects);
        }

        public IActionResult ChangeHandlerDelay(string guid, int newDelay)
        {
            HandlerHelper.Update(new Guid(guid), newDelay);
            RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        
    }
}
