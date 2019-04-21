using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace HandlerTopshelfService
{
    public static class HandlerHelper
    {
        private const string SqlServerDatabaseConnectionString =
            "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static HandlerModel Find(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Query<HandlerModel>("Select * From Handlers " +
                                              "WHERE Guid = @Guid", new { guid }).SingleOrDefault();
            }
        }

        public static int Update(HandlerModel handler)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "UPDATE Handlers SET Delay = @Delay, " +
                               " Status = @Status " + "WHERE Guid = @Guid";
                var rowsAffected = db.Execute(sqlQuery, handler);
                return rowsAffected;
            }
        }

        public static HandlerModel Create(HandlerModel handler)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "INSERT Handlers VALUES(@Guid, @Status, @Delay)";
                return db.Query<HandlerModel>(sqlQuery, handler).SingleOrDefault();
            }
        }

        public static int Remove(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Execute("DELETE FROM Handlers WHERE Guid = @Guid", new { guid });
            }
        }
    }
}
