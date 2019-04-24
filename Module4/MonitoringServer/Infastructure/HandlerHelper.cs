using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MonitoringServer.Models;
using Newtonsoft.Json;

namespace MonitoringServer.Infastructure
{
    public static class HandlerHelper
    {
        public const string SqlServerDatabaseConnectionString =
            "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private const string AppInsightsAppId = "eadcb792-3725-4a03-8799-8a883abbe833";
        private const string AppInsightsApiKey = "5f6bqdsymdwqxpiy7dib74v5z3m3dppjwdc87cga";

        public static IEnumerable<HandlerModel> GetAll()
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Query<HandlerModel>("Select * From Handlers").ToList();
            }
        }

        public static int Update(Guid guid, int delay)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "UPDATE Handlers SET Delay = @Delay WHERE Guid = @Guid";
                var rowsAffected = db.Execute(sqlQuery, new HandlerModel { Guid = guid, Delay = delay });
                return rowsAffected;
            }
        }

        public static Entity GetHandlerViewModel(HandlerModel handler)
        {
            var duration = GetHandlerDuration(handler);

            return new HandlerViewModel
            {
                Guid = handler.Guid.ToString(),
                Duration = Convert.ToDouble(duration)
            };
        }

        private static string GetHandlerDuration(HandlerModel handler)
        {
            var result = AppInsightsTelemetry.GetTelemetry
            (AppInsightsAppId, AppInsightsApiKey,
                $"timespan=PT1H&query=requests%7C%20where%20customDimensions.HandlerId%20%3D%3D%20%22{handler.Guid.ToString()}%22%20%7C%20summarize%20avg(duration)");

            var aiResult = JsonConvert.DeserializeObject<RootObject>(result);

            var duration = aiResult.Tables[0].Rows[0][0].ToString();

            return duration;
        }
    }
}
