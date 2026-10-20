using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.entity
{
    [SugarTable(TableName = "ResultCount")]
    public class ResultCountEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //数据库是自增才配自增
        public int CountId { get; set; }
        public string ProductName { get; set; }
        public string Result { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
