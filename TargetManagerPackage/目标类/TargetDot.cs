﻿using System;
using CycleDataDrivePackage;
using Utilities;

namespace TargetManagerPackage
{
    public class TargetDot : Target
    {
        protected static int AzBytes = 2;
        protected static int ElBytes = 2;
        protected static int DisBytes = 2;
        public static int TotalBytes = AzBytes + ElBytes + DisBytes;

        public bool IsClotDot { get; set; } = false;

        //int am; //回波幅度值
        public TargetDot()
        {
            Adopted = false;
        }

        public TargetDot(byte[] data, int p, int sector)
        {
            sectorIndex = sector;
            int pos = p;
            int az = Tools.MakeInt(data, pos, AzBytes);
            pos += AzBytes;

            int dis = Tools.MakeInt(data, pos, DisBytes);
            pos += DisBytes;

            int el = Tools.MakeInt(data, pos, ElBytes);
            //pos += ElBytes;

            AZ = (float) az / 10;
            EL = (float) el / 10;
            Dis = dis;
        }

        public TargetDot(float az, float el, float dis)
        {
            AZ = az;
            EL = el;
            Dis = dis;
            CurrentCoordinate.ProjectedDis = (float)(dis * Math.Cos(Tools.AngleToRadian(el)));
        }

        public bool Adopted { get; set; }

        public bool ShouldDisplay { get; set; } = true;

        public TargetDot GetMiddleDot(TargetDot dot)
        {
            float az = (AZ + dot.AZ) / 2;
            float el = (EL + dot.EL) / 2;
            float dis = (Dis + dot.Dis) / 2;
            int am = (amValue + dot.amValue) / 2;

            TargetDot dot1 = new TargetDot(az, el, dis){amValue = am};

            return dot1;
        }
    }
}
