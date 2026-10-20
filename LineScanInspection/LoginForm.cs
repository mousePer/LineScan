using LineScanInspection.entity;
using LineScanInspection.Help;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineScanInspection
{
    public partial class LoginForm : UILoginForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public static string userInfoPath = AppDomain.CurrentDomain.BaseDirectory + "userInfo.ini";

        private bool LoginForm_OnLogin(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)|| string.IsNullOrEmpty(password))
            {
                this.ShowWarningTip("用户名或密码不能为空！");
                return false;
            }
            //查询数据库
            List<UserEntity> userEntities = SqlHelper.GetInstance().Queryable<UserEntity>().
                Where(x => x.UserName == userName && x.Password == password).ToList();
            if (userEntities.Count == 0)
            {
                this.ShowWarningTip("用户名或密码错误！");
                return false;
            }
            IniFile iniFile = new IniFile(userInfoPath);
            iniFile.Write("UserInfo", "userName", userName);
            iniFile.Write("UserInfo", "password", password);
            return true;
        } 
    }
}
