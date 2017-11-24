using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class TowerMeas : BaseMeas
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="agPortName"></param>
        /// <param name="lsPoatName"></param>
        public TowerMeas(LMCLaserSensor.LMCSensor lsSensor, BladePitchAngle.CompassModule agSensor)
            :base(lsSensor, agSensor)
        {
        }

        double LaserDisSum = 0;

        /// <summary>
        /// 启动
        /// </summary>
        protected override void OnStart()
        {
            LaserDisSum = 0;

            TowerVibData = new List<double>();
        }

        protected override void OnLaserDataArrived(List<int> intData)
        {
            LaserDisSum += (double)intData.Sum()/1000.0;
        }

        /// <summary>
        /// Tower Height 
        /// </summary>
        public double? TowerHeight
        {
            get
            {
                // 有数据才可以计算
                if ( AngleDataList.Count > 0 && LaserDataList.Count > 0)
                {
                    int count = LaserDataList.Count;

                    // meter unit.
                    double aveDis = LaserDisSum / count ;

                    double th = Math.Sin(Math.PI / 180 * AverageAngle) * aveDis;

                    return th;
                }

                return null;
            }
        }


        List<double> TowerVibData = new List<double>();

        /// <summary>
        /// Tower Viberation
        /// </summary>
        public List<double> TowerVibWaveForm
        {
            get
            {
                if (LaserDataList.Count > 0)
                {
                    int count = LaserDataList.Count;

                    double aveDis = LaserDisSum / count;

                    for (int inx = 0; inx < count; inx++)
                    {
                        double tv = (LaserDataList[inx] - aveDis);

                        TowerVibData.Add(tv);
                    }
                }

                return TowerVibData;
            }
        }

    }
}
