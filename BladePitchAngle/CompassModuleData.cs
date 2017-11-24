using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BladePitchAngle
{
    public class CompassModuleData : INotifyPropertyChanged
    {
        private string pitchAngle;

        /// <summary>
        /// 俯仰
        /// </summary>
        public string PitchAngle
        {
            get { return pitchAngle; }
            set { 
                pitchAngle = value; 
                OnPropertyChanged("PitchAngle"); 
            }
        }


        private string rollAngle;

        /// <summary>
        /// 横滚
        /// </summary>        
        public string RollAngle
        {
          get { return rollAngle; }
          set { rollAngle = value;
          OnPropertyChanged("RollAngle");
          }
        }

        private string headingAngle;
        /// <summary>
        /// 航向
        /// </summary>
        public string HeadingAngle
        {
            get { return headingAngle; }
            set { headingAngle = value;
            OnPropertyChanged("HeadingAngle");
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
