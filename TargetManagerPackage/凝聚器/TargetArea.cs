using CycleDataDrivePackage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TargetManagerPackage
{
    public class TargetArea
    {
        public List<TargetAreaEdge> TotalAreaEdge { get; }
        private List<TargetAreaEdge> _rigthMostEdges;
        public bool ExtentionEnd = false;
        private const int ExtentionEndTotalCount = 28;
        private const int ExtentionEndThreshold = 4;
        private readonly int[] _extentionStateRecord;
        private int _recordArrayIndex = -1;
        private bool _firstRound = true;
        private static int AreaWidthMinimum = 15;
        private static int AreaWidthMaximum = 80;
        public int Width { get; private set; } = 0;

        private int NextIndex()
        {
            if (_recordArrayIndex == ExtentionEndTotalCount - 1)
            {
                _recordArrayIndex = 0;
                _firstRound = false;
            }
            else
                _recordArrayIndex += 1;
            return _recordArrayIndex;
        }

        public TargetArea(List<TargetAreaEdge> firstEdges)
        {
            TotalAreaEdge = new List<TargetAreaEdge>(firstEdges);
            _rigthMostEdges = new List<TargetAreaEdge>(firstEdges);
            _extentionStateRecord = new int[ExtentionEndTotalCount];
        }

        public void Extend(ref List<TargetAreaEdge> edges) //区域延伸
        {
            List<TargetAreaEdge> newRightMostEdge = new List<TargetAreaEdge>();

            if (ExtentionEnd) //已经满足截止条件
                return;

            foreach (var targetAreaEdge in _rigthMostEdges)
            {
                foreach (var edge in edges)
                {
                    if (edge.IsInArea || !targetAreaEdge.AzWiseAdjacentTo(edge)) continue;
                    edge.IsInArea = true;
                    TotalAreaEdge.Add(edge);
                    newRightMostEdge.Add(edge);
                }
            }
            
            if (newRightMostEdge.Count == 0) //没有找到相邻的TargetAreaEdge
            {
                _extentionStateRecord[NextIndex()] = 0;
            }
            else
            {
                Width++;
                _extentionStateRecord[NextIndex()] = 1;
                _rigthMostEdges = newRightMostEdge;
            }

            //判断是否满足终止条件
            if (!_firstRound && _extentionStateRecord.Sum() < ExtentionEndThreshold)
                ExtentionEnd = true;
        }

        public  List<DistanceCell> GetDistanceCells()
        {
            List<DistanceCell> dots = new List<DistanceCell>();
            foreach (TargetAreaEdge edge in TotalAreaEdge)
            {
                dots.AddRange(edge.Cells);
            }

            return dots;
        }

        public static List<TargetDot> ClotAzCells(List<AzimuthCell> azCells) => ClotTargetDots(GetTargetAreas(azCells));

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
                List<TargetAreaEdge> edges = TargetAreaEdge.GetTargetAreaEdges(azimuthCell);
                foreach (TargetArea area in areas)      //先延伸现有的区域
                {
                    if (area.ExtentionEnd)
                        continue;    //已经终止延伸的区域不处理
                    area.Extend(ref edges);             //延伸区域
                }

                foreach (TargetAreaEdge edge in edges)  //将未合并的edge升级为area，供下一循环延伸
                {
                    if(edge.IsInArea)
                        continue;
                    areas.Add(new TargetArea(new List<TargetAreaEdge>(){edge}));
                    edge.IsInArea = true;
                }
            }

            int extendingCount = 0;
            for (int i = areas.Count - 1; i >=0; i--)
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
                foreach (TargetAreaEdge edge in extendingArea._rigthMostEdges)
                {
                    foreach (AzimuthCell azimuthCell in moreAzimuthCells)
                    {
                        List<TargetAreaEdge> edges = new List<TargetAreaEdge>();
                        foreach (DistanceCell disCell in azimuthCell.DisCells.Values)
                        {
                            if (!edge.IsDistanceCellAdjacentDistanceWise(disCell) || disCell.IsInAreaEdge)
                                continue;
                            disCell.IsInAreaEdge = true;
                            List<DistanceCell> distanceCells = new List<DistanceCell>() {disCell};
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
                if(area.Width < AreaWidthMinimum || area.Width > AreaWidthMaximum)
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
            return new TargetDot(){AZ = (float)az, EL = 0, Dis = dis, IsClotDot = true};
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

            az = AdjustAz(az);
            return new TargetDot(az, el, dis){IsClotDot = true};
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

    public class TargetAreaEdge
    {
        public List<DistanceCell> Cells { get; }
        public bool IsInArea { get; set; } = false;
        public float Angle { get; set; }

        public TargetAreaEdge(List<DistanceCell> cells)
        {
            Cells = new List<DistanceCell>(cells);
            if (cells != null && cells.Count != 0)
                Angle = cells[0].az;
        }

        public void AddDistanceCell(DistanceCell cell) => Cells.Add(cell);

        public void AddMultiDistanceCell(List<DistanceCell> cells) => Cells.AddRange(cells);

        public TargetAreaEdge Merge(TargetAreaEdge otherEdge)
        {
            TargetAreaEdge mergedEdge = new TargetAreaEdge(new List<DistanceCell>());
            mergedEdge.AddMultiDistanceCell(Cells);
            mergedEdge.AddMultiDistanceCell(otherEdge.Cells);
            return mergedEdge;
        }

        public bool IsDistanceCellAdjacentDistanceWise(DistanceCell cell)
        {
            foreach (DistanceCell distanceCell in Cells)
            {
                if (Math.Abs(distanceCell.Distance - cell.Distance) < 4)
                    return true;
            }

            return false;
        }

        public static List<TargetAreaEdge> GetTargetAreaEdges(AzimuthCell azCell)
        {
            List<TargetAreaEdge> ret = new List<TargetAreaEdge>();

            foreach (var disCellIndex in azCell.DisCells.Keys)
            {
                if(azCell.DisCells[disCellIndex].IsInAreaEdge) continue;
                TargetAreaEdge edge = GetCellTargetAreaEdge(azCell.DisCells[disCellIndex].index, azCell);
                if(edge != null)
                    ret.Add(edge);
            }

            return ret;
        }

        public static TargetAreaEdge GetCellTargetAreaEdge(int distanceCellIndex, AzimuthCell azimuthCell)
        {
            int upperIndex = distanceCellIndex + 1;     //fpga发送的数据是按距离从近至远排序的，所以只要搜索距离增长的相邻单元格
            if ( azimuthCell.DisCells[distanceCellIndex].IsInAreaEdge)  //当前单元格已被其他区域占用
                return null;    //返回空，表示没有区域

            //当前单元格未被其他区域占用
            azimuthCell.DisCells[distanceCellIndex].IsInAreaEdge = true;       //标记单元格被占用
            List<DistanceCell> disCells = new List<DistanceCell>() { azimuthCell.DisCells[distanceCellIndex] };
            TargetAreaEdge ret = new TargetAreaEdge(disCells);

            if (!azimuthCell.DisCells.ContainsKey(upperIndex))   //没有有相邻单元格
                return ret;

            //有相邻单元格
            if (!(azimuthCell.DisCells[upperIndex]).IsInAreaEdge)    //相邻单元格未被其他区域占用
                ret = ret.Merge(GetCellTargetAreaEdge(upperIndex, azimuthCell));      //递归调用此函数，寻找相邻单元格的相邻单元格

            //相邻单元格被其他区域占用
            return ret;
        }

        public bool AzWiseAdjacentTo(TargetAreaEdge otherEdge)
        {
            foreach (DistanceCell cell in Cells)
            {
                foreach (DistanceCell otherCell in otherEdge.Cells)
                {
                    if (Math.Abs(cell.index - otherCell.index) <= 4)
                        return true;
                }
            }

            return false;
        }
    }
}
