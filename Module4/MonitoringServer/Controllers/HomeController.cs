using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Dapper;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using MonitoringServer.Infastructure;
using MonitoringServer.Models;
using Newtonsoft.Json;

namespace MonitoringServer.Controllers
{
    public class HomeController : Controller
    {
        private const string SqlServerDatabaseConnectionString =
                "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        public IActionResult Index()
        {
            var handlers = GetAll().ToList();
           

            var objects = handlers.Select(GetHandlerViewModel).ToList();

            return View(objects);
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

        private IEnumerable<HandlerModel> GetAll()
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Query<HandlerModel>("Select * From Handlers");
            }
        }

        private HandlerViewModel GetHandlerViewModel(HandlerModel handler)
        {
            var result = AppInsightsTelemetry.GetTelemetry
            ("eadcb792-3725-4a03-8799-8a883abbe833", "5f6bqdsymdwqxpiy7dib74v5z3m3dppjwdc87cga",
                $"timespan=PT1H&query=requests%7C%20where%20customDimensions.HandlerId%20%3D%3D%20%22{handler.Guid.ToString()}%22%20%7C%20summarize%20avg(duration)");

            var aiResult = JsonConvert.DeserializeObject<RootObject>(result);

            var duration = aiResult.Tables[0].Rows[0][0].ToString();
            return new HandlerViewModel
            {
                Guid = handler.Guid.ToString(),
                Duration = Convert.ToDouble(duration)
            };
        }
    }
}
