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
    public partial class CountForm : UIForm
    {
        public CountForm()
        {
            InitializeComponent();
            LoadData();
            LoadData1();
        }
        
        public void LoadData()
        {
            List<ResultCountEntity> resultCountEntities = SqlHelper.GetInstance().
                Queryable<ResultCountEntity>().
                Where(x => x.ProductName == "OCR口红管").
                ToList();  

            UIBarOption option = new UIBarOption();
            option.Title = new UITitle();
            option.Title.Text = "产量统计";
            option.Title.SubText = "小时/产量";

            //设置Legend
            option.Legend = new UILegend();
            option.Legend.Orient = UIOrient.Horizontal;
            option.Legend.Top = UITopAlignment.Top;
            option.Legend.Left = UILeftAlignment.Left;
            option.Legend.AddData("产量");

            List<CountVO> countVOs = resultCountEntities.GroupBy(x => x.CreateTime.Hour).Select(x => new CountVO()
            {
                Hour = x.Key,
                Total = x.Count(),
                OK = x.Count(y => y.Result == "OK"),
                NG = x.Count(y => y.Result == "NG")
            }).OrderBy(x=>x.Hour).ToList();
            //渲染数据
            uiTextBox2.Text = countVOs.Sum(x=>x.Total).ToString();
            uiTextBox3.Text = countVOs.Sum(x=>x.OK).ToString();
            uiTextBox4.Text = countVOs.Sum(x=>x.NG).ToString();

            var series = new UIBarSeries();
            series.Name = "产量";
            foreach (var item in countVOs)
            {
                series.AddData(item.Total,Color.Blue);
                option.XAxis.Data.Add(item.Hour.ToString()+":00");
            }

            ////数据显示小数位数
            //series.DecimalPlaces = 1;
            option.Series.Add(series);

            option.ToolTip.Visible = true;
            option.YAxis.Scale = true;

            option.XAxis.Name = "日期";
            option.XAxis.AxisLabel.Angle = 60;//(0° ~ 90°)

            option.YAxis.Name = "数值";

            //坐标轴显示小数位数
            option.YAxis.AxisLabel.DecimalPlaces = 1;

            option.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Red, Name = "上限", Value = 50 });
            option.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Gold, Name = "下限", Value = 30 });

            option.ToolTip.AxisPointer.Type = UIAxisPointerType.Shadow;

            option.ShowValue = true;

            uiBarChart1.SetOption(option);

        }

        public void LoadData1()
        {
            List<ResultCountEntity> resultCountEntities = SqlHelper.GetInstance().
                Queryable<ResultCountEntity>().
                Where(x => x.ProductName == "OCR口红管").
                ToList();

            var option = new UIDoughnutOption();

            //设置Title
            option.Title = new UITitle();
            option.Title.Text = "产量统计";
            option.Title.SubText = "小时/产量";
            option.Title.Left = UILeftAlignment.Center;

            //设置ToolTip
            option.ToolTip.Visible = true;

            //设置Legend
            option.Legend = new UILegend();
            option.Legend.Orient = UIOrient.Vertical;
            option.Legend.Top = UITopAlignment.Top;
            option.Legend.Left = UILeftAlignment.Left;

            List<CountVO> countVOs = resultCountEntities.GroupBy(x => x.CreateTime.Hour).Select(x => new CountVO()
            {
                Hour = x.Key,
                Total = x.Count(),
                OK = x.Count(y => y.Result == "OK"),
                NG = x.Count(y => y.Result == "NG")
            }).OrderBy(x => x.Hour).ToList();
            //设置Series
            var series = new UIDoughnutSeries();
            series.Name = "StarCount";
            series.Center = new UICenter(50, 55);
            series.Radius.Inner = 40;
            series.Radius.Outer = 70;
            series.Label.Show = true;
            series.Label.Position = UIPieSeriesLabelPosition.Center;
            //增加Series
            option.Series.Clear();
            option.Series.Add(series);
            foreach (var item in countVOs)
            {
                option.Legend.AddData(item.Hour + ":00");
                //增加数据
                series.AddData(item.Hour + ":00", item.Total);
            }
            //显示数据小数位数
            option.DecimalPlaces = 1;

            ////设置Option
            uiDoughnutChart1.SetOption(option);
            //uiSymbolButton2.Enabled = true;

        }



    }
}
