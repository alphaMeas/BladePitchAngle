using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMCLaserSensor
{
    public class LMCSensor
    {
        public LMCSensor()
        {
            System.Diagnostics.Debug.WriteLine("LMCSensor Constructor.");
        }

        private System.IO.Ports.SerialPort sensorPort;

        public void CreatePort(string portName)
        {
            // 默认：115200，无校验， 8数据位，1停止位
            if (sensorPort == null)
            {
                sensorPort = new System.IO.Ports.SerialPort(portName,
                    115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            }
            // 重新创建新的串口
            else if (portName != sensorPort.PortName)
            {
                sensorPort.Close();
                sensorPort.Dispose();

                sensorPort = new System.IO.Ports.SerialPort(portName,
                   115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            }

            if (sensorPort.IsOpen == false)
            {
                sensorPort.Open();

                sensorPort.ReadTimeout = 1000;

                sensorPort.WriteTimeout = 10;

            }
        }

 
        /// <summary>
        /// 停止测量
        /// </summary>
        public void StopMeas()
        {
            Execute(CMD_STOP);

            reader.OnDataRecieved -= Reader_OnDataRecieved;

            reader.StopRead(startTime);
        }

        //ILMCDTReader reader = new LMCDTHexReader();

        ILMCDTReader reader = new LMCDTBinReader();

        private DateTime startTime;
        public void ContinueMeas()
        {
            Execute(CMD_DT);

            startTime = DateTime.Now;

            // 连续读取数据
            reader.StartRead(sensorPort);

            reader.OnDataRecieved += Reader_OnDataRecieved; ;
        }

        private void Reader_OnDataRecieved(List<int> intData)
        {
            if (OnDataRecieved != null)
            {
                OnDataRecieved(intData);
            }
        }

        public event DataRecieved OnDataRecieved;


        /// <summary>
        /// 温度采集
        /// </summary>
        /// <returns></returns>
        public string GetTempreture()
        {
            //sensorPort.DataReceived += sensorPort_DataReceived;
            Execute(CMD_TP);

            // 读取温度
            string temp = sensorPort.ReadLine();

            return temp;//"TP";
        }


        /// <summary>
        /// 单帧测量
        /// </summary>
        /// <returns></returns>
        public string GetSigleDis()
        {
            Execute(CMD_DM);

            string temp = sensorPort.ReadLine();

            return temp;
        }



        private void Execute(byte[] cmd)
        {
            sensorPort.Write(cmd, 0, cmd.Length);
        }



        // set MF 测量频率


        // set SA 平均
        public string SetSA1()
        {
            Execute(CMD_SA1);

            string temp = sensorPort.ReadLine();

            return temp;
        }


        /// <summary>
        /// set output format.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string SetOutPut(int param)
        {
            if (param == 2)
            {
                Execute(CMD_SD2);
            }
            else if (param == 16)
            {
                Execute(CMD_SD1);
            }
            else if (param == 10)
            {
                Execute(CMD_SD0);
            }
            else {
                return " param error." + param;
            }
            string temp = sensorPort.ReadLine();

            return temp;
        }


        /// <summary>
        /// stop
        /// </summary>
        public void Stop()
        {
            Execute(CMD_STOP);
        }

        // 单次测量 44 20 30 30 30 33 2E 34 30 32 0D 0A 
        private byte[] CMD_DM = new byte[] { 0x44, 0x4D, 0x0D };

        // 连续测量 无返回报文，直接返回数据
        private byte[] CMD_DT = new byte[] { 0x44, 0x54, 0x0D };

        // 停止测量 停止连续测量，无返回报文，连续发送返回 3F 0D 0A
        private byte[] CMD_STOP = new byte[] { 0x1B, 0x0D };

        // 平均次数 2 53 41 20 32 0D 0A 
        private byte[] CMD_SA1 = new byte[] { 0x53, 0x41, 0x31, 0x0D };

        // 温度显示 TP 027.6   54 50 20 30 32 37 2E 36 0D 0A 
        private byte[] CMD_TP = new byte[] {  0x54, 0x50,0x0D };

        // 设置输出格式 53 44 20 30 20 30 0D 0A 十进制 ascii D 0000.000 44 20 30 30 30 33 2E 32 37 34 0D 0A 
        private byte[] CMD_SD0 = new byte[] { 0x53, 0x44, 0x30, 0x20, 0x30, 0x0d };

        // 设置输出格式 53 44 20 31 20 30 0D 0A十六进制 ascii  48 30 30 30 32 37 34 0D 0A H00027B
        private byte[] CMD_SD1 = new byte[] { 0x53, 0x44, 0x31, 0x20, 0x30, 0x0d };

        // 设置输出格式 53 44 20 32 20 30 0D 0A  二进制 no target C2 45 52 数据：1090258 正确输出： 80 1A 48 / 80 18 7B   
        private byte[] CMD_SD2 = new byte[] { 0x53, 0x44, 0x32, 0x20, 0x30, 0x0d };

    }
}
