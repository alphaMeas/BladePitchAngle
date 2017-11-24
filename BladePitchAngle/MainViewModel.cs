using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BladePitchAngle
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 数据更新
        /// </summary>
        private CompassModuleData _CMDataRT;
        public CompassModuleData CMDataRT
        {
            get { return _CMDataRT; }
            set { _CMDataRT = value;
            OnPropertyChanged("CMDataRT");
            }
        }


        /// <summary>
        /// 叶片编号
        /// </summary>
        private string bladeCode;
        public string BladeCode
        {
            get { return bladeCode; }
            set { bladeCode = value;
            OnPropertyChanged("BladeCode");
            }
        }

        /// <summary>
        /// 机组编号
        /// </summary>
        private string turbineCode;
        public string TurbineCode
        {
            get { return turbineCode; }
            set { turbineCode = value;
            OnPropertyChanged("TurbineCode");
            }
        }


        private BladeAngleData bladeHeading1;
        /// <summary>
        /// 叶片1角度
        /// </summary>
        public BladeAngleData BladeHeading1
        {
            get { return bladeHeading1; }
            set { bladeHeading1 = value;
            OnPropertyChanged("BladeHeading1");
            }
        }

        private BladeAngleData bladeHeading2;
        /// <summary>
        /// 叶片2角度
        /// </summary>
        public BladeAngleData BladeHeading2
        {
            get { return bladeHeading2; }
            set
            {
                bladeHeading2 = value;
                OnPropertyChanged("BladeHeading2");
            }
        }


        private BladeAngleData bladeHeading3;
        /// <summary>
        /// 叶片3角度
        /// </summary>
        public BladeAngleData BladeHeading3
        {
            get { return bladeHeading3; }
            set
            {
                bladeHeading3 = value;
                OnPropertyChanged("BladeHeading3");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
