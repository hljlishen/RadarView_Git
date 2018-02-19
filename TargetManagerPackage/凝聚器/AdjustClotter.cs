using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;
using System.Collections;

namespace TargetManagerPackage
{
    class AdjustClotter : Clotter
    {
        Hashtable ranges;

        public AdjustClotter()
        {
            ranges = new Hashtable();
        }
        public override void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            List<int> keys = new List<int>();

            MoveNewDotToOldDot(center);

            foreach (AzimuthCell azCell in cells)
            {
                keys.Clear();
                foreach (object index in ranges.Keys)
                {
                    keys.Add((int)index);
                }
                foreach (int index in keys)   //先判断已有的区域是否延伸
                {
                    if(azCell.disCells.Contains(index))
                    {
                        ((List<DistanceCell>)ranges[index]).Add((DistanceCell)azCell.disCells[index]);
                    }
                    else
                    {
                        if(((List<DistanceCell>)ranges[index]).Count >= 3)
                        {
                            //用均值生成点迹
                            List<DistanceCell> ls = ((List<DistanceCell>)ranges[index]);
                            //int indexSum = 0;
                            float azSum = 0;
                            int speedSum = 0;
                            float elSum = 0;
                            int disSum = 0;
                            foreach(DistanceCell dis in ls)
                            {
                                azSum += dis.azIndex;
                                disSum += dis.Distance;
                                elSum += dis.el;
                                speedSum += dis.speed;
                            }
                            float az = azSum / ls.Count;
                            float distance = disSum / ls.Count;
                            float el = elSum / ls.Count;
                            float speed = speedSum / ls.Count();

                            ranges.Remove(index);

                            TargetDot dot = new TargetDot(az, el, distance);
                            center.AddNewDot(dot);
                        }
                        else
                        {
                            //区域未达到最小长度
                            ranges.Remove(index);
                        }
                    }
                }
                foreach (DistanceCell disCell in azCell.disCells.Values)
                {
                    List<DistanceCell> dis = new List<DistanceCell>();
                    dis.Add(disCell);
                    try
                    {
                        ranges.Add(disCell.index, dis);
                    }
                    catch { }
                }
            }

            NotifyUpdateSectorDot(center);
        }
    }
}
