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
        private const string ClockWise2Cmd = "v-150\r";            //顺时针2
        private const string ClockWise5Cmd = "v-375\r";            //顺时针5
        private const string ClockWise10Cmd = "v-750\r";           //顺时针10
        private const string ClockWise20Cmd = "v-1500\r";          //顺时针20
        private const string CounterClockWise2Cmd = "v150\r";      //逆时针2
        private const string CounterClockWise5Cmd = "v375\r";      //逆时针5
        private const string CounterClockWise10Cmd = "v750\r";     //逆时针10
        private const string CounterClockWise20Cmd = "v1500\r";    //逆时针20
        private SerialPort sp;

        public ServoController()
        {
            InitializeSerialPort(out sp);       //初始化串口设置
        }

        public void SetRotationRate(RotateMode mode)
        {
            if (!sp.IsOpen)
                return;
            switch(mode)
            {
                case RotateMode.ClockWise2:
                    sp.Write(ClockWise2Cmd);
                    break;
                case RotateMode.ClockWise5:
                    sp.Write(ClockWise5Cmd);
                    break;
                case RotateMode.ClockWise10:
                    sp.Write(ClockWise10Cmd);
                    break;
                case RotateMode.ClockWise20:
                    sp.Write(ClockWise20Cmd);
                    break;
                case RotateMode.CounterClockWise2:
                    sp.Write(CounterClockWise2Cmd);
                    break;
                case RotateMode.CounterClockWise5:
                    sp.Write(CounterClockWise5Cmd);
                    break;
                case RotateMode.CounterClockWise10:
                    sp.Write(CounterClockWise10Cmd);
                    break;
                case RotateMode.CounterClockWise20:
                    sp.Write(CounterClockWise20Cmd);
                    break;
                case RotateMode.Stop:
                    sp.Write(StopCmd);
                    break;
            }
        }

        private static void InitializeSerialPort(out SerialPort port)
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
