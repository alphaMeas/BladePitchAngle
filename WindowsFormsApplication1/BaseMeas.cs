using BladePitchAngle;
using LMCLaserSensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// 采集数据 5Hz 角度
    /// </summary>
    abstract class BaseMeas
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="agPortName"></param>
        /// <param name="lsPoatName"></param>
        public BaseMeas(LMCLaserSensor.LMCSensor lsSensor, BladePitchAngle.CompassModule agSensor)
        {
            lmc301 = lsSensor;

            cm = agSensor;
        }


        protected LMCSensor lmc301
        { get; set; }


        /// <summary>
        /// 启动采集
        /// </summary>
        public void StartMeas()
        {
            OnStart();
            // 1. Angle Acquisition
            CMDataList = new List<CompassModuleData>();
            AngleDataList = new List<double>();
            cm.OnDataChanged += cm_OnDataChanged;

            // start continuing acquisition
            cm.StartAcquisition();

            // 2. laser acquisition
            LaserDataList = new List<double>();
            lmc301.OnDataRecieved += Lmc301_OnDataRecieved;

            Temperature = lmc301.GetTempreture();

            lmc301.ContinueMeas();

            startTime = DateTime.Now;
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// 角度平均值
        /// </summary>
        public double AverageAngle { get; set; }

        protected List<double> AngleDataList = new List<double>();

        /// <summary>
        /// angle data arrived
        /// </summary>
        /// <param name="newData"></param>
        void cm_OnDataChanged(CompassModuleData newData)
        {
            CMDataList.Add(newData);

            double alpha = float.Parse(newData.PitchAngle);

            AngleDataList.Add(alpha);

            // update angle
            AverageAngle = AngleDataList.Average();
        }


        /// <summary>
        /// 最新测量值
        /// </summary>
        public double LatestDisValue {
            get {
                if (LaserDataList.Count > 0)
                    return (LaserDataList[LaserDataList.Count - 1]);
                else
                    return 0;
            }

        }

        protected List<double> LaserDataList = new List<double>();

        /// <summary>
        /// Laser Data Arrived.
        /// </summary>
        /// <param name="intData"></param>
        private void Lmc301_OnDataRecieved(List<int> intData)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("lmc301 Base Data {0}", intData.Count));

            intData.ForEach( i=> LaserDataList.Add((double)i / 1000.0));

            OnLaserDataArrived(intData);
        }


        protected virtual void OnLaserDataArrived(List<int> intData)
        {

        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopMeas()
        {
            cm.OnDataChanged -= cm_OnDataChanged;
            lmc301.OnDataRecieved -= Lmc301_OnDataRecieved;

            // 停止角度连续采集
            cm.StopAcquisition();

            lmc301.StopMeas();

            // 自动保存上次数据
            AutoSaveAngleData();

            OnStop();
        }

        protected virtual void OnStop()
        {
        }

        protected CompassModule cm { get; set; }

        List<CompassModuleData> CMDataList;

        protected DateTime startTime
        {
            get;set;
        }


        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature
        {
            get;
            set;
        }

        /// <summary>
        /// 自动保存数据
        /// </summary>
        private void AutoSaveAngleData()
        {
            if (CMDataList == null || CMDataList.Count == 0)
            {
                return;
            }
            //string fileName = string.Format(@"d:\{0}_{1}.csv", blade, DateTime.Now.ToString("yyyyMMddHHmmss"));
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, startTime.ToString("yyyyMMddHHmmss") + "_Angle.txt");

            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(fileName))
            {
                fs.Write("俯仰");
                fs.Write(", ");
                fs.Write("横滚");
                fs.Write(", ");
                fs.WriteLine("航向");
                foreach (CompassModuleData cmd in CMDataList)
                {
                    fs.Write(cmd.PitchAngle);
                    fs.Write(", ");
                    fs.Write(cmd.RollAngle);
                    fs.Write(", ");
                    fs.WriteLine(cmd.HeadingAngle);
                }
            }

        }

    }
}
