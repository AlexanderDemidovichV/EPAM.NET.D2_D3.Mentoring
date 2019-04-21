using System.Collections.Generic;

namespace MonitoringServer.Models
{
    public class Column
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string ColumnType { get; set; }
    }

    public class Table
    {
        public string TableName { get; set; }
        public List<Column> Columns { get; set; }
        public List<List<object>> Rows { get; set; }
    }

    public class RootObject
    {
        public List<Table> Tables { get; set; }
    }
}
