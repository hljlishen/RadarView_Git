using CycleDataDrivePackage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TargetManagerPackage
{
    public class TargetArea
    {
        public List<TargetAreaEdge> TotalAreaEdge { get; }
        public List<TargetAreaEdge> RigthMostEdges { get; private set; }
        public bool ExtentionEnd = false;
        private const int ExtentionEndTotalCount = 28;
        private const int ExtentionEndThreshold = 4;
        private readonly int[] _extentionStateRecord;
        private int _recordArrayIndex = -1;
        private bool _firstRound = true;
        //private static int AreaWidthMinimum = 15;
        //private static int AreaWidthMaximum = 80;
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
            RigthMostEdges = new List<TargetAreaEdge>(firstEdges);
            _extentionStateRecord = new int[ExtentionEndTotalCount];
        }

        public void Extend(ref List<TargetAreaEdge> edges) //区域延伸
        {
            List<TargetAreaEdge> newRightMostEdge = new List<TargetAreaEdge>();

            if (ExtentionEnd) //已经满足截止条件
                return;

            foreach (var targetAreaEdge in RigthMostEdges)
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
                RigthMostEdges = newRightMostEdge;
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



        public bool AzWiseAdjacentTo(TargetAreaEdge otherEdge)
        {
            foreach (DistanceCell cell in Cells)
            {
                foreach (DistanceCell otherCell in otherEdge.Cells)
                {
                    if (Math.Abs(cell.index - otherCell.index) <= 4 && Math.Abs(cell.el - otherCell.el) < 1)
                        return true;
                }
            }

            return false;
        }
    }
}
