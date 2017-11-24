using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladePitchAngle
{
    public delegate void CMDataChanged (CompassModuleData newData);

    public class CompassModule
    {
        System.IO.Ports.SerialPort CompassModulePort;

        public CompassModule()
        {
            PortName = "COM1";

            PcBaudRate = DefaultBaudRate;

            CMBaudRate = DefaultBaudRate;

            SampleRate = "50Hz";
        }

        /// <summary>
        /// 串口名称
        /// </summary>
        public string PortName
        {
            get;
            set;
        }

        /// <summary>
        /// 采样频率
        /// </summary>
        public string SampleRate
        {
            get;
            set;
        }


        /// <summary>
        /// PC BaudRate
        /// </summary>
        public string PcBaudRate
        {
            get;
            set;
        }


        /// <summary>
        /// CompassModule BaudRate
        /// </summary>
        public string CMBaudRate
        {
            get;
            set;
        }


        private const string DefaultBaudRate = "9600";





        /// <summary>
        /// 按照预定的波特率设置 波特率
        /// </summary>
        /// <param name="currentRate"></param>
        /// <param name="newRate"></param>
        /// <returns></returns>
        public bool TrySetBaudRate(string currentRate, string newRate)
        {
            // 按照当前波特率打开；
            CreateOpenPort(currentRate);

            // 发送设置波特率命令；
            SendBaudRateCommand(newRate);

            // 等待1秒
            System.Threading.Thread.Sleep(1000);

            // 尝试新的波特率
            if (TryBaudRate(newRate) == newRate)
            {

                // 串口的波特率更新
                CompassModulePort.BaudRate = int.Parse(newRate);

                // 罗盘的波特率更新
                this.CMBaudRate = newRate;
                this.PcBaudRate = newRate;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 发送 设置波特率 命令
        /// </summary>
        private void SendBaudRateCommand(string _rate)
        {
            if (CompassModulePort.IsOpen == false)
            {
                CompassModulePort.Open();
            }
            // 配置设备的Baudrate
            if (_rate == "9600")
            {
                // 发送读取单帧数据命令
                CompassModulePort.Write(CMD_SETBAUDRATE_9600, 0, CMD_SETBAUDRATE_9600.Length);
            }
            else if (_rate == "19200")
            {
                // 发送读取单帧数据命令
                CompassModulePort.Write(CMD_SETBAUDRATE_19200, 0, CMD_SETBAUDRATE_19200.Length);
            }
            else if (_rate == "38400")
            {
                // 发送读取单帧数据命令
                CompassModulePort.Write(CMD_SETBAUDRATE_38400, 0, CMD_SETBAUDRATE_38400.Length);
            }
            else if (_rate == "57600")
            {
                // 发送读取单帧数据命令
                CompassModulePort.Write(CMD_SETBAUDRATE_57600, 0, CMD_SETBAUDRATE_57600.Length);
            }
            else if (_rate == "115200")
            {
                // 发送读取单帧数据命令
                CompassModulePort.Write(CMD_SETBAUDRATE_115200, 0, CMD_SETBAUDRATE_115200.Length);
            }

            //bool bOkay = false;
            //// 寻找帧头
            //if (ReadFrameHead())
            //{
            //    // 读取帧数据
            //    byte[] frame = TryReadFrame(0x05);

            //    for (int inx = 0; inx < 5; inx++)
            //    {
            //        if (frame[inx] != CMDRES_CMD_SETBAUDRATE[inx])
            //        {
            //            //Error.
            //        }
            //    }

            //    bOkay = true;
            //}
            //else
            //{
            //    bOkay = false;                
            //}

            //return bOkay;
        }



        /// <summary>
        /// 搜索波特率
        /// </summary>
        /// <returns></returns>
        public string SearchBaudRate()
        {
            string data = null;

            foreach (string br in Defines.BaudRates)
            {
                data = TryBaudRate(br);
                if (data != null)
                {
                    return br;
                }
            }

            throw new NotSupportedException("Unknow Baudrate.");
        }
        

        /// <summary>
        /// 尝试以波特率通讯
        /// </summary>
        /// <param name="_baudRate"></param>
        /// <returns></returns>
        private string TryBaudRate(string _baudRate)
        {
            try
            {
                CreateOpenPort(_baudRate);

                CompassModuleData data = ReadAllData();

                if (data != null)
                {
                    return _baudRate;
                }

                return null;
            }
            catch (TimeoutException)
            {
                return null;
            }
            finally
            {
                CompassModulePort.Close();
                CompassModulePort.Dispose();
            }
        }


        /// <summary>
        /// 按照默认波特率打开串口
        /// </summary>
        private void CreateOpenPort(string _baudRate)
        {
            int rate = int.Parse(_baudRate);

            // 默认：9600，无校验， 8数据位，1停止位
            if (CompassModulePort == null)
            {
                CompassModulePort = new System.IO.Ports.SerialPort(PortName,
                    rate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            }
            // 重新创建新的串口
            else if (PortName != CompassModulePort.PortName)
            {
                CompassModulePort.Close();
                CompassModulePort.Dispose();

                CompassModulePort = new System.IO.Ports.SerialPort(PortName,
                   rate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            }
            // 重新设置波特率
            else if (CompassModulePort.BaudRate != rate)
            {
                // 需要重新打开。
                CompassModulePort.Close();

                CompassModulePort.BaudRate = rate;
            }

            if (CompassModulePort.IsOpen == false)
            {
                CompassModulePort.ReadTimeout = 1000;

                CompassModulePort.Open();
            }
        }



        /// <summary>
        /// 读取单帧数据
        /// </summary>
        /// <returns></returns>
        public CompassModuleData ReadSigleFrameData()
        {
            CompassModuleData data = null;

            if (TryBaudRate(PcBaudRate) != PcBaudRate)
            {
                throw new NotSupportedException("访问超时，可能使用了错误的波特率。");
            }

            try
            {
                // 按照默认波特率打开串口
                CreateOpenPort(PcBaudRate);

                // 设置波特率
                //if (PcBaudRate != CMBaudRate)
                //{
                //    TrySetBaudRate(CMBaudRate, PcBaudRate);
                //}

                // 重新打开串口（工作在新的波特率
                if (CompassModulePort.IsOpen == false)
                {
                    CompassModulePort.Open();
                }

                data = ReadAllData();
            }
            finally
            {
                // 设置默认波特率
                //if (PcBaudRate != DefaultBaudRate)
                //{
                //    TrySetBaudRate(CMBaudRate, DefaultBaudRate);
                //}

                CompassModulePort.Close();
            }

            return data;
        }

        private CompassModuleData ReadAllData()
        {
            CompassModuleData data = null;

            // 发送读取单帧数据命令
            CompassModulePort.Write(CMD_READALL, 0, CMD_READALL.Length);

            // 寻找帧头
            if (ReadFrameHead())
            {
                // 读取帧数据
                byte[] frame = ReadFrame(0x0d);

                // 转换数据
                data = ConvertToCMData(frame);
            }
            return data;
        }


        /// <summary>
        /// 寻找帧头标志
        /// modified by steel @ 2016-5-16 增加超时退出
        /// </summary>
        /// <returns></returns>
        private bool ReadFrameHead()
        {
            int charCount = 0;
            //try
            //{
                byte temp = (byte)CompassModulePort.ReadByte();
                // 数据帧开始？ 
                while (temp != 0x77)
                {
                    // 错误数据记录起来
                    //errByte.Add(temp);

                    temp = (byte)CompassModulePort.ReadByte();

                    charCount += 1;

                    // 超时退出？
                    if (charCount > 0x20)
                    {
                        throw new NotSupportedException("May be error BaudRate.");
                    }
                }

                return true;
            //}
            //catch
            //{
            //    return false;
            //}

        }


        /// <summary>
        /// 转换一帧数据
        /// </summary>
        /// <returns></returns>
        private CompassModuleData ConvertToCMData(byte[] frame)
        {
            CompassModuleData data = new CompassModuleData();

            data.PitchAngle = ConvertWitLinkBCD(frame, 3);

            data.RollAngle = ConvertWitLinkBCD(frame, 6);

            data.HeadingAngle = ConvertWitLinkBCD(frame, 9);

            return data;
        }
        




        /// <summary>
        /// 数据改变事件
        /// </summary>
        public event CMDataChanged OnDataChanged;

        
        /// <summary>
        /// 启动采集 5Hz
        /// </summary>
        public void StartAcquisition()
        {
            if (TryBaudRate(PcBaudRate) != PcBaudRate)
            {
                throw new NotSupportedException("错误的波特率。");
            }

            if (IsAcquisiting != true)
            {
                IsAcquisiting = true;
               
                // 启动采集线程
                AcquisitionTask = new Task(Acquisition50HzAction);

                AcquisitionTask.Start();

            }
        }

        /// <summary>
        /// 50Hz 采集过程
        /// </summary>
        private void Acquisition50HzAction()
        {
            // 发送50Hz 命令
            try
            {
                // 按照默认波特率打开串口
                CreateOpenPort(PcBaudRate);

                // 发送连续采样命令
                SetSampleRate();
                
                while (IsAcquisiting)
                {
                    if (ReadFrameHead())
                    {
                        // 读取一帧数据
                        // 读取帧数据
                        byte[] frame = ReadFrame(0x0d);

                        if (frame == null) continue;
                        
                        // 转换数据
                        CompassModuleData data = ConvertToCMData(frame);

                        if (OnDataChanged != null)
                        {
                            // System.Diagnostics.Debug.WriteLine("Data Changed.");

                            OnDataChanged(data);
                        }
                    }
                }


            }
            finally
            {
                StopAcq();

                CompassModulePort.Close();
            }
        }


        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopAcq()
        {
            CreateOpenPort(PcBaudRate);

            // 停止采集
            CompassModulePort.Write(CMD_SETSAMRATE_0Hz, 0, CMD_SETSAMRATE_0Hz.Length);

            // 寻找帧头
            if (ReadFrameHead())
            {
                // 读取帧数据
                byte[] frame = ReadFrame(5);

                System.Diagnostics.Debug.WriteLine("Start 0Hz Acquisition.");
            }
        }


        /// <summary>
        /// Set SampleRate
        /// </summary>
        private void SetSampleRate()
        {
            if (CompassModulePort.IsOpen == false)
            {
                CompassModulePort.Open();
            }

            CompassModulePort.ReadTimeout = 1000;

            if (SampleRate == "50Hz")
            {
                // 发送50Hz 采集数据命令
                CompassModulePort.Write(CMD_SETSAMRATE_50Hz, 0, CMD_SETSAMRATE_50Hz.Length);
            }
            else if (SampleRate == "25Hz")
            {
                CompassModulePort.Write(CMD_SETSAMRATE_25Hz, 0, CMD_SETSAMRATE_25Hz.Length); 
            }
            else if (SampleRate == "15Hz")
            {
                CompassModulePort.Write(CMD_SETSAMRATE_15Hz, 0, CMD_SETSAMRATE_15Hz.Length);
            }
            else if (SampleRate == "10Hz")
            {
                CompassModulePort.Write(CMD_SETSAMRATE_10Hz, 0, CMD_SETSAMRATE_10Hz.Length);
            }
            else if (SampleRate == "5Hz")
            {
                CompassModulePort.Write(CMD_SETSAMRATE_5Hz, 0, CMD_SETSAMRATE_5Hz.Length);
            }

            // 寻找帧头
            if (ReadFrameHead())
            {
                // 读取帧数据
                byte[] frame = ReadFrame(5);

                System.Diagnostics.Debug.WriteLine("Start 5Hz Acquisition.");
            }
        }


        private Task AcquisitionTask
        {
            get;
            set;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopAcquisition()
        {
            IsAcquisiting = false;

            AcquisitionTask.Wait();

            // 设置默认波特率
            //if (PcBaudRate != DefaultBaudRate)
            //{
            //    TrySetBaudRate(CMBaudRate, DefaultBaudRate);
            //}

            System.Diagnostics.Debug.WriteLine("AcquisitionTask Stopped.");
        }

        /// <summary>
        /// 正在连续采集？
        /// </summary>
        public bool IsAcquisiting
        {
            get;
            set;
        }


        /// <summary>
        /// 读取一个帧
        /// </summary>
        /// <returns></returns>
        private byte[] ReadFrame(int frameLen)
        {
            //try
            //{

                int inx = 0;
                // 读取数据长度
                byte len = (byte)CompassModulePort.ReadByte();
                // System.Diagnostics.Debug.WriteLine(len);

                if (len != frameLen)
                {
                    //errByte.Add(len);

                    return null;
                }

                byte[] frame = new byte[len];

                frame[0] = len;

                for (int buf = 1; buf < len; buf++)
                {
                    frame[buf] = (byte)CompassModulePort.ReadByte();
                    inx++;
                }

                // 验证校验和
                int check = 0;
                for (int buf = 0; buf < frame.Length - 1; buf++)
                {
                    check += frame[buf];
                }

                byte[] checkBytes = BitConverter.GetBytes(check);

                if (frame[frame.Length - 1] != checkBytes[0])
                {
                    //errByte.AddRange(frame);

                    return null;
                }
                return frame;
            //}
            //catch
            //{
            //    return null;
            //}
        }
        

        // 读取全部寄存器
        private byte[] CMD_READALL = new byte[] { 0x77, 0x04, 0x00, 0x04, 0x08};

        // 设置采样频率5Hz
        private byte[] CMD_SETSAMRATE_5Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x01, 0x12 };

        // 设置采样频率响应帧
        private byte[] CMDRES_SETSAMRATE_5Hz = new byte[] { 0x77, 0x05, 0x00, 0x8C, 0x00, 0x91};
        
        // 设置采样频率50Hz
        private byte[] CMD_SETSAMRATE_50Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x05, 0x16};

        // 设置采样频率10Hz
        private byte[] CMD_SETSAMRATE_10Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x02, 0x13 };

        // 设置采样频率15Hz
        private byte[] CMD_SETSAMRATE_15Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x03, 0x14 };


        // 设置采样频率25Hz
        private byte[] CMD_SETSAMRATE_25Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x04, 0x15 };


        // 设置采样模式0Hz，即应答式
        private byte[] CMD_SETSAMRATE_0Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x00, 0x11 };


        // 设置波特率115200，57600,38400,19200,9600,4800,2400
        //           04      07    06    03    02   01   00
        private byte[] CMD_SETBAUDRATE_9600 = new byte[] { 0x77, 0x05, 0x00, 0x0B, 0x02, 0x12 };
        private byte[] CMD_SETBAUDRATE_19200 = new byte[] { 0x77, 0x05, 0x00, 0x0B, 0x03, 0x13 };
        private byte[] CMD_SETBAUDRATE_115200 = new byte[] { 0x77, 0x05, 0x00, 0x0B, 0x04, 0x14 };
        private byte[] CMD_SETBAUDRATE_38400 = new byte[] { 0x77, 0x05, 0x00, 0x0B, 0x06, 0x16 };
        private byte[] CMD_SETBAUDRATE_57600 = new byte[] { 0x77, 0x05, 0x00, 0x0B, 0x07, 0x17 };

        // 设置波特率响应帧
        private byte[] CMDRES_CMD_SETBAUDRATE = new byte[] { 0x77, 0x05, 0x00, 0x8B, 0x00, 0x90 };


        //List<byte> errByte = new List<byte>();
        
        /// <summary>
        /// 转换Witlink BCD，每个数字3个字节（6个四位），首个四位代表符号，接下来3个四位代表整数，最后2个四位代表小树
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private string ConvertWitLinkBCD(byte[] buffer, int offset)
        {
            byte[] angle = new byte[3];

            StringBuilder sb = new StringBuilder();

            Buffer.BlockCopy(buffer, offset, angle, 0, 3);

            // 1st four bits.
            int sign = (byte)(angle[0] >> 4);
            if(sign == 1) 
            {
                sign = -1;

                sb.Append("-");
            }
            else
            {
                sign = 1;
            }

            byte hundreds = (byte)(angle[0] & 0x0F);
            byte tens = (byte)((angle[1] & 0xF0) >> 4);
            byte units = (byte)(angle[1] & 0x0F);
            byte tenths = (byte)((angle[2] & 0xF0) >> 4);
            byte hundredths = (byte)(angle[2] & 0x0F);
            
            // 百位有数据
            if (hundreds > 0)
            {
                sb.Append(hundreds);
                sb.Append(tens);
                sb.Append(units);
                sb.Append(".");
                sb.Append(tenths);
                sb.Append(hundredths);
            }
            else if (tens > 0)
            {
                sb.Append(tens);
                sb.Append(units);
                sb.Append(".");
                sb.Append(tenths);
                sb.Append(hundredths);
            }
            else
            {
                sb.Append(units);
                sb.Append(".");
                sb.Append(tenths);
                sb.Append(hundredths);
            }

            return sb.ToString();
            
            //return sign * (hundreds * 100 + tens * 10 + units + tens * 0.1 + hundredths * 0.01);
        }
    }
}
