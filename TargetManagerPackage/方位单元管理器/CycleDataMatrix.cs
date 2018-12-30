using CycleDataDrivePackage;
using System;
using System.Collections.Generic;
using System.Linq;
using TargetManagerPackage.方位单元管理器;

namespace TargetManagerPackage
{
    internal class CycleDataMatrix : ICycleDataObserver, IDisposable      //扫描数据管理器，存储天线扫描一周的所有数据
    {
        public AzimuthCell[] Matrix { get; private set; }
        private int _currentMatrixIndex;       //新数据存储位置           
        public const int AzimuthCellCount = 256;        //数组的长度
        private static CycleDataMatrix _cycleDataMatrix;
        private readonly ITestDataGenerator _testDataGenerator;

        public CycleDataMatrix()
        {
            //初始化矩阵
            Matrix = new AzimuthCell[AzimuthCellCount];
            _testDataGenerator = new TargetSimulator(new AngleArea(20, 25));
        }


        public static CycleDataMatrix CreateCycleDataMatrix() => _cycleDataMatrix ?? (_cycleDataMatrix = new CycleDataMatrix());

        public void SaveAzimuthCell(AzimuthCell data)   //添加周期数据
        {
            //data = _testDataGenerator.ModifyOriginalData(data);
            _currentMatrixIndex = NextIndex(_currentMatrixIndex);
            Matrix[_currentMatrixIndex]?.Dispose();
            Matrix[_currentMatrixIndex] = data;           //保存周期数据
        }

        private AzimuthCell RemoveDistanceBelow(AzimuthCell cell, int dis)
        {
            Dictionary<int, DistanceCell> newCells = new Dictionary<int, DistanceCell>();
            foreach (int key in cell.DisCells.Keys)
            {
                if (key > dis)
                {
                    newCells.Add(key, cell.DisCells[key]);
                }
            }

            cell.DisCells = newCells;
            return cell;
        }

        private static int NextIndex(int currentIndex)
        {
            var ret = currentIndex + 1;

            if (ret >= AzimuthCellCount)
                ret = 0;

            return ret;
        }

        private bool ShouldSaveData(AzimuthCell data)
        {
            if (data.DisCells.Count == 0)   //没有距离单元格，直接返回false
                return false;

            bool isPreviousDataNull = Matrix[_currentMatrixIndex] == null;

            var isAngleNearPreviousData = !isPreviousDataNull && Math.Abs(data.Angle - Matrix[_currentMatrixIndex].Angle) < 0.175780;

            return !isAngleNearPreviousData;
        }

        public AzimuthCell[] AzimuthCellsInAngleArea(AngleArea area) //返回角度在begin和end之间的周期数据集合
        {
            //没有保存过周期数据，返回一个0长度数组
            return Matrix.Length == 0 ? new AzimuthCell[0] : Matrix.Where(cell => cell != null).Where(cell => area.IsAngleInArea(cell.Angle)).ToArray();
        }

        public AzimuthCell[] GetAzimuthCellsInSectorSpan(Sector previous, Sector next)
        {
            AngleArea area = CalCoveredAngleArea(previous, next);

            return AzimuthCellsInAngleArea(area);
        }

        private static AngleArea CalCoveredAngleArea(Sector s1, Sector s2)
        {
            float begin;
            float end;

            if (Math.Abs(s2.Index - s1.Index) > 2)   //大于2说明三个扇区跨越360度
            {
                begin = (s1.Index > s2.Index) ? s1.BeginAngle : s2.BeginAngle;
                end = (s1.Index < s2.Index) ? s1.EndAngle : s2.EndAngle;
            }
            else
            {
                begin = (s1.Index < s2.Index) ? s1.BeginAngle : s2.BeginAngle;
                end = (s1.Index > s2.Index) ? s1.EndAngle : s2.EndAngle;
            }

            return new AngleArea(begin, end);
        }

        public void Clear()
        {
            Matrix = new AzimuthCell[AzimuthCellCount];
            _currentMatrixIndex = 0;
        }

        public void Dispose() => Matrix = null;

        public void NotifyNewCycleData(byte[] rawData)
        {
            AzimuthCell cell = new AzimuthCell(rawData);
            SaveAzimuthCell(cell);
        }
    }
}
