using CycleDataDrivePackage;
using System;
using System.Collections.Generic;

//master branch
namespace TargetManagerPackage
{
    class CycleDataMatrix : ICycleDataObserver,IDisposable      //扫描数据管理器，存储天线扫描一周的所有数据
    {
        private AzimuthCell[] _matrix;       //保存周期数据的数组
        private int _currentMatrixIndex = 0;       //新数据存储位置           
        public const int AzimuthCellCount = 2048;        //数组的长度

        public CycleDataMatrix()
        {
            //初始化矩阵
            _matrix = new AzimuthCell[AzimuthCellCount];
        }

        public void SaveAzimuthCell(AzimuthCell data)   //添加周期数据
        {
            if (!ShouldSaveData(data))
                return;
            _currentMatrixIndex = GetNextIndex(_currentMatrixIndex);
            _matrix[_currentMatrixIndex]?.Dispose();
            _matrix[_currentMatrixIndex] = data;           //保存周期数据
        }

        private static int GetNextIndex(int currentIndex)
        {
            var ret = currentIndex + 1;

            if (ret >= AzimuthCellCount)
                ret = 0;

            return ret;
        }

        private bool ShouldSaveData(AzimuthCell data)
        {
            bool isPreviousDataNull = _matrix[_currentMatrixIndex] == null;
            bool isAngleNearPreviousData ;

            if (!isPreviousDataNull)
                isAngleNearPreviousData = Math.Abs(data.Angle - _matrix[_currentMatrixIndex].Angle) < 0.17578;
            else
                isAngleNearPreviousData = false;

            return !isAngleNearPreviousData;
        }

        public void NotifyNewCycleData(AzimuthCell data)
        {
            SaveAzimuthCell(data);
        }

        public AzimuthCell[] GetAzimuthCellArray(AngleArea area) //返回角度在begin和end之间的周期数据集合
        {
            List<AzimuthCell> ls = new List<AzimuthCell>();

            //没有保存过周期数据，返回一个0长度数组
            if (_matrix.Length == 0)
                return new AzimuthCell[0];

            foreach (AzimuthCell cell in _matrix)
            {
                if (cell == null)
                    continue;

                if (area.IsAngleInArea(cell.Angle)) //角度范围内的方位单元格保存引用
                    ls.Add(cell);
            }

            return ls.ToArray();
        }

        public void Clear()
        {
            _matrix = new AzimuthCell[AzimuthCellCount];
        }
        public void Dispose()
        {
            _matrix = null;
        }
    }
}
