using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMCLaserSensor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// stop meas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, EventArgs e)
        {
            CreateSencor();

            lmc301.StopMeas();

            timer1.Stop();

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            // 串口初始化
            String[] Portname = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string pn in Portname)
            {
                comboBox1.Items.Add(pn);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }


        private void CreateSencor()
        {
            if (lmc301 == null)
            {
                lmc301 = new LMCSensor();
            }

            lmc301.CreatePort(comboBox1.Text);
        }

        private LMCSensor lmc301;

        private void buttonDM_Click(object sender, EventArgs e)
        {
            CreateSencor();

            string te = lmc301.GetSigleDis();

            label2.Text = (te);

        }

        /// <summary>
        /// 温度采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTemp_Click(object sender, EventArgs e)
        {
            CreateSencor();

            string te = lmc301.GetTempreture();

            label1.Text = (te);
        }

        private void buttonStart_Click_1(object sender, EventArgs e)
        {
            CreateSencor();

            lmc301.OnDataRecieved += Lmc301_OnDataRecieved;

            lmc301.ContinueMeas();

            second = 0;

            timer1.Start();
        }

        private static int LastValue;

        private List<int> currentObj = new List<int>();
        private void Lmc301_OnDataRecieved(List<int> intData)
        {
            foreach (int i in intData)
            {
                // 新的对象 ，距离突变50cm
                if (Math.Abs(LastValue - i) > 500 && currentObj.Count > 0)
                {
                    System.Diagnostics.Debug.Write("Hits:");
                    System.Diagnostics.Debug.WriteLine(currentObj.Count);

                    currentObj = new List<int>();
                }

                LastValue = i;

                currentObj.Add(i);
            }
            
            //throw new NotImplementedException();
        }

        private void buttonSA2_Click(object sender, EventArgs e)
        {
            CreateSencor();

            // 平均次数
            string temp = lmc301.SetSA1();

            // 输出格式 2进制
            string temp2 = lmc301.SetOutPut(2);

            label3.Text = temp + temp2;
        }

        int second = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            second++;

            label4.Text = "Time:" + second;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CreateSencor();

            // 输出格式 2进制
            string temp2 = lmc301.SetOutPut(10);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            CreateSencor();

            lmc301.Stop();
        }
    }
}
