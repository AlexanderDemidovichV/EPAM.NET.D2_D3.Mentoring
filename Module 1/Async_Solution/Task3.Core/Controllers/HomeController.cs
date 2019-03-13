using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task3.Core.Models;

namespace Task3.Core.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string _syncPrefix = "sync_";
        private static readonly Dictionary<string, object> _syncObjects; 
        private ISession Session => this.HttpContext?.Session;

        static HomeController()
        {
            _syncObjects = new Dictionary<string, object>();
            foreach (var value in Enum.GetValues(typeof(ItemType)))
            {
                _syncObjects.Add(_syncPrefix + (int)value, new object());
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Buy(ItemType item)
        {
            lock (_syncObjects[GetSyncKeyForItem(item)])
            {
                if (Session.GetInt32(item.ToString()) == null 
                    || Session.GetInt32(item.ToString()) == 0)
                    Session.SetInt32(item.ToString(), 1);
            }
            return Json(Session.GetInt32(item.ToString()));
        }

        public JsonResult Increase(ItemType item)
        {
            lock (_syncObjects[GetSyncKeyForItem(item)])
            {
                if (Session.GetInt32(item.ToString()) == null)
                {
                    Session.SetInt32(item.ToString(), 1);
                }
                else
                {
                    Session.SetInt32(item.ToString(), 
                        (int)Session.GetInt32(item.ToString()) + 1);
                }
            }
            return Json(Session.GetInt32(item.ToString()));
        }

        public JsonResult Decrease(ItemType item)
        {
            lock (_syncObjects[GetSyncKeyForItem(item)])
            {
                if (Session.GetInt32(item.ToString()) == null
                    || Session.GetInt32(item.ToString()) == 1)
                {
                    Session.SetInt32(item.ToString(), 0);
                }
                else
                {
                    Session.SetInt32(item.ToString(),
                        (int)Session.GetInt32(item.ToString()) - 1);
                }
            }
            return Json(Session.GetInt32(item.ToString()));
        }

        public JsonResult GetTotal()
        {
            lock (_syncObjects[GetSyncKeyForItem(ItemType.Burger)])
                lock (_syncObjects[GetSyncKeyForItem(ItemType.Pasta)])
                    lock (_syncObjects[GetSyncKeyForItem(ItemType.Pizza)])
                    {
                        var total = (Session.GetInt32(ItemType.Burger.ToString()) ?? 0) +
                                    (Session.GetInt32(ItemType.Pizza.ToString()) ?? 0) +
                                    (Session.GetInt32(ItemType.Pasta.ToString()) ?? 0);
                        return Json(total);
                    }
        }

        private string GetSyncKeyForItem(ItemType item)
        {
            return _syncPrefix + (int)item;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
