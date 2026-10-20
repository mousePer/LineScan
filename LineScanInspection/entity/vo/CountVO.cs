using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.entity.vo
{
    public class CountVO
    {
        public int Hour { get; set; }
        public int OK { get; set; }
        public int NG { get; set; }
        public int Total { get; set; }
    }
}
