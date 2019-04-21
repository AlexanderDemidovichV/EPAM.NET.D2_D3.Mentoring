using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MonitoringServer.Models;

namespace MonitoringServer.Infastructure
{
    public static class GeneratorHelper
    {
        public static IEnumerable<GeneratorModel> GetAll()
        {
            using (IDbConnection db = new SqlConnection(
                HandlerHelper.SqlServerDatabaseConnectionString))
            {
                return db.Query<GeneratorModel>("Select * From Generators").ToList();
            }
        }

        public static IEnumerable<Entity> GetGeneratorViewModels(IEnumerable<GeneratorModel> generatorModels)
        {
            return generatorModels.Select(m => new GeneratorViewModel { 
                Guid = m.Guid.ToString(), 
                Duration = m.Delay});
        }
    }
}
