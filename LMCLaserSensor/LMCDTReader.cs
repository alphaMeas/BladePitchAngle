using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMCLaserSensor
{

    public  delegate void DataRecieved(List<int> intData);
    class LMCDTHexReader : ILMCDTReader
    {
        public LMCDTHexReader()
        {
            IsRunning = false;
        }


        Task thisTask;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public void StartRead(SerialPort sp)
        {
            IsRunning = true;

            allStrData = new List<string>();

            allIntData = new List<int>();

            thisTask = Task.Factory.StartNew(ReadData, sp);
        }

        public void StopRead(DateTime startTime)
        {
            IsRunning = false;

            if (thisTask != null)
            {
                thisTask.Wait(3000);
            }

            // 保存原始数据
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, startTime.ToString("yyyyMMddHHmmss_S") + ".txt");

            if (allStrData != null && allStrData.Count > 0)
            {
                using (System.IO.TextWriter tw = System.IO.File.CreateText(fileName))
                {
                    foreach (string str in allStrData)
                    {
                        tw.Write(str);
                    }

                    tw.WriteLine();

                    tw.WriteLine(sw.ElapsedMilliseconds.ToString());
                }
            }

            // 保存Int 数据
            string fileName2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, startTime.ToString("yyyyMMddHHmmss_I") + ".txt");

            if (allIntData != null && allIntData.Count > 0)
            {
                using (System.IO.TextWriter tw = System.IO.File.CreateText(fileName2))
                {
                    foreach (int i in allIntData)
                    {
                        tw.WriteLine(i.ToString());
                    }
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

                // 停止采集后 读取残余数据
                ReadProtocol(sp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        private void ReadProtocol(SerialPort sp)
        {
            string strdata = sp.ReadExisting();

            if (strdata.Length == 0)
            {
                System.Threading.Thread.Sleep(10);

                return;
            }

            // 保存全部原始数据
            allStrData.Add(strdata);

            List<int> intData = ProcessData(strdata);

            allIntData.AddRange(intData);

            if (OnDataRecieved != null)
            {
                OnDataRecieved(intData);
            }
        }
        

        List<string> allStrData;
        List<int> allIntData;

        string buf = "";
        private List<int> ProcessData(string strdata)
        {
            // 之前遗留的数据连接
            string[] data = (buf + strdata).Split('H');

            List<int> intdata = new List<int>();

            for (int inx = 1; inx < data.Length - 1; inx++)
            {
                string str = data[inx];

                str = str.Replace("\r\n", "");

                // 示例：H00027B, 6个字符
                if (str.Trim().Length >= 6)
                {
                    intdata.Add(Convert.ToInt32(str, 16));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("");

                    System.Diagnostics.Debug.Write("Error:");

                    System.Diagnostics.Debug.WriteLine(str);

                }
            }

            // 最后一个数据是否完整
            if (data[data.Length - 1].Length > 7)
            {
                string str = data[data.Length - 1];
                str = str.Replace("\r\n", "");
                intdata.Add(Convert.ToInt32(str, 16));
            }
            else
            {
                buf = data[data.Length - 1];

                //System.Diagnostics.Debug.Write(buf);
            }

            return intdata;
        }

        public event DataRecieved OnDataRecieved;
    }
}
