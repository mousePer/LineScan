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
        public CameraForm()
        {
            InitializeComponent();
            if (hKServerHelper == null)
            {
                hKServerHelper = new HKServerHelper();
                hKServerHelper.showImageCallBack = showImageCallBack;
            }
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
    }
}
