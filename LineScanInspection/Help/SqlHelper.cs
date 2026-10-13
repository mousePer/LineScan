using SqlSugar;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.Help
{
    public class SqlHelper
    {
        //通过懒汉模式实现单例模式
        private SqlHelper()
        {
        }
        private static SqlSugarClient Instance = null;
        //创建一个对象锁
        private static object obj = new object();
        public static SqlSugarClient GetInstance()
        {
            if (Instance == null)
            {
                lock (obj)
                {
                    if (Instance == null)
                    {
                        Instance = new SqlSugarClient(new ConnectionConfig()
                        {
                            ConnectionString = "Server=.; Database=LineScan; User Id=sa; Password=123; TrustServerCertificate=True;",
                            DbType = DbType.SqlServer,
                            IsAutoCloseConnection = true
                        });
                        Instance.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            string nativeSql = UtilMethods.GetNativeSql(sql, pars);
                            //调试输出
                            Console.WriteLine($"[SQL] {nativeSql}");
                        };

                        Instance.Aop.OnError = ex =>
                        {
                            Console.WriteLine($"[SQL异常] {ex.Message}");
                        };
                    }
                    return Instance;
                }
            }
            
            return Instance;
        }


       
}
}
