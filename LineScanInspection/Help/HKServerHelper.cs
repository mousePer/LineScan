using HalconDotNet;
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

        public Action<IDevice, IFrameOut> showImageCallBack
            ;

        public HKServerHelper()
        {

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

            if (deviceInfoList.Count == 0)
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
        public IFrameOut frameForSave;
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
            if (device != null)
            {
                device.Close();
                device.Dispose();
            }
            return AppResultHelper<bool>.Success();


        }
        /// 尝试将通用图像对象转换为 Halcon 图像对象
        /// </summary>
        /// <param name="image">输入的原始图像对象（包含像素数据、宽高、像素格式等信息）</param>
        /// <param name="halconImage">输出：转换成功后的 Halcon 图像对象</param>
        /// <returns>转换成功返回 true，失败/不支持的格式返回 false</returns>
        public bool TryCreateHalconImage(IImage image, out HObject halconImage)
        {
            // 初始化输出的 Halcon 图像为 null
            halconImage = null;

            // 校验：图像对象为空 或 像素数据指针为空 → 无法转换，直接返回失败
            if (image == null || image.PixelDataPtr == IntPtr.Zero)
            {
                return false;
            }

            // 获取图像的宽和高（强制转换为 int 类型）
            int width = (int)image.Width;
            int height = (int)image.Height;

            switch (image.PixelType)
            {
                case MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MvGvspPixelType.PixelType_Gvsp_BayerRBGG8:
                    /*
                         2. 每个参数到底什么意思？
                        ① out HObject halconImage
                        输出参数
                        函数执行成功后，会自动创建一个 Halcon 图像，并赋值给这个变量
                        你后面才能用它显示、处理、保存
                        ② "byte" → 最重要！
                        表示每个像素的数据类型：
                        byte = 8 位无符号整数（0~255）
                        适用于：
                        Mono8 黑白相机
                        BayerGR8 / RG8 / GB8 / BG8 彩色相机
                        8 位灰度图
                        如果是 16 位图像，就写 "uint2"。
                     */
                    HOperatorSet.GenImage1(
                        out halconImage,    // 输出：创建好的 Halcon 图像
                        "byte",             // 图像类型（每个像素占 8 位）
                        width,              // 图像宽度
                        height,             // 图像高度
                        image.PixelDataPtr  // 像素数据的内存指针
                    );
                    //HOperatorSet.GenImage1(out halconImage, "byte", width, height, image.PixelDataPtr);
                    return true;

                case MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MvGvspPixelType.PixelType_Gvsp_Mono14:
                case MvGvspPixelType.PixelType_Gvsp_Mono16:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR16:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG16:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB16:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG16:
                    HOperatorSet.GenImage1(out halconImage, "uint2", width, height, image.PixelDataPtr);
                    return true;
                case MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                    //GenImageInterleaved = 专门用来创建【RGB/BGR 连续打包】的彩色图像
                    //RGB8 打包格式（最常见）
                    HOperatorSet.GenImageInterleaved(
                        out halconImage,     // 输出：创建好的Halcon彩色图像
                        image.PixelDataPtr,  // 像素数据内存指针
                        "rgb",               // ✅ 颜色排列顺序：R → G → B
                        width,               // 图像宽度
                        height,              // 图像高度
                        0,                   // 行对齐：0 = 每行开头无额外填充
                        "byte",              // 每个通道是8位（0~255）
                        width,               // 输入图像每行宽度（和宽度一致）
                        height,              // 输入图像高度
                        0,                   // 起始偏移：从第0个像素开始
                        0,                   // 无额外填充
                        -1,                  // 自动计算通道数
                        0                    // 保留参数，固定0
                    );
                    return true;

                //BGR8 打包格式（OpenCV 常用）
                case MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                    HOperatorSet.GenImageInterleaved(
                        out halconImage,
                        image.PixelDataPtr,
                        "bgr",               // ✅ 颜色排列顺序：B → G → R
                        width,
                        height,
                        0,
                        "byte",
                        width,
                        height,
                        0,
                        0,
                        -1,
                        0
                    );
                    return true;

                default:
                    return false;
            }
        }
    

        /// <summary>
        /// 反初始化SDK
        /// </summary>
        ~HKServerHelper()
        {
            SDKSystem.Finalize();
        }
    }
}
