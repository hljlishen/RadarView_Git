using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public class SystemCommunicator
    {
        private const int LocalComponentNumber = 0x11;   //本机设备编号
        private const int SysteComponentmNumber = 0x31;  //系统编号
        private const int WaitTime = 1000;
        private Dictionary<int, Thread> _waitingResponedThreads;
        private const string LocalIpAndPortString = "192.168.10.99:10012";
        private const string SystemIpAndPortString = "192.168.10.33:10011";
        private int SendCommandCount = 0;   //发送命令的序号，每发送一条+1
        private const int OutterFrameLength = 10;    //包外围长度为8个字节
        private const int x80CmdLength = 29;         //0x80命令29个字节长

        public SystemCommunicator()
        {
            _waitingResponedThreads = new Dictionary<int, Thread>();
            UdpEthernetCenter.RegisterIpAndPort(LocalIpAndPortString);
        }

        public void BeginRecvData()
        {
            UdpEthernetCenter.BeginRecvData(LocalIpAndPortString,SystemIpAndPortString, ProcessCommandData);
        }

        public void Send0x80Cmd(TargetTrack t)
        {
            //byte[] cmdData = new byte[OutterFrameLength + x80CmdLength];
            List<byte> cmdData = new List<byte>();
            cmdData.Add(LocalComponentNumber);
            cmdData.Add(0);
            byte[] sendCmdCountBytes = IntToByteLsb(SendCommandCount++, 2);
            cmdData.AddRange(sendCmdCountBytes);
            cmdData.Add(SysteComponentmNumber);
            cmdData.Add(0x80);
            cmdData.Add(0); //发送次数
            cmdData.Add(29);    //长度
            cmdData.Add(0);     //长度高位
            byte[] trackIdBytes = IntToByteLsb(t.trackID, 2);
            cmdData.AddRange(trackIdBytes);
            cmdData.Add((byte)DateTime.Now.Hour);
            cmdData.Add((byte) DateTime.Now.Minute);
            cmdData.Add((byte)DateTime.Now.Second); 
            cmdData.Add((byte)(DateTime.Now.Millisecond / 10));
            byte[] emptyBytes = new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0};
            cmdData.AddRange(emptyBytes);
            byte[] distanceBytes = IntToByteLsb((int) t.CurrentCoordinate.Dis, 4);
            cmdData.AddRange(distanceBytes);
            byte[] azBytes = IntToByteLsb((int) (t.AZ * 100), 2);
            cmdData.AddRange(azBytes);
            byte[] elBytes = IntToByteLsb((int) (t.EL * 100), 2);
            cmdData.AddRange(elBytes);
            cmdData.AddRange(new byte[]{0,0});

            byte[] cmdBytes = AddXorCheckByte(cmdData.ToArray());

            UdpEthernetCenter.SendData(cmdBytes, LocalIpAndPortString, SystemIpAndPortString);
        }

        private void ProcessCommandData(byte[] cmdData)     //处理接收的数据
        {
            if (CheckCommandValid(cmdData))
            {
                //校验通过
            }
            else
            {
                //校验不通过
            }
        }

        private bool CheckCommandValid(byte[] cmdData)  //检测命令有效性，校验字是否通过
        {
            int length = cmdData.Length - 1;
            byte xor = CalBytesXor(cmdData, 0, length);

            return xor == cmdData[cmdData.Length - 1];
        }

        private byte[] AddXorCheckByte(byte[] data)     //为给定数组添加异或和校验字
        {
            List<byte> ls = new List<byte>(data);
            byte xor = CalBytesXor(data, 0, data.Length);
            ls.Add(xor);

            return ls.ToArray();
        }

        public static byte CalBytesXor(byte[] data, int offset, int length) //计算异或和
        {
            int tmp = data[offset] ^ data[offset + 1];
            for (int dataIndex = offset + 2; dataIndex <= length - offset - 1; dataIndex++)
            {
                tmp = tmp ^ data[dataIndex];
            }

            return (byte) (tmp & 0xff);
        }

        public void SendCommand(byte[] cmdData)
        {
            Thread sendCommandThread = new Thread(SendData);
            _waitingResponedThreads.Add(GetCommandNumber(cmdData), sendCommandThread);
            sendCommandThread.Start();
        }

        private int GetCommandNumber(byte[] cmdData)
        {
            return cmdData[2];
        }

        private int GetCommandLength(byte[] cmdData)
        {
            return cmdData[6];
        }

        private void SendData(object data)
        {
            byte[] cmdData = (byte[]) data;
            UdpEthernetCenter.SendData(cmdData,LocalIpAndPortString,SystemIpAndPortString);   //发送三次，如果接收到回应则删除线程
            Thread.Sleep(WaitTime);
            UdpEthernetCenter.SendData(cmdData, LocalIpAndPortString, SystemIpAndPortString);
            Thread.Sleep(WaitTime);
            UdpEthernetCenter.SendData(cmdData, LocalIpAndPortString, SystemIpAndPortString);
            Thread.Sleep(WaitTime);

            //运行到此处还未被停止，则表示该命令没有得到回应
            ProcessCommandWithNoRespond(cmdData);
        }

        private void ProcessCommandWithNoRespond(byte[] cmdData)
        {

        }

        public static byte[] IntToByteLsb(int number, int reqiredMinimumLength)   //整型转化为byte数组，小端模式
        {
            List<byte> ls = new List<byte>();
            if (number < 255)
                ls.Add((byte)number);

            else
            {
                int tmp = number;

                for (int shiftCount = 0; ; shiftCount += 8)
                {
                    tmp = number >> shiftCount;
                    if (tmp == 0)
                        break; ;
                    byte b = (byte)(tmp & 0xff);
                    ls.Add(b);
                }
            }


            int actualLength = ls.Count;
            for (int i = 0; i < reqiredMinimumLength - actualLength; i++)
            {
                ls.Add(0);
            }

            return ls.ToArray();
        }
    }
}
