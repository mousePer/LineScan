using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace LineScanInspection.entity
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    [SugarTable(TableName ="user")]
    public class UserEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //数据库是自增才配自增
        public int UId { get; set; }
        public string UserName { get; set; }
        public string AccountNum { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
