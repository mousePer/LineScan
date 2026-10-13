using MvCameraControl;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineScanInspection.Help
{
    public class HKServerHelper
    {

        readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
            | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;
        //基本信息设备列表
        private List<IDeviceInfo> deviceInfoList;
        /// <summary>
        /// 相机控制类
        /// </summary>
        public IDevice device;
        /// <summary>
        /// 是否打开设备
        /// </summary>
        public bool Enabled = false;
        /// <summary>
        /// 是否打开采集
        /// </summary>
        public bool Acquisition = false;
        /// <summary>
        /// 初始化SDK
        /// </summary>

        public Action<IDevice,IFrameOut> showImageCallBack
            ;

        public HKServerHelper() {

            SDKSystem.Initialize();
        }
        public AppResultHelper<List<string>> GetDevice()
        {
            List<string> devices = new List<string>();
            //去查询所有相机设备，enumTLayerType相机类型，deviceInfoList 相机基本信息列表
            int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
            if (nRet != MvError.MV_OK)
            {
                //ShowErrorMsg("Enumerate devices fail!", nRet);
                return AppResultHelper<List<string>>.Fail("Enumerate devices fail!");
            }
            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < deviceInfoList.Count; i++)
            {
                IDeviceInfo deviceInfo = deviceInfoList[i];

                devices.Add(deviceInfo.SerialNumber);
            }

            return AppResultHelper<List<string>>.Success(devices);
        }
        /// <summary>
        /// 打开设备
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public AppResultHelper<bool> OpenDevice(string serialNumber)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                return AppResultHelper<bool>.Fail("请选择设备！");
            }

            if (deviceInfoList.Count == 0 )
            {
                return AppResultHelper<bool>.Fail("No device, please select");
            }

            // ch:获取选择的设备信息 | en:Get selected device information
            IDeviceInfo deviceInfo = deviceInfoList.Find(x => x.SerialNumber == serialNumber);
            try
            {
                // ch:打开设备 | en:Open device
                device = DeviceFactory.CreateDevice(deviceInfo);
            }
            catch (Exception ex)
            {
                return AppResultHelper<bool>.Fail($"Create Device fail!:{ex.Message}");
            }

            int result = device.Open();
            if (result != MvError.MV_OK)
            {
                return AppResultHelper<bool>.Fail($"Open Device fail!:{result}");
            }

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            if (device is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                IGigEDevice gigEDevice = device as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                int optionPacketSize;
                result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
                if (result != MvError.MV_OK)
                {
                    return AppResultHelper<bool>.Fail($"Warning: Get Packet Size failed!:{result}");
                }
                else
                {
                    result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
                    if (result != MvError.MV_OK)
                    {
                        return AppResultHelper<bool>.Fail($"\"Warning: Set Packet Size failed!:{result}");
                    }
                }
            }
            Enabled = true;  
            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            device.Parameters.SetEnumValueByString("TriggerMode", "Off");
            // ch:开始采集 | en:Start Grabbing
            //device.StreamGrabber.StartGrabbing();
            //Acquisition = true;
            return AppResultHelper<bool>.Success();
        }


        public Thread receiveThread;
        private IFrameOut frameForSave;
        private static readonly object saveImageLock = new object();

        public AppResultHelper<bool> StartGrabbing()
        {
            if (!Enabled)
            {
                return AppResultHelper<bool>.Fail("请先打开设备！");
            }
            try
            {
                // ch:标志位置位true | en:Set position bit true
                Acquisition = true;

                receiveThread = new Thread(ReceiveThreadProcess);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                return AppResultHelper<bool>.Fail($"Start thread failed!!:{ex.Message}");
            }

            // ch:开始采集 | en:Start Grabbing
            int result = device.StreamGrabber.StartGrabbing();
            if (result != MvError.MV_OK)
            {
                Acquisition = false;
                receiveThread.Join();
                return AppResultHelper<bool>.Fail($"Start Grabbing Fail!:{result}");
            }

            device.StreamGrabber.StartGrabbing();
            Acquisition = true;
            return AppResultHelper<bool>.Success();

        }

        private void ReceiveThreadProcess()
        {
            int nRet;

            Graphics graphics;   // ch:使用GDI在pictureBox上绘制图像 | en:Display frame using a graphics

            while (Acquisition)
            {
                IFrameOut frameOut;

                nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == nRet)
                {


                    lock (saveImageLock)
                    {
                        try
                        {
                            frameForSave = frameOut.Clone() as IFrameOut;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("IFrameOut.Clone failed, " + e.Message);
                            return;
                        }
                    }
                    //显示图片
                    //device.ImageRender.DisplayOneFrame(pictureBox1.Handle, frameOut.Image);
                    showImageCallBack(device, frameForSave);
                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
            }
        }


        public AppResultHelper<bool> StropCamera()
        {
            if (!Enabled)
            {
                return AppResultHelper<bool>.Fail("请先打开相机");
            }
            if (!Acquisition)
            {
                return AppResultHelper<bool>.Fail("请先开始采集！");
            }
            Acquisition = false;
            receiveThread.Join();
            //停止采集
            device.StreamGrabber.StopGrabbing();
            //关闭设备
            if(device != null)
            {
                device.Close();
                device.Dispose();
            }
            return AppResultHelper<bool>.Success();


        }

        /// <summary>
        /// 反初始化SDK
        /// </summary>
        ~HKServerHelper() {
            SDKSystem.Finalize();
        }
    }
}
