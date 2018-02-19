using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace AntennaControlPackage
{
    class ServoController : IServoController
    {
        private const string StopCmd = "v0\r";                      //天线停止
        private const string ClockWise_2Cmd = "v-150\r";            //顺时针2
        private const string ClockWise_5Cmd = "v-375\r";            //顺时针5
        private const string ClockWise_10Cmd = "v-750\r";           //顺时针10
        private const string ClockWise_20Cmd = "v-1500\r";          //顺时针20
        private const string CounterClockWise_2Cmd = "v150\r";      //逆时针2
        private const string CounterClockWise_5Cmd = "v375\r";      //逆时针5
        private const string CounterClockWise_10Cmd = "v750\r";     //逆时针10
        private const string CounterClockWise_20Cmd = "v1500\r";    //逆时针20
        private SerialPort sp;

        public ServoController()
        {
            InitializeSerialPort(out sp);       //初始化串口设置
        }

        public void SetRotationRate(RotationRate rate)
        {
            if (!sp.IsOpen)
                return;
            switch(rate)
            {
                case RotationRate.ClockWise_2:
                    sp.Write(ClockWise_2Cmd);
                    break;
                case RotationRate.ClockWise_5:
                    sp.Write(ClockWise_5Cmd);
                    break;
                case RotationRate.ClockWise_10:
                    sp.Write(ClockWise_10Cmd);
                    break;
                case RotationRate.ClockWise_20:
                    sp.Write(ClockWise_20Cmd);
                    break;
                case RotationRate.CounterClockWise_2:
                    sp.Write(CounterClockWise_2Cmd);
                    break;
                case RotationRate.CounterClockWise_5:
                    sp.Write(CounterClockWise_5Cmd);
                    break;
                case RotationRate.CounterClockWise_10:
                    sp.Write(CounterClockWise_10Cmd);
                    break;
                case RotationRate.CounterClockWise_20:
                    sp.Write(CounterClockWise_20Cmd);
                    break;
                case RotationRate.Stop:
                    sp.Write(StopCmd);
                    break;
            }
        }

        private void InitializeSerialPort(out SerialPort port)
        {
            port = new SerialPort();

            string[] names = SerialPort.GetPortNames();
            string portName = "COM1";   //默认COM1，如果没有其他接口打开COM1

            foreach(string name in names)
            {
                if (name != "COM1")
                    portName = name;
            }
            port.PortName = portName;

            port.BaudRate = 9600;
            port.RtsEnable = true;
            port.StopBits = StopBits.One;
            port.DataBits = 8;
            port.Open();
        }
    }
}
