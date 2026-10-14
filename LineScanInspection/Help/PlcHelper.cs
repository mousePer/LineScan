using NModbus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.Help
{
    public class PlcHelper
    {
        private string PlcIp { get; set; }
        private int PlcPort { get; set; }
        public string PlcSlaveId { get; set; }

        private PlcHelper()
        {
            PlcIp = ConfigurationManager.AppSettings["PlcIp"].ToString();
            PlcPort = Convert.ToInt32(ConfigurationManager.AppSettings["PlcPort"].ToString());
            PlcSlaveId = ConfigurationManager.AppSettings["PlcSlaveId"].ToString();
        }

        public static PlcHelper instance { get; private set; } = new PlcHelper();
        public TcpClient tcpClient;
        public IModbusMaster modbusMaster;
        public bool isConnect = false;
        public AppResultHelper<bool> Connect()
        {
            try
            {
                //PLC连接
                tcpClient = new TcpClient(PlcIp, PlcPort);
                modbusMaster = new ModbusFactory().CreateMaster(tcpClient);
                isConnect = tcpClient.Connected;
                return isConnect ? AppResultHelper<bool>.Success() : AppResultHelper<bool>.Fail("PLC连接失败");
            }catch(Exception ex)
            {
                return AppResultHelper<bool>.Fail("plc连接错误："+ex.Message);
            }
        }
        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <returns></returns>
        public AppResultHelper<ushort> ReadPlc()
        {
            try
            {
                if (!isConnect)
                {
                    return AppResultHelper<ushort>.Fail("PLC未连接");
                }
                //读取PLC数据   40001
                ushort[] result = modbusMaster.ReadHoldingRegisters(byte.Parse(PlcSlaveId), 0, 1);
                if( result==null || result.Length == 0)
                {
                    return AppResultHelper<ushort>.Fail("PLC数据读取失败");
                }
                return AppResultHelper<ushort>.Success(result[0]);
            }
            catch (Exception ex)
            {
                return AppResultHelper<ushort>.Fail("错误信息：" + ex.Message);
            }
        }

        public AppResultHelper<ushort> writePlc(ushort value)
        {
            try
            {
                if (!isConnect)
                {
                    return AppResultHelper<ushort>.Fail("PLC未连接");
                }
                //读取PLC数据   40001
                modbusMaster.WriteSingleRegister(byte.Parse(PlcSlaveId), 0, value);
                return AppResultHelper<ushort>.Success();
            }
            catch (Exception ex)
            {
                return AppResultHelper<ushort>.Fail("错误信息：" + ex.Message);
            }
        }

    }
}
