using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.Help
{
    public class AppResultHelper<T>
    {
        public bool isSuccessful { get; set; }

        public string message { get; set; }

        public T data { get; set; }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="appResultHelper"></param>
        /// <param name="data"></param>
        public static AppResultHelper<T> Success( T data)
        {
            return new AppResultHelper<T>()
            {
                isSuccessful = true,
                message = "成功",
                data = data
            };
        }


        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static AppResultHelper<T> Success()
        {
            return new AppResultHelper<T>()
            {
                isSuccessful = true,
                message = "成功"
            };
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="appResultHelper"></param>
        /// <param name="message"></param>
        public static AppResultHelper<T> Fail(string msg)
        {
            return new AppResultHelper<T>()
            {
                isSuccessful = false,
                message = msg
            };
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="appResultHelper"></param>
        /// <param name="message"></param>
        public static AppResultHelper<T> Fail(string msg,T data)
        {
            return new AppResultHelper<T>()
            {
                isSuccessful = false,
                message = msg,
                data = data
            };
        }
        
    }
}
