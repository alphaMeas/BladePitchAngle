using System;
using System.IO.Ports;

namespace LMCLaserSensor
{
    interface ILMCDTReader
    {
        event DataRecieved OnDataRecieved;

        void StartRead(SerialPort sp);
        void StopRead(DateTime startTime);
    }
}