using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BladePitchAngle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 1);

            timer.Tick += timer_Tick;
            
            VMThis = new MainViewModel();

            //this.BtnSigleFrame.IsEnabled = false;
            //this.Btn5HzFrame.IsEnabled = false;
            //this.SaveData.IsEnabled = false;


            // 串口初始化
            String[] Portname = System.IO.Ports.SerialPort.GetPortNames();            
            foreach(string pn in Portname)
            {
                CbPort.Items.Add(pn);
            }
            if (CbPort.Items.Count > 0)
            {
                CbPort.SelectedIndex = 0;
            }

            // 采样频率初始化
            CbSampleRate.Items.Add("5Hz");
            CbSampleRate.Items.Add("10Hz");
            CbSampleRate.Items.Add("15Hz");
            CbSampleRate.Items.Add("25Hz");
            CbSampleRate.Items.Add("50Hz");

            CbSampleRate.SelectedIndex = 4;

            //波特率初始化
            foreach(string br in Defines.BaudRates)
            {
                CbBaudRate.Items.Add(br);
            }

            CbBaudRate.SelectedIndex = 1;
            this.DataContext = VMThis;

            this.Title = "叶片相对安装角测量软件 " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        /// <summary>
        /// 读取单帧数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            cm.PortName = CbPort.SelectedItem.ToString();

            cm.SampleRate = CbSampleRate.SelectedItem.ToString();

            cm.PcBaudRate = CbBaudRate.SelectedItem.ToString();

            try
            {

                VMThis.CMDataRT = cm.ReadSigleFrameData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }


        /// <summary>
        /// 数据更新
        /// </summary>
        public MainViewModel VMThis
        {
            get;
            set;
        }

        CompassModule cm = new CompassModule();

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();


        public static readonly DependencyProperty IsNotContinueAcqProperty = DependencyProperty.Register(
            "IsNotContinueAcq", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        /// <summary>
        /// 非连续采集状态
        /// </summary>
        public bool IsNotContinueAcq
        {
            get
            {
                return (bool)GetValue(IsNotContinueAcqProperty);
            }
            set
            {
                SetValue(IsNotContinueAcqProperty, value);
            }
        }


        /// <summary>
        /// 连续采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ContinueAcq(object sender, RoutedEventArgs e)
        {
            try
            {
                // 启动采集
                if (IsNotContinueAcq == true)
                {
                    StartContinueAcq();

                    IsNotContinueAcq = false;

                    rbBlade1.IsEnabled = rbBlade1.IsChecked.Value;
                    rbBlade2.IsEnabled = rbBlade2.IsChecked.Value;
                    rbBlade3.IsEnabled = rbBlade3.IsChecked.Value;

                    //BtnSigleFrame.IsEnabled = false;
                    Btn5HzFrame.Content = "停止采集";
                }
                else
                {
                    IsNotContinueAcq = true;

                    Btn5HzFrame.Content = "连续采集";
                    //BtnSigleFrame.IsEnabled = true;
                    rbBlade1.IsEnabled = true;
                    rbBlade2.IsEnabled = true;
                    rbBlade3.IsEnabled = true;

                    StopCotinueAcq();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// 启动连续采集
        /// </summary>
        private void StartContinueAcq()
        {
            Seconds = 0;
            CMDataList = new List<CompassModuleData>();

            cm.PortName = CbPort.SelectedItem.ToString();
            cm.SampleRate = CbSampleRate.SelectedItem.ToString();
            cm.PcBaudRate = CbBaudRate.SelectedItem.ToString();

            cm.OnDataChanged += cm_OnDataChanged;

            // 启动连续采集
            cm.StartAcquisition();

            timer.Start();
        }


        /// <summary>
        /// 停止连续采集
        /// </summary>
        private void StopCotinueAcq()
        {
            timer.Stop();

            // 计算平均值数据
            UpdateAverage();

            // 自动保存上次数据
            AutoSave();

            // 停止连续采集
            cm.StopAcquisition();
        }
        

        /// <summary>
        /// 计算平均值
        /// </summary>
        private void UpdateAverage()
        {
            string max = "";
            string min = "";
            float fltmax = float.MinValue;
            float fltmin = float.MaxValue;

            double sum = 0;
            int number = 0;
            foreach (CompassModuleData cmd in CMDataList)
            {
                float fltangle = float.Parse(cmd.HeadingAngle);

                // 最大值
                if (fltangle > fltmax)
                {
                    fltmax = fltangle;
                    max = cmd.HeadingAngle;
                }

                // 最小值
                if (fltangle < fltmin)
                {
                    fltmin = fltangle;
                    min = cmd.HeadingAngle;
                }

                sum += fltangle;
                number++;
            }

            double average = sum / number;

            BladeAngleData bad = new BladeAngleData();
            bad.Average = average.ToString("F3");
            bad.Max = max;
            bad.Min = min;
            
            if (rbBlade1.IsChecked == true)
            {
                VMThis.BladeHeading1 = bad;
            }

            if (rbBlade2.IsChecked == true)
            {
                VMThis.BladeHeading2 = bad;
            }

            if (rbBlade3.IsChecked == true)
            {
                VMThis.BladeHeading3 = bad;
            }

        }


        /// <summary>
        /// 更新采集时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            Seconds++;
            TimeElaps.Content = Seconds;

            if (CMDataList != null && CMDataList.Count > 1)
            {
                UpdateAverage();
            }
        }

        int Seconds;

        delegate void UpdateView(CompassModuleData newData);

        /// <summary>
        /// 采集数据到达
        /// </summary>
        /// <param name="newData"></param>
        void cm_OnDataChanged(CompassModuleData newData)
        {
            UpdateView ev2 = delegate(CompassModuleData args)
            {
                VMThis.CMDataRT = args;

                CMDataList.Add(args);
            };

            Dispatcher.BeginInvoke(ev2, newData) ;            
        }


        private List<CompassModuleData> CMDataList;


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            if (CMDataList == null || CMDataList.Count == 0)
            {
                MessageBox.Show("请先采集数据。");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            //设置这个对话框的起始保存路径  
            sfd.InitialDirectory = @"D:\";
            //设置保存的文件的类型，注意过滤器的语法  
            sfd.Filter = "CSV文件|*.csv";
            //调用ShowDialog()方法显示该对话框，该方法的返回值代表用户是否点击了确定按钮  
            if (sfd.ShowDialog() == true)
            {
                SaveSrcData(sfd.FileName);
            }
        }


        /// <summary>
        /// 自动保存数据
        /// </summary>
        private void AutoSave()
        {
            try
            {
                if (CMDataList == null || CMDataList.Count == 0)
                {
                    return;
                }

                string blade = "b1";
                if (rbBlade2.IsChecked == true)
                {
                    blade = "b2";
                }
                if (rbBlade3.IsChecked == true)
                {
                    blade = "b3";
                }

                string fileName = string.Format(@"d:\{0}_{1}.csv", blade, DateTime.Now.ToString("yyyyMMddHHmmss"));

                SaveSrcData(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 保存原始数据
        /// </summary>
        /// <param name="sfd"></param>
        private void SaveSrcData(string fileFullName)
        {
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(fileFullName))
            {
                fs.Write("俯仰");
                fs.Write(", ");
                fs.Write("横滚");
                fs.Write(", ");
                fs.WriteLine("航向");
                foreach (CompassModuleData cmd in CMDataList)
                {
                    //fs.WriteLine(f.ToString());
                    fs.Write(cmd.PitchAngle);
                    fs.Write(", ");
                    fs.Write(cmd.RollAngle);
                    fs.Write(", ");
                    fs.WriteLine(cmd.HeadingAngle);
                }
            }
        }



        /// <summary>
        /// 设置波特率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SetBaudRate(object sender, RoutedEventArgs e)
        {
            cm.PortName = CbPort.SelectedItem.ToString();

            cm.SampleRate = CbSampleRate.SelectedItem.ToString();

            cm.PcBaudRate = CbBaudRate.SelectedItem.ToString();

            try
            {
                // 搜索设备的当前波特率。
                string cmBaudrate = cm.SearchBaudRate();

                cm.CMBaudRate = cmBaudrate;

                // 需要重新设置baudrate？
                if (cm.CMBaudRate != cm.PcBaudRate)
                {
                    cm.TrySetBaudRate(cmBaudrate, cm.PcBaudRate);
                }

                //this.BtnSigleFrame.IsEnabled = true;
                //this.Btn5HzFrame.IsEnabled = true;
                //this.SaveData.IsEnabled = true;

                MessageBox.Show(string.Format("波特率：{0}", cm.PcBaudRate));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }

        }



        /// <summary>
        /// 波特率改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbBaudRate_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //this.BtnSigleFrame.IsEnabled = false;
            //this.Btn5HzFrame.IsEnabled = false;
            //this.SaveData.IsEnabled = false;
            cm.PcBaudRate = CbBaudRate.SelectedItem.ToString();
        }


        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 如果正在连续采集，则停止下来
            if (IsNotContinueAcq == false)
            {
                StopCotinueAcq();
            }
        }
    }
}
