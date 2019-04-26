using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MonitoringServer.Infastructure;
using MonitoringServer.Models;

namespace MonitoringServer.Controllers
{
    public class HomeController : Controller
    {
        private IList<HandlerViewModel> handlers;

        public IActionResult Index()
        {
            var handlers = HandlerHelper.GetAll();
            var generators = GeneratorHelper.GetAll();

            var objects = handlers.Select(HandlerHelper.GetHandlerViewModel).ToList();
            var handlerStatuses = TimedHostedService.HandlerStatuses;
            foreach (HandlerViewModel handler in objects)
            {
                var model = handlerStatuses.Single(h => h.Key == handler.Guid).Value;
                handler.Status = model.Status;
                handler.LastUpdated = model.LastUpdated;
            }

            objects.AddRange(GeneratorHelper.GetGeneratorViewModels(generators));
            

            return View(objects);
        }

        [HttpPost]
        public IActionResult ChangeHandlerDelay(int delay, string guid)
        {
            HandlerHelper.Update(new Guid(guid), delay);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeGeneratorDelay(int delay, string guid)
        {
            GeneratorHelper.Update(new Guid(guid), delay);
            return RedirectToAction("Index");
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
