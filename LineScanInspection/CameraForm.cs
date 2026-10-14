using HalconDotNet;
using LineScanInspection.Help;
using MvCameraControl;
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
            HOperatorSet.OpenWindow(0, 0, pictureBox2.Width-1, pictureBox2.Height-1,pictureBox2.Handle, "visible","",out hv_WindowHandle);
            HDevWindowStack.Push(hv_WindowHandle);
            HOperatorSet.SetWindowAttr("background_color", "black");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="out"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void showImageCallBack(IDevice device, IFrameOut iFrameOut)
        {
            this.Invoke(new Action(() =>
            {
                device.ImageRender.DisplayOneFrame(pictureBox1.Handle, iFrameOut.Image);
            }));
        }

        public HKServerHelper hKServerHelper;

        //打开相机
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
            if (!hKServerHelper.OpenDevice(result.data[0]).isSuccessful)
            {
                this.ShowWarningTip("打开相机失败");
                return;
            }
            if (!hKServerHelper.StartGrabbing().isSuccessful)
            {
                this.ShowWarningTip("开始抓图失败");
                return;
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
        private void DisplayHalconImage()
        {
            
            if (!hKServerHelper.TryCreateHalconImage(hKServerHelper.frameForSave.Image,out halconImage))
            {
                this.ShowWarningTip("转换图片失败");
            }
            HOperatorSet.GetImageSize(halconImage, out HTuple hv_ImageWidth, out HTuple hv_ImageHeight);
            //让图像自适应
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_ImageWidth, hv_ImageHeight);
            HOperatorSet.DispObj(halconImage, hv_WindowHandle);
            if(!int.TryParse(uiTextBox1.Text,out int upValue))
            {
                this.ShowWarningTip("请填写正确上限值");
            }
            if(!int.TryParse(uiTextBox2.Text,out int downValue))
            {
                this.ShowWarningTip("请填写正确下限值");
            }
            halconHelper.ReadImageOcr(halconImage, hv_WindowHandle, upValue, downValue);
        }
    }
}
