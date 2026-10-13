using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.entity
{
    /// <summary>
    /// 日志实体类
    /// </summary>
    [SugarTable(TableName = "log")]
    public class LogEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //数据库是自增才配自增
        public int LogId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public int LogLevel { get; set; }
        public string SourceDevice { get; set; }
        public string LogCentent { get; set; }
    }
}
