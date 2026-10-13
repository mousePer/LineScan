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
using LineScanInspection.entity;
using LineScanInspection.Help;
using LineScanInspection.entity.vo;
using System.Threading;

namespace LineScanInspection
{
    public partial class EditUserForm : UIForm
    {
        public EditUserForm()
        {
            InitializeComponent();
        }
        //定义刷新界面委托
        public Action editAction;

        public UserAndAuthorityVO VO;

        public void LoadData(UserAndAuthorityVO userAndAuthorityVO)
        { 
            Txb_userName.Text = userAndAuthorityVO.UserName;
            Txb_account.Text = userAndAuthorityVO.AccountNum;
            Txb_password.Text = userAndAuthorityVO.Password;
            Txb_checkPassword.Text = userAndAuthorityVO.Password;
            cb_io.Checked = userAndAuthorityVO.IOManage;
            cb_log.Checked = userAndAuthorityVO.LogManage;
            cb_yield.Checked = userAndAuthorityVO.CountManage;
            cb_user.Checked = userAndAuthorityVO.UserManage;
            cb_camera.Checked = userAndAuthorityVO.CameraManage;
            //账号禁止修改   账号框使能关闭
            Txb_account.Enabled = false;
            VO = userAndAuthorityVO;
        }

        //保存与保存用户信息用户信息
        private void btn_saveUser_Click(object sender, EventArgs e)
        {
            foreach (Control control in uiGroupBox1.Controls)
            {
                if (!(control is UITextBox))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(control.Text))
                {
                    this.ShowWarningTip(control.Tag.ToString());
                    return;
                }
            }
            if (Txb_checkPassword.Text != Txb_password.Text)
            {
                this.ShowWarningTip("两次输入的密码不一致");
                return;
            }
            
            //当VO为空时，表示调开该窗口没有传入用户数据，代表是新增用户操作
            if (VO == null)
            {
                ///新增用户
                saveUser();
            }
            else
            {
                //修改用户信息
                editUser();
            }



            
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void editUser()
        {
            //开启事务
            SqlHelper.GetInstance().BeginTran();
            try
            {
                //修改用户信息表
                SqlHelper.GetInstance().Updateable<UserEntity>(new UserEntity
                {
                    UId = VO.UId,
                    UserName = Txb_userName.Text,
                    Password = Txb_password.Text,
                    AccountNum = Txb_account.Text,
                    UpdateTime = DateTime.Now
                }).ExecuteCommand();
                //修改权限信息表
                SqlHelper.GetInstance().Updateable<AuthorityEntity>(new AuthorityEntity
                {
                    AId = VO.AId,
                    IOManage = cb_io.Checked,
                    LogManage = cb_log.Checked,
                    UserManage = cb_user.Checked,
                    CountManage = cb_yield.Checked,
                    CameraManage = cb_camera.Checked,
                }).ExecuteCommand();

                //提交事务
                SqlHelper.GetInstance().CommitTran();
                
                this.ShowSuccessTip("修改成功");
                Thread.Sleep(800);
                editAction.Invoke();
                this.Close();
            }
            catch (Exception)
            {
                //回滚事务
                SqlHelper.GetInstance().RollbackTran();
                this.ShowErrorTip("修改失败");
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        private void saveUser()
        {
            //验证用户名是否已存在
            if (SqlHelper.GetInstance().Queryable<UserEntity>().Where(x => x.AccountNum == Txb_account.Text).ToList().Count > 0)
            {
                this.ShowWarningTip("用户已存在");
                return;
            }
            //开启事务
            SqlHelper.GetInstance().BeginTran();
            try
            {
                //插入用户信息
                int uid = SqlHelper.GetInstance().Insertable<UserEntity>(new UserEntity
                {
                    UserName = Txb_userName.Text,
                    Password = Txb_password.Text,
                    AccountNum = Txb_account.Text,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }).ExecuteReturnIdentity();

                //插入用户权限信息
                int aid = SqlHelper.GetInstance().Insertable<AuthorityEntity>(new AuthorityEntity
                {
                    IOManage = cb_io.Checked,
                    LogManage = cb_log.Checked,
                    UserManage = cb_user.Checked,
                    CountManage = cb_yield.Checked,
                    CameraManage = cb_camera.Checked,
                }).ExecuteReturnIdentity();

                //插入用户权限关系
                int result = SqlHelper.GetInstance().Insertable<UserAndAuthorityEntity>(new UserAndAuthorityEntity
                {
                    UId = uid,
                    AId = aid
                }).ExecuteCommand();
                if (result > 0)
                {
                    this.ShowSuccessTip("保存成功");
                    //提交事务
                    SqlHelper.GetInstance().CommitTran();
                    //调用委托刷新主页用户界面
                    editAction.Invoke();
                    this.Close();
                }
            }
            catch (Exception)
            {
                //回滚事务
                SqlHelper.GetInstance().RollbackTran();
            }
        }

        /// <summary>
        /// 全选/全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            foreach (var item in uiGroupBox2.Controls)
            {
                if(!(item is UICheckBox))
                {
                    continue;
                }
                UICheckBox checkBox = item as UICheckBox;
                checkBox.Checked = !cb_io.Checked;
            }
        }
        /// <summary>
        /// 取消（关闭窗口）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancelUser_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
