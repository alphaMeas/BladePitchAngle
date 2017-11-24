using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class BladeMeas : BaseMeas
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="agPortName"></param>
        /// <param name="lsPoatName"></param>
        public BladeMeas(LMCLaserSensor.LMCSensor lsSensor, BladePitchAngle.CompassModule agSensor)
            :base(lsSensor, agSensor)
        {
        }    

        /// <summary>
        /// 启动
        /// </summary>
        protected override void OnStart()
        {
            LastValue = 0;
            currentObj = null;
            ObjList = new List<MeasObj>();
        }

        private int LastValue = 0;
        private MeasObj currentObj = null;

        private List<MeasObj> ObjList;


        /// <summary>
        /// 激光器数据到达
        /// </summary>
        /// <param name="intData"></param>
        protected override void OnLaserDataArrived(List<int> intData)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Blade Data {0}", intData.Count));
            // first time.
            if (currentObj == null)
            {
                LastValue = intData[0];
                currentObj = new MeasObj();
            }

            int count = intData.Count;

            // 所有数据点
            for (int inx = 0; inx < count; inx++)
            {
                // 无效数据
                if (intData[inx] == 1090258) intData[inx] = LastValue;

                // 距离发生突变超过 50cm， 应该是过度，或者新对象。
                if (Math.Abs(LastValue - intData[inx]) > AppDefines.OBJDELTA)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("NewObj v1 {0} -- V2 {1}",LastValue, intData[inx]));
                    // 超过50个点 才是一个新的对象。
                    if (currentObj.MeasData.Count > AppDefines.OBJMINCOUNT)
                    {
                        currentObj.Max = currentObj.MeasData.Max();
                        currentObj.Min = currentObj.MeasData.Min();
                        currentObj.Average = currentObj.MeasData.Average();
                        
                        ObjList.Add(currentObj);
                    }
                    // 上个对象的过渡数据
                    else if (ObjList.Count > 0 && currentObj.MeasData.Count > 0)
                    {
                        ObjList[ObjList.Count - 1].MiddleDataList.AddRange(currentObj.MeasData);
                    }

                    currentObj = new MeasObj();
                }

                // 数据加入当前对象 单位：m
                currentObj.MeasData.Add((double)intData[inx] / 1000.0);

                LastValue = intData[inx];
            }


            // 超过一转
            if (ObjList.Count > 5)
            {
                // first time.
                if (ObjList[0].MeasObjType == EnumObjType.Unknown)
                {
                    // 第一个对象有可能是没用的
                    double temp1 = ObjList[1].MeasData.Max() - ObjList[1].MeasData.Min();
                    double temp2 = ObjList[2].MeasData.Max() - ObjList[2].MeasData.Min();

                    // 差值大的为叶片
                    if (temp1 > temp2)
                    {
                        ObjList[0].MeasObjType = EnumObjType.Tower;
                        ObjList[1].MeasObjType = EnumObjType.Blade;
                        ObjList[2].MeasObjType = EnumObjType.Tower;
                    }
                    else
                    {
                        ObjList[0].MeasObjType = EnumObjType.Blade;
                        ObjList[1].MeasObjType = EnumObjType.Tower;
                        ObjList[2].MeasObjType = EnumObjType.Blade;
                    }
                }

                int cc = ObjList.Count;

                for(int inx = 1; inx < cc; inx++)
                {
                    if (ObjList[inx].MeasObjType == EnumObjType.Unknown)
                    {
                        if (ObjList[inx - 1].MeasObjType == EnumObjType.Tower)
                        {
                            ObjList[inx].MeasObjType = EnumObjType.Blade;
                        }
                        else
                        {
                            ObjList[inx].MeasObjType = EnumObjType.Tower;
                        }
                    }
                }
            }
        }
        

        /// <summary>
        /// RotSpeed
        /// </summary>
        public double? RotSpeed
        {
            get
            {
                if (ObjList == null || ObjList.Count < AppDefines.BLADENUM2)
                {
                    return null;
                }

                int cc = ObjList.Count;

                double time = 0;

                for (int inx = AppDefines.BLADENUM2; inx >= 1; inx--)
                {
                    time += (ObjList[cc - inx].MeasData.Count + ObjList[cc - inx].MiddleDataList.Count);
                }

                // 单位：S
                time /= AppDefines.SAMPLERATE;

                // 单位：RPM
                time = 1 / time * 60;

                return time;
            }
        }


        /// <summary>
        /// 叶片位置的塔筒高度
        /// </summary>
        public double? TowerHeight
        {
            get
            {
                if (ObjList == null || ObjList.Count < AppDefines.BLADENUM2)
                {
                    return null;
                }

                double sum = 0;
                double count = 0;
                foreach (var obj in ObjList)
                {
                    if (obj.MeasObjType == EnumObjType.Tower)
                    {
                        sum += obj.MeasData.Sum();

                        count += obj.MeasData.Count;
                    }
                }

                double aveDis = sum / count ;

                double th = Math.Sin(Math.PI / 180 * AverageAngle) * aveDis;

                return th;
            }
        }

        
        /// <summary>
        /// get each Blade data.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public List<BladeData> GetBladeData(double r)
        {
            List<BladeData> bdList = new List<BladeData>();

            // 少于10 圈
            if (ObjList == null || ObjList.Count < AppDefines.BLADENUM2*10)
            {
                return bdList;
            }
            for (int inx = 0; inx < AppDefines.BLADENUM; inx++)
            {
                bdList.Add(new BladeData());
            }

            int bdIndex = 0;

            // 计算叶片宽度
            int cc = ObjList.Count;

            // 确保 inx 指向第一个完整叶片；
            int startBlade = 1;
            if (ObjList[0].MeasObjType == EnumObjType.Blade)
            {
                startBlade = 2;
            }

            // blade data.
            for (int objInx = startBlade; (objInx+1) < cc; objInx += 2)
            {
                // tower - blade
                double left = ObjList[objInx - 1].MeasData.Last() - ObjList[objInx].MeasData.First();

                double right = ObjList[objInx + 1].MeasData.First() - ObjList[objInx].MeasData.Last();

                // calculate blade width 
                double bladetime = (ObjList[objInx].MeasData.Count + ObjList[objInx].MiddleDataList.Count);
                double time = bladetime;
                // blade-tower-blade-tower-blade-...
                if (startBlade == 2)
                {
                    time += (ObjList[objInx - 1].MeasData.Count + ObjList[objInx - 1].MiddleDataList.Count);
                }
                // tower-blade-tower-blade-tower-...
                else
                {
                    time += (ObjList[objInx + 1].MeasData.Count + ObjList[objInx + 1].MiddleDataList.Count);
                }

                // one-third circle length.
                double oneThirdCircle = (2 * Math.PI * r) / 3.0;

                double bladeWidth = bladetime * (oneThirdCircle / time);

                bdList[bdIndex].LeftEdge.Add(left);
                bdList[bdIndex].RightEdge.Add(right);
                bdList[bdIndex].Width.Add(bladeWidth);

                bdIndex++;
                bdIndex = bdIndex % AppDefines.BLADENUM;
            }

            return bdList;
        }

        /// <summary>
        /// stop measurement.
        /// </summary>
        protected override void OnStop()
        {
            if (ObjList == null || ObjList.Count == 0)
            {
                return;
            }

            // 最后一个对象
            if (currentObj != null)
            {
                currentObj.Max = currentObj.MeasData.Max();
                currentObj.Min = currentObj.MeasData.Min();
                currentObj.Average = currentObj.MeasData.Average();

                ObjList.Add(currentObj);
            }

            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, startTime.ToString("yyyyMMddHHmmss") + "_Obj.txt");

            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(fileName))
            {
                foreach (var obj in ObjList)
                {
                    fs.Write("Obj");
                    fs.Write(obj.MeasObjType);
                    fs.Write("  Ave:");
                    fs.Write(obj.Average);
                    fs.Write("  Max:");
                    fs.Write(obj.Max);
                    fs.Write("  Min:");
                    fs.Write(obj.Min);
                    fs.Write("  Time:");
                    fs.Write(obj.MeasData.Count);
                    fs.Write("  Time Middle:");
                    fs.Write(obj.MiddleDataList.Count);
                    fs.WriteLine();
                }
            }
        }
    }    


    enum EnumObjType
    {
        Unknown =0,
        Tower = 1,
        Blade = 2
    }

    class MeasObj
    {
        public MeasObj()
        {
            MeasObjType = EnumObjType.Unknown;
            MeasData = new List<double>();
            MiddleDataList = new List<double>();
        }


        public double Max { get; set; }

        public double Min { get; set; }

        public double Average { get; set; }

        public EnumObjType MeasObjType { get; set; }

        /// <summary>
        /// 测量数据
        /// </summary>
        public List<double> MeasData { get; set; }

        /// <summary>
        /// 中间数据
        /// </summary>
        public List<double> MiddleDataList { get; set; }
    }


    class BladeData
    {
        public BladeData()
        {
            Width = new List<double>();

            LeftEdge = new List<double>();

            RightEdge = new List<double>();
        }



        /// <summary>
        /// calculate alpha 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public double CalcAlpha(double angle)
        {
            double left = this.LeftEdge.Average();

            double right = this.RightEdge.Average();

            double width = this.Width.Average();

            // 角度
            double bpa = Math.Atan(Math.Cos(Math.PI / 180 * angle) * (right - left) / width) / Math.PI * 180;

            return bpa;
        }


        public List<double> Width { get; set; }


        /// <summary>
        /// 左边缘
        /// </summary>
        public List<double> LeftEdge { get;set; }

        /// <summary>
        /// 右边缘
        /// </summary>
        public List<double> RightEdge { get; set; }
    }
}
