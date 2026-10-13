using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace LineScanInspection.entity
{
    [SugarTable(TableName ="user_authority")]
    public class UserAndAuthorityEntity
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true)] //数据库是自增才配自增
        public int MId { get; set; }
        public int AId { get; set; }
        public int UId { get; set; }
    }
}
