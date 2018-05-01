using CycleDataDrivePackage;
using System.Collections;
using System.Collections.Generic;

namespace TargetManagerPackage
{
    class AdjustClotter_47End : Clotter
    {
        Hashtable ranges;

        public AdjustClotter_47End()
        {
            ranges = new Hashtable();
        }
        public override void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            //List<int> keys = new List<int>();

            //MoveNewDotToOldDot(center);

            //foreach (AzimuthCell azCell in cells)
            //{
            //    keys.Clear();
            //    foreach (object index in ranges.Keys)
            //    {
            //        keys.Add((int)index);
            //    }
            //    foreach (int index in keys)   //先判断已有的区域是否延伸
            //    {
            //        List<DistanceCell> tmp = (List<DistanceCell>)ranges[index];
            //        if (azCell.DisCells.Contains(index))
            //        {
            //            tmp.Add((DistanceCell)azCell.DisCells[index]);;
            //        }
            //        else
            //        {
            //            tmp.Add(null);
            //            if (IsRangeEnd(tmp))
            //            {
            //                //用均值生成点迹
            //                TargetDot dot = GetDot(tmp);

            //                ranges.Remove(index);
            //                center.AddNewDot(dot);
            //            }
            //            else
            //            {
            //                //区域未达到最小长度
            //                //ranges.Remove(index);
            //            }
            //        }
            //    }
            //    foreach (DistanceCell disCell in azCell.DisCells.Values)
            //    {
            //        List<DistanceCell> dis = new List<DistanceCell>();
            //        dis.Add(disCell);
            //        try
            //        {
            //            ranges.Add(disCell.index, dis);
            //        }
            //        catch { }
            //    }
            //}

            //NotifyUpdateSectorDot(center);
        }

        protected override List<TargetDot> ClotAzCells(List<AzimuthCell> azCells)
        {
            throw new System.NotImplementedException();
        }

        protected bool IsRangeEnd(List<DistanceCell> ls)
        {
            int totalCount = 10;
            int totalNullCount = 9;
            if (ls.Count <= totalCount)   //长度不满足4/7准备，直接返回否
                return false;

            int nullCount = 0;
            for(int i = ls.Count - 1; i >= ls.Count - totalCount - 1; i--)   //访问最后7个距离单元，用4/7准则判断
            {
                if (ls[i] == null)
                    nullCount++;
            }

            if (nullCount >= totalNullCount)
                return true;
            else
                return false;
        }

        protected TargetDot GetDot(List<DistanceCell> ls)
        {
            //int indexSum = 0;
            float azSum = 0;
            int speedSum = 0;
            float elSum = 0;
            int disSum = 0;
            int count = 0;
            foreach (DistanceCell dis in ls)
            {
                if (dis != null)
                {
                    //azSum += dis.azIndex;
                    disSum += dis.Distance;
                    elSum += dis.el;
                    speedSum += dis.speed;
                    count++;
                }
            }
            float az = azSum / count;
            float distance = disSum / count;
            float el = elSum / count;
            float speed = speedSum / count;

            TargetDot dot = new TargetDot(az, el, distance);
            return dot;
        }
    }
}
