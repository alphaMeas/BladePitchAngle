using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMCLaserSensor
{

    class LMCDTBinReader : ILMCDTReader
    {
        public LMCDTBinReader()
        {
            IsRunning = false;
        }


        Task thisTask;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public void StartRead(SerialPort sp)
        {
            IsRunning = true;

            allStrData = new List<byte[]>();

            allIntData = new List<int>();

            buf = new List<byte>();

            thisTask = Task.Factory.StartNew(ReadData, sp);
        }

        public void StopRead(DateTime startTime)
        {
            IsRunning = false;

            if (thisTask != null)
            {
                thisTask.Wait(5000);
            }

            // 保存Int 数据
            string fileName2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, startTime.ToString("yyyyMMddHHmmss") + "_laser.txt");

            if (allIntData != null && allIntData.Count > 0)
            {
                using (System.IO.TextWriter tw = System.IO.File.CreateText(fileName2))
                {
                    foreach (int i in allIntData)
                    {
                        tw.WriteLine(i.ToString());
                    }

                    tw.WriteLine(sw.ElapsedMilliseconds.ToString());
                }
            }
        }

        private bool IsRunning;
        void ReadData<T>(T  param)
        {
            SerialPort sp = param as SerialPort;
            try
            {
                System.Diagnostics.Debug.WriteLine("start Reading.");
                // 启动计时器
                sw.Reset();
                sw.Start();

                while (IsRunning)
                {
                    ReadProtocol(sp);
                }

                // 停止计时器
                sw.Stop();

                System.Threading.Thread.Sleep(100);

                // 停止采集后 读取残余数据
                ReadProtocol(sp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            System.Diagnostics.Debug.WriteLine("Reading thread Exit.");

        }


        private void ReadProtocol(SerialPort sp)
        {
            int count = sp.BytesToRead;
            if (count >= 3)
            {
                byte[] buffer = new byte[count];
                sp.Read(buffer, 0, count);
                
                // 保存全部原始数据
                allStrData.Add(buffer);

                List<int> intData = ProcessData(buffer);

                allIntData.AddRange(intData);

                if (OnDataRecieved != null)
                {
                    OnDataRecieved(intData);
                }
            }
        }
        

        List<byte[]> allStrData;
        List<int> allIntData;

        List<byte> buf = new List<byte>();

        private List<int> ProcessData(byte[] strdata)
        {
            List<byte> tempBuf = new List<byte>();
            tempBuf.AddRange(buf);
            tempBuf.AddRange(strdata);
            System.Diagnostics.Debug.WriteLine(string.Format("Buf Len = {0}", buf.Count));

            buf = new List<byte>();

            List<int> intdata = new List<int>(); 

            // 每次三个字节
            for (int inx = 0; inx < tempBuf.Count; inx++ )
            {
                // 不够一个数据（3个字节）
                if ((inx + 2) >= tempBuf.Count)
                {
                    buf.Add(tempBuf[inx]);
                    continue;
                }

                // 第1个字节 最高位为1
                int byte1 = tempBuf[inx];
                int temp1 = byte1 & 0x80;

                // 第2个字节 最高位为0
                int byte2 = tempBuf[inx+1];
                int temp2 = byte2 & 0x80;

                // 第3个字节 最高位为0
                int byte3 = tempBuf[inx + 2];
                int temp3 = byte3 & 0x80;

                int data;
                // 协议验证通过
                if (temp1 == 0x80 && temp2 == 0 && temp3 == 0)
                {
                    // 最高位1 消除。
                    byte1 = byte1 & 0x7f;
                    byte1 = byte1 << 14;

                    byte2 = byte2 << 7;

                    data = byte1 | byte2 | byte3;

                    intdata.Add(data);

                    inx += 2;
                }
                else
                {
                    System.Diagnostics.Debug.Write("Error:");
                }
            }

            return intdata;
        }

        public event DataRecieved OnDataRecieved;
    }
}
