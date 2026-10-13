using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.entity
{
    /// <summary>
    /// 权限表
    /// </summary>
    [SugarTable(TableName ="authority")]
    public class AuthorityEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //数据库是自增才配自增
        public int AId { get; set; }
        public bool IOManage { get; set; }
        public bool LogManage { get; set; }
        public bool CountManage { get; set; }
        public bool UserManage { get; set; }
        public bool CameraManage { get; set; }
    }
}
