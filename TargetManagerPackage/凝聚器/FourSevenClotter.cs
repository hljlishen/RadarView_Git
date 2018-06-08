using CycleDataDrivePackage;
using System.Collections.Generic;
using System;

namespace TargetManagerPackage
{
    class FourSevenClotter : Clotter
    {
        private static int AreaWidthMinimum = 20;
        private static int AreaWidthMaximum = 150;
        //private int count = 0;
        public override void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            base.Clot(center, right,left,cells);

            List<TargetDot> dots = ClotAzCells(new List<AzimuthCell>(cells));

            foreach (var targetDot in dots)
            {
                if (center.IsAngleInArea(targetDot.Az))
                    center.AddNewDot(targetDot);
                if(right.IsAngleInArea(targetDot.Az))
                    right.AddPrepareDot(targetDot);
                if (left.IsAngleInArea(targetDot.Az))
                    left.AddPrepareDot(targetDot);
            }
            center.LoadPrepareDot();
            NotifyUpdateSectorDot(center);  //通知更新该扇区的目标点视图
        }

        protected override List<TargetDot> ClotAzCells(List<AzimuthCell> azCells)
        {
            List<TargetDot> dots = ClotTargetDots(GetTargetAreas(azCells));
            dots = MergeDotsNear(dots);
            return dots;
        }

        public List<TargetDot> MergeDotsNear(List<TargetDot> dots)
        {
            List<TargetDot> ls = dots;
            for (int i = ls.Count - 1; i >= 0; i--)   //从后向前遍历所有点，距离<50的点直接取均值，从后向前合并
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (ls[i].DistanceTo(ls[j]) < 80 && Math.Abs( ls[i].Dis - ls[j].Dis) < 4)
                    {
                        ls[j] = ls[i].GetMiddleDot(ls[j]);
                        ls[j].IsClotDot = true;
                        ls.RemoveAt(i);
                        break;
                    }
                }
            }

            return ls;
        }

        public static List<TargetAreaEdge> GetAzCellTargetAreaEdges(AzimuthCell azCell)
        {
            List<TargetAreaEdge> ret = new List<TargetAreaEdge>();

            foreach (var disCellIndex in azCell.DisCells.Keys)
            {
                if (azCell.DisCells[disCellIndex].Occupied) continue;
                TargetAreaEdge edge = GetDisCellTargetAreaEdge(azCell.DisCells[disCellIndex].index, azCell);
                if (edge != null)
                    ret.Add(edge);
            }

            return ret;
        }

        public static TargetAreaEdge GetDisCellTargetAreaEdge(int distanceCellIndex, AzimuthCell azimuthCell)
        {
            int upperIndex = distanceCellIndex + 1;     //fpga发送的数据是按距离从近至远排序的，所以只要搜索距离增长的相邻单元格
            if (azimuthCell.DisCells[distanceCellIndex].Occupied)  //当前单元格已被其他区域占用
                return null;    //返回空，表示没有区域

            //当前单元格未被其他区域占用
            azimuthCell.DisCells[distanceCellIndex].Occupied = true;       //标记单元格被占用
            List<DistanceCell> disCells = new List<DistanceCell>() { azimuthCell.DisCells[distanceCellIndex] };
            TargetAreaEdge ret = new TargetAreaEdge(disCells);

            if (!azimuthCell.DisCells.ContainsKey(upperIndex))   //没有有相邻单元格
                return ret;

            //有相邻单元格
            if (!(azimuthCell.DisCells[upperIndex]).Occupied)    //相邻单元格未被其他区域占用
                ret = ret.Merge(GetDisCellTargetAreaEdge(upperIndex, azimuthCell));      //递归调用此函数，寻找相邻单元格的相邻单元格

            //相邻单元格被其他区域占用
            return ret;
        }

        public static List<TargetArea> GetTargetAreas(List<AzimuthCell> azCells)
        {
            azCells.Sort();     //按方位排序
            List<AzimuthCell> moreAzimuthCells = new List<AzimuthCell>();

            //获取下个扇区的数据，准备将本扇区的TargetArea向下个扇区扩展
            if (TargetManagerFactory.CreateAntennaDataProvider().GetAntennaDirection() == RotateDirection.ClockWise)
            {
                if (azCells.Count > 0)
                {
                    AngleArea angleArea = new AngleArea(azCells[azCells.Count - 1].Angle,
                        azCells[azCells.Count - 1].Angle + 20.25f);
                    moreAzimuthCells = new List<AzimuthCell>(CycleDataMatrix.CreateCycleDataMatrix().AzimuthCellsInAngleArea(angleArea));
                    moreAzimuthCells.Sort();
                }
            }
            else
            {
                azCells.Reverse(); //沿着天线转动方向凝聚
                if (azCells.Count > 0)
                {
                    AngleArea angleArea = new AngleArea(azCells[azCells.Count - 1].Angle - 20.25f,
                        azCells[azCells.Count - 1].Angle);
                    moreAzimuthCells = new List<AzimuthCell>(CycleDataMatrix.CreateCycleDataMatrix().AzimuthCellsInAngleArea(angleArea));
                    moreAzimuthCells.Sort();
                    moreAzimuthCells.Reverse();
                }
            }

            List<TargetArea> areas = new List<TargetArea>();
            foreach (AzimuthCell azimuthCell in azCells)
            {
                List<TargetAreaEdge> edges = FourSevenClotter.GetAzCellTargetAreaEdges(azimuthCell);
                foreach (TargetArea area in areas)      //先延伸现有的区域
                {
                    if (area.ExtentionEnd)
                        continue;    //已经终止延伸的区域不处理
                    area.Extend(ref edges);             //延伸区域
                }

                foreach (TargetAreaEdge edge in edges)  //将未合并的edge升级为area，供下一循环延伸
                {
                    if (edge.IsInArea)
                        continue;
                    areas.Add(new TargetArea(new List<TargetAreaEdge>() { edge }));
                    edge.IsInArea = true;
                }
            }

            int extendingCount = 0;
            for (int i = areas.Count - 1; i >= 0; i--)
            {
                if (!areas[i].ExtentionEnd) //尚未结束的区域
                    extendingCount++;
            }

            if (extendingCount == 0)
                return areas;  //所有区域都已经结束扩展

            //未完成扩展的区域继续向下个扇区扩展
            foreach (TargetArea extendingArea in areas)
            {
                if (extendingArea.ExtentionEnd) continue;
                foreach (TargetAreaEdge edge in extendingArea.RigthMostEdges)
                {
                    foreach (AzimuthCell azimuthCell in moreAzimuthCells)
                    {
                        List<TargetAreaEdge> edges = new List<TargetAreaEdge>();
                        foreach (DistanceCell disCell in azimuthCell.DisCells.Values)
                        {
                            if (!edge.IsDistanceCellAdjacentDistanceWise(disCell) || disCell.Occupied)
                                continue;
                            disCell.Occupied = true;
                            List<DistanceCell> distanceCells = new List<DistanceCell>() { disCell };
                            TargetAreaEdge tae = new TargetAreaEdge(distanceCells);
                            edges.Add(tae);
                        }
                        extendingArea.Extend(ref edges);
                    }
                }
            }

            return areas;
        }

        public static List<TargetDot> ClotTargetDots(List<TargetArea> areas)
        {
            List<TargetDot> dots = new List<TargetDot>();
            foreach (TargetArea area in areas)
            {
                if (area.Width < AreaWidthMinimum || area.Width > AreaWidthMaximum)
                    continue;
                List<DistanceCell> disCells = area.GetDistanceCells();
                dots.Add(ClotSingleDot_MassCenter(disCells));
            }

            return dots;
        }

        public static TargetDot ClotSingleDot_MassCenter(List<DistanceCell> distanceCells)
        {
            double azPowerSum = 0;
            double powerSum = 0;
            int amMax = 0;
            float dis = 0;

            foreach (DistanceCell distanceCell in distanceCells)
            {
                powerSum += distanceCell.sumAM;
                azPowerSum += distanceCell.sumAM * distanceCell.az;

                //距离取最大幅度的距离
                if (amMax >= distanceCell.sumAM) continue;
                amMax = distanceCell.sumAM;
                dis = distanceCell.Distance;
            }

            double az = azPowerSum / powerSum;
            //az = AdjustAz((float)az);       //修正回差
            return new TargetDot() { Az = (float)az, El = 0, Dis = dis, IsClotDot = true };
        }

        private static TargetDot ClotSingleDot_MaxAm(List<DistanceCell> distanceCells)
        {
            PolarCoordinate c = new PolarCoordinate();
            int amMax = 0;
            float az = 0, el = 0, dis = 0;
            foreach (DistanceCell cell in distanceCells)
            {
                if (amMax >= cell.sumAM) continue;
                amMax = cell.sumAM;
                az = cell.az;
                el = cell.el;
                dis = cell.Distance;
            }

            //az = AdjustAz(az);
            return new TargetDot(az, el, dis) { IsClotDot = true };
        }

        private static float AdjustAz(float az)
        {
            RotateDirection direction = TargetManagerFactory.CreateAntennaDataProvider().GetAntennaDirection();
            switch (direction)
            {
                case RotateDirection.ClockWise:
                    return az - 0.5f;
                default:
                    return az + 0.5f;
            }
        }
    }
}
