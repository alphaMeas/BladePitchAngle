using BladePitchAngle;
using LMCLaserSensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 串口初始化
            String[] Portname = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string pn in Portname)
            {
                cbAngle.Items.Add(pn);
                cbLaser.Items.Add(pn);
            }
            if (Portname.Length > 0)
            {
                cbAngle.SelectedIndex = 1;
                cbLaser.SelectedIndex = 0;
            }
        }


        BaseMeas meas;


        /// <summary>
        /// timer tick update view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            Seconds++;

            // update time
            labelSec.Text = Seconds.ToString();

            try
            {
                // update Temprature
                labelTemp.Text = meas.Temperature;

                // update angle
                double alpha = meas.AverageAngle;
                labelAngle.Text = alpha.ToString("F2");

                // update Laser;
                double dis = meas.LatestDisValue;
                labelLaser.Text = dis.ToString("F3");

                if (IsBladeMeas == false)
                {
                    // update Tower Height 
                    if (tower.TowerHeight != null)
                    {
                        labelHight.Text = tower.TowerHeight.Value.ToString("F3");
                    }
                }
                else
                {
                    if (blade.RotSpeed != null)
                    {
                        labelRPM.Text = blade.RotSpeed.Value.ToString("F1");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        int Seconds;

        /// <summary>
        /// tower height measurment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTstart_Click(object sender, EventArgs e)
        {
            Seconds = 0;
            IsBladeMeas = false;

            if (tower == null)
            {
                ConfigSensor();

                tower = new TowerMeas(ls, cm);
            }

            meas = tower;

            try
            {
                tower.StartMeas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            timer1.Start();
        }

        TowerMeas tower;

        CompassModule cm = new CompassModule();
        LMCSensor ls = new LMCSensor();

        /// <summary>
        /// Config sensor.
        /// </summary>
        private void ConfigSensor()
        {
            cm.PortName = cbAngle.SelectedItem.ToString();
            cm.SampleRate = "5Hz";
            cm.PcBaudRate = "9600";

            ls.CreatePort(cbLaser.SelectedItem.ToString());
        }

        /// <summary>
        /// stop measurement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTstop_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            try
            {
                tower.StopMeas();

                // 记录 塔筒情况
                textBoxTA.Text = labelAngle.Text;
                textBoxTH.Text = labelHight.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        BladeMeas blade;

        bool IsBladeMeas = false;

        /// <summary>
        /// blade Meas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBladeStart_Click(object sender, EventArgs e)
        {
            Seconds = 0;
            IsBladeMeas = true;

            if (blade == null)
            {
                ConfigSensor();

                blade = new BladeMeas(ls, cm);
            }

            meas = blade;

            try
            {
                blade.StartMeas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            timer1.Start();
        }


        /// <summary>
        /// blade meas stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBladeStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            try
            {
                blade.StopMeas();

                // 记录 Blade Data
                textBoxBA.Text = labelAngle.Text;

                if (blade.TowerHeight != null)
                {
                    textBoxBH.Text = blade.TowerHeight.Value.ToString("F3");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// 计算 alpha 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCalc_Click(object sender, EventArgs e)
        {
            try
            {
                double r = tower.TowerHeight.Value - blade.TowerHeight.Value;

                double alpha = blade.AverageAngle;

                var bpa = blade.GetBladeData(r);
                if (bpa.Count < 3) return;

                labelB1.Text = bpa[0].CalcAlpha(alpha).ToString("F3");
                labelB2.Text = bpa[1].CalcAlpha(alpha).ToString("F3");
                labelB3.Text = bpa[2].CalcAlpha(alpha).ToString("F3");

                labelB1W.Text = bpa[0].Width.Average().ToString("F3");
                labelB2W.Text = bpa[1].Width.Average().ToString("F3");
                labelB3W.Text = bpa[2].Width.Average().ToString("F3");

                string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                    DateTime.Now.ToString("yyyyMMddHHmmss") + "_Blade.txt");

                using (System.IO.StreamWriter fs = new System.IO.StreamWriter(fileName))
                {
                    fs.Write(labelB1.Text);
                    fs.Write(", ");
                    fs.Write(labelB2.Text);
                    fs.Write(", ");
                    fs.WriteLine(labelB3.Text);

                    int bindex = 1; 
                    foreach (var obj in bpa)
                    {
                        fs.Write("B");
                        fs.WriteLine(bindex++);

                        fs.Write(" Width");
                        fs.Write("  Ave:");
                        fs.Write(obj.Width.Average());
                        fs.Write("  Max:");
                        fs.Write(obj.Width.Max());
                        fs.Write("  Min:");
                        fs.Write(obj.Width.Min());
                        fs.Write("  Count:");
                        fs.Write(obj.Width.Count);
                        fs.WriteLine();

                        fs.Write("Left Edge");
                        fs.Write("  Ave:");
                        fs.Write(obj.LeftEdge.Average());
                        fs.Write("  Max:");
                        fs.Write(obj.LeftEdge.Max());
                        fs.Write("  Min:");
                        fs.Write(obj.LeftEdge.Min());
                        fs.Write("  Count:");
                        fs.Write(obj.LeftEdge.Count);
                        fs.WriteLine();

                        fs.Write("Right Edge");
                        fs.Write("  Ave:");
                        fs.Write(obj.RightEdge.Average());
                        fs.Write("  Max:");
                        fs.Write(obj.RightEdge.Max());
                        fs.Write("  Min:");
                        fs.Write(obj.RightEdge.Min());
                        fs.Write("  Count:");
                        fs.Write(obj.RightEdge.Count);
                        fs.WriteLine();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
