using LineScanInspection.entity;
using LineScanInspection.entity.vo;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LineScanInspection
{
    public partial class FormMain : UIForm
    {
        public UserEntity userEntity = new UserEntity();
        // string pageName;   构建页面字典
        public Dictionary<string,UIForm> pageDic = new Dictionary<string, UIForm>();
        public FormMain()
        {
            InitializeComponent();
            IniFile file = new IniFile(LoginForm.userInfoPath);
            userEntity.UserName = file.Read("UserInfo", "userName", "");
            userEntity.Password = file.Read("UserInfo", "password", "");
            //默认导航入口，去产量页面
            GoPage(nameof(CountForm));
        }
        //当前所在页面
        public string CurrentPage { get; set; }
        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <param name="formName">窗体名称</param>
        private void GoPage(string formName)
        {
            if (string.IsNullOrEmpty(formName))
            {
                UIMessageBox.Show("未配置目标页面");
                return;
            }
            switch (formName)
            {
                case nameof(CountForm):
                case nameof(CameraForm):
                case nameof(LogForm):
                case nameof(UserForm):
                    //这些页面是已经实现了的，所以直接跳转
                    break;
                default:
                    UIMessageBox.Show("未配置目标页面");
                    return;
            }
            if (CurrentPage == formName)
            {
                return;
            }
            UIForm page;
            try
            {
                //获取权限
                UserAndAuthorityVO userAndAuthority = GetUserAuthority();
                if (userAndAuthority == null)
                {
                    UIMessageBox.Show("未找到用户权限，请重新登录或者联系管理员");
                    return;
                }
                //检查权限
                if (!checkAuthority(userAndAuthority,formName))
                {
                    this.ShowWarningTip("无权限访问");
                    return;
                }
                page = GetOrCreateForm(formName);

            }
            catch (Exception ex)
            {
                UIMessageBox.Show("未配置目标页面");
                return;
            }
            //暂停容器布局，再一次布局更新中完成页面替换
            panelMain.SuspendLayout();
            try
            {
                //清空容器  clear  只将旧页面移除容器，不会释放对象，缓存依然保存该窗体供下次使用
                panelMain.Controls.Clear();
                //添加页面
                panelMain.Controls.Add(page);
                page.Show();
                CurrentPage = formName;
            }catch(Exception)
            {
                return;
            }finally
            {
                //恢复布局
                panelMain.ResumeLayout();
            }

        }
        /// <summary>
        /// 获取页面
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private UIForm GetOrCreateForm(string formName)
        {
            if(!pageDic.TryGetValue(formName,out UIForm page) || page.IsDisposed)
            {
                switch(formName)
                {
                    case nameof(CountForm):
                        page = new CountForm();
                        break;
                    case nameof(CameraForm):
                        page = new CameraForm();
                        break;
                    case nameof(LogForm):
                        page = new LogForm();
                        break;
                    case nameof(UserForm):    
                        page = new UserForm();
                        break;
                    default:
                        //防止以后其他地方直接调用本函数时，传入不支持的页面名称。
                        throw new ArgumentException  ("该页面暂未开放",nameof(formName));
                }
                //禁用顶级窗口模式，才能将Form做为控件添加进
                page.TopLevel = false;
                //让页面随容器大小变化，并填充容器
                page.Dock = DockStyle.Fill;
                //保存新实例，如果旧实例已被释放，这里回替换旧的缓存值
                pageDic[formName] = page;
            }
            //缓存命中且未被释放时直接返回原对象
            return page;
            
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="userAndAuthority"></param>
        /// <returns></returns>
        private bool checkAuthority(UserAndAuthorityVO userAndAuthority,string formName)
        {
            switch (formName)
            {
                case nameof(CountForm):
                    return userAndAuthority.CountManage;
                case nameof(CameraForm):
                    return userAndAuthority.CameraManage;
                case nameof(LogForm):
                    return userAndAuthority.LogManage;
                case nameof(UserForm):
                    return userAndAuthority.UserManage;
                    default:
                    return false;
            }
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <returns></returns>
        private UserAndAuthorityVO GetUserAuthority()
        {
            List<UserAndAuthorityVO> userAndAuthorityVOs = SqlHelper.GetInstance().Queryable<UserEntity>().
                InnerJoin<UserAndAuthorityEntity>((u,ua)=>u.UId==ua.UId).
                InnerJoin<AuthorityEntity>((u,ua,a)=>ua.AId==a.AId).
                Where((u,ua,a)=>u.UserName == userEntity.UserName&&u.Password == userEntity.Password).
                Select((u,ua,a)=>new UserAndAuthorityVO
                {
                    AId = a.AId,
                    UId = u.UId,
                    UserName = u.UserName,
                    AccountNum = u.AccountNum,
                    Password = u.Password,
                    IOManage = a.IOManage,
                    LogManage = a.LogManage,
                    CountManage = a.CountManage,
                    UserManage = a.UserManage,
                    CameraManage = a.CameraManage
                }).ToList();
            return userAndAuthorityVOs.FirstOrDefault();

        }

        private void uiHeaderButton1_Click(object sender, EventArgs e)
        {
            UIHeaderButton uIHeaderButton = sender as UIHeaderButton;
            GoPage(uIHeaderButton?.Tag.ToString());
        }
    }
}
