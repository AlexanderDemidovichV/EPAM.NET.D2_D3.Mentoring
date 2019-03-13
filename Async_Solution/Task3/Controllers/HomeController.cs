using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task3.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string _syncPrefix = "sync_";
        private static readonly Dictionary<string, object> _syncObjects; // Simulate locking on a row.

        static HomeController()
        {
            _syncObjects = new Dictionary<string, object>();
            foreach (var value in Enum.GetValues(typeof(ItemType)))
            {
                _syncObjects.Add(_syncPrefix + (int)value, new object());
            }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public JsonResult Buy(ItemType item)
        {
            lock (_syncObjects[GetSyncKeyForItem(item)])
            {
                if (Session[item.ToString()] == null || (int) Session[item.ToString()] == 0)
                {
                    Session[item.ToString()] = 1;
                }
                    
            }
            return Json(Session[item.ToString()]);
        }

        public JsonResult Increase(ItemType item)
        {
            lock (_syncObjects[GetSyncKeyForItem(item)])
            {
                if (Session[item.ToString()] == null)
                {
                    Session[item.ToString()] = 1;
                }
                else
                {
                    Session[item.ToString()] = (int)Session[item.ToString()] + 1;
                }
            }
            return Json(Session[item.ToString()]);
        }

        public JsonResult Decrease(ItemType item)
        {
            lock (_syncObjects[GetSyncKeyForItem(item)])
            {
                if (Session[item.ToString()] == null || (int)Session[item.ToString()] == 1)
                {
                    Session[item.ToString()] = 0;
                }
                else
                {
                    Session[item.ToString()] = (int)Session[item.ToString()] - 1;
                }
            }
            return Json(Session[item.ToString()]);
        }

        private string GetSyncKeyForItem(ItemType item)
        {
            return _syncPrefix + (int)item;
        }
    }

    public enum ItemType
    {
        Pasta = 1,
        Burger = 2,
        Kebab = 3
    }
}
