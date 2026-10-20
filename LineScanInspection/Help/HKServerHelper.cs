using HalconDotNet;          // Halcon 的 .NET 接口命名空间，用来把图像转成 Halcon 能处理的 HObject 对象
using MvCameraControl;       // 海康相机 SDK 的命名空间，提供 IDevice、IFrameOut、DeviceEnumerator 等相机控制类型
using Sunny.UI.Win32;        // SunnyUI 界面库的 Win32 辅助命名空间（本项目用来做 UI 美化，这里只是引用）
using System;                // 基础类库：Object、IntPtr、Exception 等
using System.Collections.Generic;  // 泛型集合：List<T> 等
using System.Drawing;        // GDI 绘图类型：Graphics（在 PictureBox 上画图）
using System.Linq;           // LINQ 扩展：List.Find(x => ...) 等查询语法
using System.Text;           // 文本编码/字符串处理
using System.Threading;      // 多线程：Thread 线程类
using System.Threading.Tasks;// 任务并行库（本类实际没用到，保留引用）
using System.Windows.Forms;  // WinForms 控件：PictureBox、MessageBox 等

namespace LineScanInspection.Help   // 命名空间：把相机的辅助操作类都放在这个包里
{
    /// <summary>
    /// 海康相机操作帮助类：负责枚举设备、打开相机、开始/停止采集、
    /// 后台取帧、把海康图像转换成 Halcon 图像等所有相机相关操作。
    /// </summary>
    public class HKServerHelper
    {
        // 枚举哪些类型的相机设备（按位或组合，表示同时支持这些连接类型）
        // MvGigEDevice=千兆网相机(GigE)、MvUsbDevice=USB相机、MvGenTL* = 通过 GenTL 通用接口访问的各类相机
        readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
            | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;
        //基本信息设备列表
        // 保存枚举到的相机设备基本信息列表（每条含序列号、型号、IP 等信息），用于后续打开指定相机
        private List<IDeviceInfo> deviceInfoList;
        /// <summary>
        /// 相机控制类
        /// </summary>
        public IDevice device;   // 当前打开的那台相机的控制对象；对相机的一切操作（开、关、采参数）都通过它

        /// <summary>
        /// 是否打开设备
        /// </summary>
        public bool Enabled = false;      // 标记：相机设备是否已成功打开（true=已打开）

        /// <summary>
        /// 是否打开采集
        /// </summary>
        public bool Acquisition = false;  // 标记：是否正在连续采集（true=正在采图），后台取帧线程靠它决定是否继续跑

        /// <summary>
        /// 图像显示回调委托：每取到一帧就回调一次，让外部（CameraForm）把图像显示到界面上
        /// 委托签名：接收(相机设备对象, 一帧图像对象)，这里只声明变量，具体显示逻辑在外部赋值
        /// </summary>
        public Action<IDevice, IFrameOut> showImageCallBack;

        /// <summary>
        /// 构造函数：创建海康相机帮助类时，先初始化海康 SDK
        /// （SDK 初始化后，后面的枚举/打开/采集接口才能正常使用）
        /// </summary>
        public HKServerHelper()
        {
            SDKSystem.Initialize();   // 调用海康 SDK 的初始化接口，做全局环境准备
        }

        /// <summary>
        /// 枚举（搜索）电脑上所有海康相机，返回它们的序列号列表
        /// 返回值：AppResultHelper 是自定义的结果包装类，isSuccessful 表示是否成功，data 存真正的数据
        /// </summary>
        public AppResultHelper<List<string>> GetDevice()
        {
            List<string> devices = new List<string>();   // 建一个列表，准备装所有相机的序列号

            // 调用海康 SDK 的枚举接口，按 enumTLayerType 指定的类型去查找相机
            // 查到的相机基本信息会写进 out 参数 deviceInfoList；返回值 nRet 是错误码
            int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
            if (nRet != MvError.MV_OK)   // MV_OK 表示成功；如果不是，说明枚举失败
            {
                //ShowErrorMsg("Enumerate devices fail!", nRet);   // 原注释掉的弹窗写法，被注释没启用
                return AppResultHelper<List<string>>.Fail("Enumerate devices fail!");  // 返回一个"失败"结果，带错误信息
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            // 遍历刚才枚举到的所有相机，把每台的序列号(SerialNumber)加入 devices 列表
            for (int i = 0; i < deviceInfoList.Count; i++)
            {
                IDeviceInfo deviceInfo = deviceInfoList[i];        // 取第 i 台相机的基本信息
                devices.Add(deviceInfo.SerialNumber);              // 把它的序列号存进列表
            }
            return AppResultHelper<List<string>>.Success(devices); // 返回"成功"结果，并带上所有序列号
        }

        /// <summary>
        /// 打开指定序列号的相机，并做必要的网络参数优化
        /// 参数 serialNumber：要打开的相机的序列号
        /// 返回：成功/失败的包装结果
        /// </summary>
        public AppResultHelper<bool> OpenDevice(string serialNumber)
        {
            // 第一步：参数校验——序列号为空就直接失败，不让后续代码拿空值去折腾
            if (string.IsNullOrEmpty(serialNumber))
            {
                return AppResultHelper<bool>.Fail("请选择设备！");
            }
            // 第二步：如果之前枚举时一台相机都没找到，也没法打开
            if (deviceInfoList.Count == 0)
            {
                return AppResultHelper<bool>.Fail("No device, please select");
            }

            // ch:获取选择的设备信息 | en:Get selected device information
            // 在之前枚举到的列表里，用序列号精确找到那台相机的信息对象
            IDeviceInfo deviceInfo = deviceInfoList.Find(x => x.SerialNumber == serialNumber);
            try
            {
                // ch:打开设备 | en:Open device
                // 用设备信息创建出一个真正的相机控制对象（device）
                // 这步可能抛异常（例如相机被占用），所以用 try 包住
                device = DeviceFactory.CreateDevice(deviceInfo);
            }
            catch (Exception ex)   // 创建失败时捕获异常
            {
                return AppResultHelper<bool>.Fail($"Create Device fail!:{ex.Message}");  // 返回失败，把异常信息带出来
            }

            int result = device.Open();   // 真正执行"打开设备"的 SDK 接口，result 是错误码
            if (result != MvError.MV_OK)  // 打开失败
            {
                return AppResultHelper<bool>.Fail($"Open Device fail!:{result}");
            }

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            // 判断这台相机是不是千兆网(GigE)相机；因为只有 GigE 相机需要调网络包大小优化
            if (device is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                // 把通用接口强转成 GigE 专属接口，才能调下面的网络优化方法
                IGigEDevice gigEDevice = device as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                // 让 SDK 自动探测一个"最优网络包大小"（能减少丢包、提高传输效率）
                int optionPacketSize;
                result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
                if (result != MvError.MV_OK)   // 探测失败
                {
                    return AppResultHelper<bool>.Fail($"Warning: Get Packet Size failed!:{result}");
                }
                else   // 探测成功
                {
                    // 把这个最优包大小写进相机参数 GevSCPSPacketSize，让相机按这个大小发包
                    result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
                    if (result != MvError.MV_OK)   // 设置失败
                    {
                        return AppResultHelper<bool>.Fail($"\"Warning: Set Packet Size failed!:{result}");
                    }
                }
            }
            Enabled = true;   // 标记：设备已成功打开，之后 StartGrabbing 才允许执行

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            // 把采集模式设为"连续采集"(Continuous)，即相机一直出帧，不需要一张一张手动触发
            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            // 把触发模式设为 Off：不需要外部/内部硬触发，相机自由连续出帧
            device.Parameters.SetEnumValueByString("TriggerMode", "Off");
            // ch:开始采集 | en:Start Grabbing
            //device.StreamGrabber.StartGrabbing();   // 这两行被注释掉了：OpenDevice 里不真正开始采集，
            //Acquisition = true;                       // 而是把"开始采集"留给 StartGrabbing() 方法单独做

            return AppResultHelper<bool>.Success();    // 打开成功，返回成功结果（无数据）
        }

        public Thread receiveThread;      // 后台取帧线程：单独一个线程不停地从相机取图，避免阻塞主界面
        public IFrameOut frameForSave;    // 保存"最新一帧"图像对象，供外部（如识别逻辑）随时读取最近一帧

        // 锁对象：因为"后台取帧线程"写 frameForSave、"其他线程(如识别/显示)"读 frameForSave，
        // 可能同时访问导致冲突，所以用这把锁保证同一时刻只有一个线程碰它（线程安全）
        private static readonly object saveImageLock = new object();

        /// <summary>
        /// 开始连续采集：启动后台取帧线程 + 让相机 StreamGrabber 开始出帧
        /// 返回：成功/失败包装结果
        /// </summary>
        public AppResultHelper<bool> StartGrabbing()
        {
            if (!Enabled)   // 设备还没打开就不能开始采集
            {
                return AppResultHelper<bool>.Fail("请先打开设备！");
            }
            try
            {
                // ch:标志位置位true | en:Set position bit true
                Acquisition = true;   // 把"正在采集"标记置为 true，后台取帧线程的 while 循环才会继续跑

                // 新建一个后台线程，执行 ReceiveThreadProcess（循环取帧）
                receiveThread = new Thread(ReceiveThreadProcess);
                receiveThread.Start();   // 启动这个后台线程
            }
            catch (Exception ex)   // 启动线程失败（极少见，比如线程资源不够）
            {
                return AppResultHelper<bool>.Fail($"Start thread failed!!:{ex.Message}");
            }

            // ch:开始采集 | en:Start Grabbing
            // 真正让相机的数据流开始工作（出帧），result 是错误码
            int result = device.StreamGrabber.StartGrabbing();
            if (result != MvError.MV_OK)   // 开始采集失败
            {
                Acquisition = false;            // 先把采集标记关掉，让后台线程自己退出
                receiveThread.Join();           // 等待后台取帧线程结束（避免线程还没退就返回导致混乱）
                return AppResultHelper<bool>.Fail($"Start Grabbing Fail!:{result}");
            }
            Acquisition = true;   // 再确认一次采集标记为 true
            return AppResultHelper<bool>.Success(); // 返回成功
        }

        /// <summary>
        /// 后台取帧线程的方法体：只要 Acquisition 为 true，就不断地从相机取一帧、
        /// 保存到 frameForSave（加锁保护）、并回调 showImageCallBack 让界面显示
        /// </summary>
        private void ReceiveThreadProcess()
        {
            int nRet;                 // 存放每次取帧的返回错误码
            Graphics graphics;   // 声明一个 GDI 绘图对象（这里只声明，实际绘制由海康的 ImageRender 完成）

            while (Acquisition)   // 只要还在采集（Acquisition=true）就一直循环取帧
            {
                IFrameOut frameOut;   // 声明一个变量，用来接收取到的当前帧

                // 从相机数据流取一帧，超时时间 1000 毫秒；取到就把帧放进 out 参数 frameOut，返回码进 nRet
                nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == nRet)   // 取帧成功
                {
                    lock (saveImageLock)   // 加锁：保护下面的"写 frameForSave"不被其他线程同时读导致冲突
                    {
                        try
                        {
                            // 把当前帧 Clone（复制）一份存到 frameForSave，供其他线程读取最新帧
                            // 之所以 Clone，是因为原始帧 frameOut 用完后要还给 SDK，不能直接让别人拿着用
                            frameForSave = frameOut.Clone() as IFrameOut;
                        }
                        catch (Exception e)   // Clone 失败
                        {
                            MessageBox.Show("IFrameOut.Clone failed, " + e.Message);  // 弹窗提示克隆失败
                            return;            // 直接退出取帧线程
                        }
                    }
                    //显示图片
                    //device.ImageRender.DisplayOneFrame(pictureBox1.Handle, frameOut.Image);  // 原注释：直接显示图像
                    // 改为通过回调委托把帧交给外部(CameraForm)去显示，职责解耦
                    showImageCallBack(device, frameForSave);
                    device.StreamGrabber.FreeImageBuffer(frameOut);   // 把原始帧还给 SDK（释放缓冲区），否则内存会越积越多
                }
            }
        }

        /// <summary>
        /// 停止采集并关闭设备（方法名拼写有点小瑕疵：Strop 应为 Stop，但不影响使用）
        /// 返回：成功/失败包装结果
        /// </summary>
        public AppResultHelper<bool> StropCamera()
        {
            if (!Enabled)   // 没打开过设备，就不用停
            {
                return AppResultHelper<bool>.Fail("请先打开相机");
            }
            if (!Acquisition)   // 当前根本没在采集，就不用停
            {
                return AppResultHelper<bool>.Fail("请先开始采集！");
            }
            Acquisition = false;       // 把采集标记置 false，后台取帧线程的 while 循环会自己退出
            receiveThread.Join();      // 阻塞等待后台取帧线程真正跑完再继续（保证线程干净退出）
            //停止采集
            device.StreamGrabber.StopGrabbing();   // 让相机数据流停止出帧
            //关闭设备
            if (device != null)   // 设备对象存在才关
            {
                device.Close();      // 关闭设备
                device.Dispose();    // 释放设备占用的资源
            }
            return AppResultHelper<bool>.Success();   // 返回成功
        }

        /// <summary>
        /// 尝试把海康相机的通用图像对象(IImage)转换成 Halcon 图像对象(HObject)
        /// 转换依据是图像里的"像素格式"(PixelType)，不同格式用不同的 Halcon 算子来建图
        /// 参数 image      ：海康取到的原始图像对象（含像素数据指针、宽高、像素格式）
        /// 参数 halconImage：输出，转换成功后得到的 Halcon 图像对象
        /// 返回：true=转换成功；false=图像无效或遇到不支持的像素格式
        /// </summary>
        public bool TryCreateHalconImage(IImage image, out HObject halconImage)
        {
            // 初始化输出的 Halcon 图像为 null（避免没转换时外面拿到一个脏对象）
            halconImage = null;

            // 校验：图像对象为空 或 像素数据指针为空 → 无法转换，直接返回失败
            // PixelDataPtr 是指向像素内存的指针，没有它 Halcon 拿不到任何像素数据
            if (image == null || image.PixelDataPtr == IntPtr.Zero)
            {
                return false;
            }

            // 获取图像的宽和高（海康返回的类型可能是 uint，这里强制转成 int 给 Halcon 用）
            int width = (int)image.Width;
            int height = (int)image.Height;

            // 根据海康图像的"像素格式"决定用哪种方式创建 Halcon 图像
            switch (image.PixelType)
            {
                // ↓ 这一组是"8 位"格式：Mono8=8位黑白图；BayerGR8/RG8/GB8/BG8/RBGG8=8位彩色原图(拜耳阵列)
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
                    // GenImage1：根据一块连续内存 + 宽高，直接创建一张 Halcon 图
                    HOperatorSet.GenImage1(
                        out halconImage,    // 输出：创建好的 Halcon 图像
                        "byte",             // 图像类型（每个像素占 8 位）
                        width,              // 图像宽度
                        height,             // 图像高度
                        image.PixelDataPtr  // 像素数据的内存指针（Halcon 直接从这块内存读像素）
                    );
                    //HOperatorSet.GenImage1(out halconImage, "byte", width, height, image.PixelDataPtr);  // 原注释：同一行代码
                    return true;   // 转换成功
                // ↓ 这一组是"16 位"格式：Mono10/12/14/16=16位黑白图；Bayer*10/12/16=16位彩色原图(拜耳阵列)
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
                    // 16 位图：像素类型用 "uint2"（16 位无符号整数），其他参数和 byte 一样
                    HOperatorSet.GenImage1(out halconImage, "uint2", width, height, image.PixelDataPtr);
                    return true;   // 转换成功
                // ↓ RGB8 打包格式：彩色图像，三个通道 R、G、B 连续排在一起（最常见的一种彩色格式）
                case MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                    // GenImageInterleaved = 专门用来创建【RGB/BGR 连续打包】的彩色图像
                    // RGB8 打包格式（最常见）
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
                    return true;   // 转换成功
                //BGR8 打包格式（OpenCV 常用）
                case MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                    // 和上面 RGB 基本一样，唯一区别是颜色排列顺序是 B → G → R
                    HOperatorSet.GenImageInterleaved(
                        out halconImage,   // 输出：创建好的 Halcon 彩色图像
                        image.PixelDataPtr, // 像素数据内存指针
                        "bgr",               // ✅ 颜色排列顺序：B → G → R
                        width,              // 图像宽度
                        height,             // 图像高度
                        0,                  // 行对齐：0
                        "byte",             // 每通道8位
                        width,              // 行宽
                        height,             // 图像高
                        0,                  // 起始偏移0
                        0,                  // 无填充
                        -1,                 // 自动计算通道数
                        0                   // 保留参数
                    );
                    return true;   // 转换成功
                default:   // 走到这里说明是上面都没列出的像素格式（不支持的格式）
                    return false;   // 转换失败
            }
        }

        /// <summary>
        /// 析构函数：当这个对象被垃圾回收时调用，用来反初始化海康 SDK
        /// 注意：析构函数在 C# 里不保证及时执行，正式项目建议改用显式的 Close/Dispose 方式反初始化
        /// </summary>
        ~HKServerHelper()
        {
            SDKSystem.Finalize();   // 调用海康 SDK 的反初始化接口，释放 SDK 全局资源
        }
    }
}
