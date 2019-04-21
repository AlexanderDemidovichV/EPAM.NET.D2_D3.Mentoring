using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GeneratorTopshelfService
{
    public static class GeneratorHelper
    {
        private const string SqlServerDatabaseConnectionString =
            "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        public static GeneratorModel Find(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Query<GeneratorModel>("Select * From Generators " +
                                                "WHERE Guid = @Guid", new { guid }).SingleOrDefault();
            }
        }

        public static int Update(GeneratorModel generator)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "UPDATE Generators SET Delay = @Delay WHERE Guid = @Guid";
                var rowsAffected = db.Execute(sqlQuery, generator);
                return rowsAffected;
            }
        }

        public static GeneratorModel Create(GeneratorModel generator)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "INSERT Generators VALUES(@Guid, @Delay)";
                return db.Query<GeneratorModel>(sqlQuery, generator).SingleOrDefault();
            }
        }

        public static int Remove(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Execute("DELETE FROM Generators WHERE Guid = @Guid", new { guid });
            }
        }
    }
}
