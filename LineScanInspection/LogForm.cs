using LineScanInspection.entity;
using LineScanInspection.entity.vo;
using LineScanInspection.Help;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineScanInspection
{
    public partial class LogForm : UIForm
    {
        List<LogLevelVO> logLevelVOs = new List<LogLevelVO>();
        int pageIndex = 1;
        int pageSize = 15;
        int totalCount = 0;
        int totalPage = 0;
        public LogForm()
        {
            InitializeComponent();
            //关闭datagridview自动生成列
            uiDataGridView1.AutoGenerateColumns = false;
            //清空默认日期
            dp_createTime.Text = string.Empty;
            //========新增日期控件初始化========
            dp_createTime.ShowCheckBox = true;   //开启左侧复选框
            dp_createTime.Checked = false;        //默认未勾选，界面空白
            //加载日志等级
            LoadLogLevelCmb();
            //刷新数据
            LoadData();
        }
        /// <summary>
        /// 加载日志等级
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void LoadLogLevelCmb()
        {
            logLevelVOs.Add(new LogLevelVO() { LogLevelId = 1, LogName = "信息" });
            logLevelVOs.Add(new LogLevelVO() { LogLevelId = 2, LogName = "警告" });
            logLevelVOs.Add(new LogLevelVO() { LogLevelId = 3, LogName = "错误" });
            //设置数据源
            cb_logLevel.DataSource = null;
            cb_logLevel.DataSource = logLevelVOs;
            //设置显示和值
            cb_logLevel.DisplayMember = "LogName";
            cb_logLevel.ValueMember = "LogLevelId";
            //默认不选中
            cb_logLevel.SelectedIndex = -1;
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void LoadData()
        {
            //查询日志   （动态拼接条件）
            List<LogEntity> logEntities = SqlHelper.GetInstance().Queryable<LogEntity>().
                WhereIF(!string.IsNullOrEmpty(txt_userName.Text), x => x.UserName.Contains(txt_userName.Text)).  //拼接用户名
                WhereIF(dp_createTime.Checked, x => x.CreateTime >= DateTime.Parse(dp_createTime.Text) && x.CreateTime < DateTime.Parse(dp_createTime.Text).AddDays(1)).  //拼接时间
                WhereIF(cb_logLevel.SelectedIndex != -1, x => x.LogLevel == int.Parse(cb_logLevel.SelectedValue.ToString())).     //拼接日志等级
                ToPageList(pageIndex, pageSize, ref totalCount, ref totalPage)//返回Count+总页数
                  .ToList();
            uiDataGridView1.DataSource = null;
            uiDataGridView1.DataSource = logEntities;
            //显示提示语
            label4.Text = $"共{totalPage}页，当前第{pageIndex}页";
            label7.Text = $"共{totalCount}条";
        }
        //查询
        private void uiButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        //上一页
        private void uiButton3_Click(object sender, EventArgs e)
        {
            if(pageIndex == 1)
            {
                this.ShowWarningTip("已经是第一页了");
                return;
            }
            pageIndex--;
            LoadData();
        }
        //下一页
        private void uiButton4_Click(object sender, EventArgs e)
        {
            if(pageIndex == totalPage)
            {
                this.ShowWarningTip("已经是最后一页了");
                return;
            }
            pageIndex++;
            LoadData();
            }
        //页面跳转
        private void uiButton5_Click(object sender, EventArgs e)
        {
            if(!int.TryParse(txt_pateCount.Text, out int pageIndex))
            {
                this.ShowWarningTip("请输入页码或正确页码");
                return;
            }
            if (pageIndex > totalPage)
            {
                pageIndex = totalPage;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            this.pageIndex = pageIndex;
            LoadData();
        }
    
    }
}
