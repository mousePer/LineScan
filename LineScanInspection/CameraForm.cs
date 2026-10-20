using HalconDotNet;
using LineScanInspection.Help;
using MvCameraControl;
using Newtonsoft.Json.Linq;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineScanInspection
{
    
    public partial class CameraForm : UIForm
    {
        public HalconHelper halconHelper;
        public CameraForm()
        {
            InitializeComponent();
            if (hKServerHelper == null)
            {
                hKServerHelper = new HKServerHelper();
                hKServerHelper.showImageCallBack = showImageCallBack;
            }
            //实例化halcon帮助类
            halconHelper = new HalconHelper();
            BandHalconHandler();

        }
        /// <summary>
        /// 绑定Halcon句柄
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void BandHalconHandler()
        {
            // 创建一个 Halcon 图形窗口，并把 pictureBox2 作为它的父窗口（传入其句柄），
            // 使该 Halcon 窗口"嵌入"pictureBox2 —— 之后 Halcon 画的内容就直接显示在 pictureBox2 上
            HOperatorSet.OpenWindow(0, 0, pictureBox2.Width - 1, pictureBox2.Height - 1, pictureBox2.Handle,
                                    "visible", "", out hv_WindowHandle);
            HDevWindowStack.Push(hv_WindowHandle);
            HOperatorSet.SetWindowAttr("background_color", "black");

        }

        /// <summary>
        /// 将相机抓取到的图片显示在实时图像pictureBox1中
        /// </summary>
        /// <param name="device">连接的相机本身</param>
        /// <param name="iFrameOut">相机取到的最新的一帧</param>
        /// <exception cref="NotImplementedException"></exception>
        private void showImageCallBack(IDevice device, IFrameOut iFrameOut)
        {
            //由于这个方法时候取帧的那个线程被调用，所以不能在里面直接显示图片，通过Invoke方法跳回主线程显示图片
            this.Invoke(new Action(() =>
            {
                device.ImageRender.DisplayOneFrame(pictureBox1.Handle, iFrameOut.Image);
            }));
        }

        public HKServerHelper hKServerHelper;

        /// <summary>
        /// 打开相机（在打开相机之后，是直接默认开启了采集功能）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_Click(object sender, EventArgs e)
        {
            
            AppResultHelper<List<string>> result = hKServerHelper.GetDevice();
            if (!result.isSuccessful)
            {
                this.ShowWarningTip(result.message);
                return;
            }
            if(result.data.Count == 0)
            {
                this.ShowWarningTip("没有可用的相机");
                return;
            }
            //由于没有设置选择相机的列表，所以默认选择第一个相机
            if (!hKServerHelper.OpenDevice(result.data[0]).isSuccessful)
            {
                this.ShowWarningTip("打开相机失败");
                return;
            }
            //开启采集功能并判断是否成功
            if (!hKServerHelper.StartGrabbing().isSuccessful)
            {
                this.ShowWarningTip("开始抓图失败");
                return;
            }

            //连接plc
            if (!PlcHelper.instance.Connect().isSuccessful)
            {
                this.ShowWarningTip("连接PLC失败");
                return;
            }
            Thread thread = new Thread(monitorAsync);
            thread.IsBackground = true;
            thread.Start();

        }
        /// <summary>
        /// 监控plc的状态
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private  async void monitorAsync()
        {
            while (PlcHelper.instance.isConnect)
            {
                //相机是否打开
                if (!hKServerHelper.Acquisition || !hKServerHelper.Enabled)
                {
                    continue;
                }
                //通过采集PLC信号 触发相机拍照
                AppResultHelper<ushort> result = PlcHelper.instance.ReadPlc();
                if (!result.isSuccessful)
                {
                    this.ShowWarningTip("读取PLC失败");
                    continue;
                }
                ushort readValues = result.data;
                if(readValues == 16)
                {
                    //获取当前照片
                    DisplayHalconImage();
                    Thread.Sleep(1500);
                    PlcHelper.instance.writePlc(32);
                    //Thread.Sleep(1000);
                    if (!int.TryParse(uiTextBox1.Text, out int upValue))
                    {
                        this.ShowWarningTip("请填写正确上限值");
                    }
                    if (!int.TryParse(uiTextBox2.Text, out int downValue))
                    {
                        this.ShowWarningTip("请填写正确下限值");
                    }
                    AppResultHelper<string> resultHelper = await halconHelper.ReadImageOcr(halconImage, hv_WindowHandle, upValue, downValue);
                    if (!resultHelper.isSuccessful)
                    {
                        this.Invoke(new Action(() =>
                        {
                            //显示错误信息到listView1
                            listView1.Items.Add(new ListViewItem(new string[] { "错误", DateTime.Now.ToString(), resultHelper.message }));
                        }));
                        return;
                    }
                    this.Invoke(new Action(() =>
                    {
                        //显示完成信息到listView1
                        listView1.Items
                            .Add(new ListViewItem(new string[] { "完成", DateTime.Now.ToString(), "识别结果：" + resultHelper.data }));
                    }));
                    
                    //写入plc：明确告知，图像处理完成，可以进行下一步工序
                    PlcHelper.instance.writePlc(64);
                }

            }
        }

        /// <summary>
        /// 下面的窗口的句柄，抓图成功后显示图片
        /// </summary>
        private HTuple hv_WindowHandle;


        /// <summary>
        /// 选择照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "请选择图片";
                openFileDialog.Filter = "图片文件|*.jpg;*.png;*.jpeg";
                openFileDialog.Multiselect = false;  //是否允许多选
                openFileDialog.InitialDirectory = @"C:\Users\16348\Desktop\课件\实战\0928\ocr";
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    pictureBox1.ImageLocation = filePath;
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation == null)
            {
                this.ShowWarningTip("请选择图片");
                return;
            }else
            {
                halconHelper.ReadImageOcr(pictureBox1.ImageLocation, hv_WindowHandle);
            }
            halconHelper.ReadImageOcr(pictureBox1.ImageLocation,hv_WindowHandle);


        }
        /// <summary>
        /// 选择当前照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            //显示图片
            DisplayHalconImage();
        }
        public HObject halconImage;
        /// <summary>
        /// 显示图片
        /// </summary>
        private void DisplayHalconImage()
        {
            if(halconImage != null)
            {
                halconImage.Dispose();
            }
            if (!hKServerHelper.TryCreateHalconImage(hKServerHelper.frameForSave.Image,out halconImage))
            {
                this.ShowWarningTip("转换图片失败");
            }
            HOperatorSet.GetImageSize(halconImage, out HTuple hv_ImageWidth, out HTuple hv_ImageHeight);
            //让图像自适应
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_ImageWidth, hv_ImageHeight);
            HOperatorSet.DispObj(halconImage, hv_WindowHandle);
                
        }
    }
}
