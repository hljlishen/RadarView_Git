using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace TargetManagerPackage
{
    interface ICmdProcessor
    {
        void ExecuteCmd(byte[] cmdBytes);
        bool IsCmdExecutable(byte[] cmdBytes);
    }
    class RemoteController
    {
        protected IPort recvPort;
        protected List<ICmdProcessor> processors;

        public RemoteController(IPort recvPort)
        {
            processors = new List<ICmdProcessor>();
            this.recvPort = recvPort;
            recvPort.DataReceived += RecvPort_DataReceived;
            if (!recvPort.IsOpen())
                recvPort.Open();
        }

        private void RecvPort_DataReceived(byte[] recvData)
        {
            foreach(var p in processors)
            {
                if (!p.IsCmdExecutable(recvData)) continue;

                p.ExecuteCmd(recvData);
                break;
            }
        }

        public void AddProcessor(ICmdProcessor processor) => processors.Add(processor);
    }

    class Casa12thSectionSweepCmdProcessor : ICmdProcessor
    {
        private IAntennaController sweepController;

        public Casa12thSectionSweepCmdProcessor(IAntennaController sweepController)
        {
            this.sweepController = sweepController;
        }

        public void ExecuteCmd(byte[] cmdBytes)
        {
            ushort angle = BitConverter.ToUInt16(cmdBytes, 9);
            double angleD = (double)angle / 100;

            AngleArea area = CalSweepArea((float)angleD);
            sweepController.SetSectionSweepMode(area);
            sweepController.SetRotateRate(RotateRate.Rpm2);
        }

        public AngleArea CalSweepArea(float angle)
        {
            int sectorCount = TargetManagerFactory.CreateTargetDataProvider().GetSectorCount();
            double sectorCoverage = (double)360 / sectorCount;
            int sectorIndex = -1;
            AngleArea area = new AngleArea(0, 0);
            for(sectorIndex = 0; sectorIndex < sectorCount; sectorIndex++)
            {
                double begin = sectorCoverage * sectorIndex;
                double end = sectorCoverage * (sectorIndex + 1);
                area = new AngleArea(begin, end);
                if (area.IsAngleInArea(angle)) break;
            }

            double sweepAreaBegin = area.BeginAngle - sectorCoverage * 2;
            double sweepAreaEnd = area.EndAngle + sectorCoverage * 2;

            return new AngleArea(sweepAreaBegin, sweepAreaEnd);
        }

        public bool IsCmdExecutable(byte[] cmdBytes)
        {
           //if (cmdBytes.Length != 15) return false;
            if (cmdBytes[0] != 0x31) return false;

            return true;
        }
    }
}
