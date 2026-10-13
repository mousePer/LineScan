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

namespace LineScanInspection
{
    public partial class UserForm : UIForm
    {
        public UserForm()
        {
            InitializeComponent();
            //禁止datagridview自动生成列
            uiDataGridView1.AutoGenerateColumns = false;
            LoadData();
        }

        private void LoadData()
        {
            var result = SqlHelper.GetInstance().Queryable<UserEntity>().
                InnerJoin<UserAndAuthorityEntity>((u, ua) => u.UId == ua.UId).
                InnerJoin<AuthorityEntity>((u,ua, a) => ua.AId == a.AId).
                Select((u, ua, a) => new 
                {
                    UserName = u.UserName,
                    AccountNum = u.AccountNum,
                    Password = u.Password,
                    IOManage = a.IOManage,
                    LogManage = a.LogManage,
                    UserManage = a.UserManage,
                    CountManage = a.CountManage,
                    CameraManage = a.CameraManage,
                    AId = a.AId,
                    UId = u.UId
                }).ToList();

            uiDataGridView1.DataSource = null;
            uiDataGridView1.DataSource = result;

        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addUser_Click(object sender, EventArgs e)
        {
            using (EditUserForm editForm = new EditUserForm())
            {
                //将刷新界面的方法传递给编辑界面委托，编辑完成后调用
                editForm.editAction = LoadData;
                editForm.ShowDialog();
            }
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            //判断是否选中用户
            if(SelectRow() != 1)
            {
                this.ShowWarningTip("请选择修改用户");
                return;
            }
            using (EditUserForm editForm = new EditUserForm())
            {
                //获取当前选中行数据
                UserAndAuthorityVO userAndAuthorityVO = SelectRowItems();
                editForm.LoadData(userAndAuthorityVO);
                //将刷新界面的方法传递给编辑界面委托，编辑完成后调用
                editForm.editAction = LoadData;
                editForm.ShowDialog();
            }

        }
        /// <summary>
        /// 获取当前选中行数据
        /// </summary>
        private UserAndAuthorityVO SelectRowItems()
        {
            foreach (DataGridViewRow row in uiDataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    UserAndAuthorityVO userAndAuthorityVO = new UserAndAuthorityVO
                    {
                        UId = Convert.ToInt32(row.Cells["UId"].Value),
                        UserName = row.Cells["UserName"].Value.ToString(),
                        AccountNum = row.Cells["AccountNum"].Value.ToString(),
                        Password = row.Cells["Password"].Value.ToString(),
                        AId = Convert.ToInt32(row.Cells["AId"].Value),
                        IOManage = Convert.ToBoolean(row.Cells["IOManage"].Value),
                        LogManage = Convert.ToBoolean(row.Cells["LogManage"].Value),
                        UserManage = Convert.ToBoolean(row.Cells["UserManage"].Value),
                        CountManage = Convert.ToBoolean(row.Cells["CountManage"].Value),
                        CameraManage = Convert.ToBoolean(row.Cells["CameraManage"].Value)
                    };
                    return userAndAuthorityVO;
                }
            }
            return null;
        }

        /// <summary>
        /// 判断是否选中uiDataGridView1用户
        /// </summary>
        /// <returns></returns>
        private int SelectRow()
        {
            int count = 0;
            //遍历uiDataGridView1中的所有行
            foreach (DataGridViewRow row in uiDataGridView1.Rows)
            {
                //判断选择列是否选中  即第0列
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    return ++count;
                }
            }
            return count;

        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            if(SelectRow() != 1)
            {
                this.ShowWarningTip("请选择删除用户");
                return;
            }
            //=======》删除用户
            //获取当前选中行数据
            UserAndAuthorityVO userAndAuthorityVO = SelectRowItems();
            //开启事务
            SqlHelper.GetInstance().BeginTran();
            try
            {
                //删除用户权限关系
                SqlHelper.GetInstance().Deleteable<UserAndAuthorityEntity>().
                    Where(x => x.UId == userAndAuthorityVO.UId).ExecuteCommand();
                //删除用户权限信息
                SqlHelper.GetInstance().Deleteable<AuthorityEntity>().
                    Where(x => x.AId == userAndAuthorityVO.UId).ExecuteCommand();
                //删除用户信息
                SqlHelper.GetInstance().Deleteable<UserEntity>().
                    Where(x => x.UId == userAndAuthorityVO.UId).ExecuteCommand();
                //提交事务
                SqlHelper.GetInstance().CommitTran();
                //刷新数据
                LoadData();
                this.ShowSuccessTip("删除成功");
            }catch(Exception)
            {
                //回滚事务
                SqlHelper.GetInstance().RollbackTran();
                this.ShowWarningTip("删除失败");
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton4_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
