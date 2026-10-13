using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.entity.vo
{
    //用户信息与权限信息
    public class UserAndAuthorityVO
    {
        public int UId { get; set; }
        public string UserName { get; set; }
        public string AccountNum { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public int AId { get; set; }
        public bool IOManage { get; set; }
        public bool LogManage { get; set; }
        public bool CountManage { get; set; }
        public bool UserManage { get; set; }
        public bool CameraManage { get; set; }
    }
}
