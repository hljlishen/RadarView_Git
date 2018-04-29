using CycleDataDrivePackage;
using System;
using System.Linq;

namespace TargetManagerPackage
{
    internal class CycleDataMatrix : ICycleDataObserver, IDisposable      //扫描数据管理器，存储天线扫描一周的所有数据
    {
        private AzimuthCell[] _matrix;       //保存周期数据的数组
        private int _currentMatrixIndex;       //新数据存储位置           
        public const int AzimuthCellCount = 1024;        //数组的长度
        private static CycleDataMatrix _cycleDataMatrix;

        public CycleDataMatrix()
        {
            //初始化矩阵
            _matrix = new AzimuthCell[AzimuthCellCount];
        }

        public static CycleDataMatrix CreaCycleDataMatrix() => _cycleDataMatrix ?? (_cycleDataMatrix = new CycleDataMatrix());

        public void SaveAzimuthCell(AzimuthCell data)   //添加周期数据
        {
            if (!ShouldSaveData(data))
                return;
            _currentMatrixIndex = NextIndex(_currentMatrixIndex);
            _matrix[_currentMatrixIndex]?.Dispose();
            _matrix[_currentMatrixIndex] = data;           //保存周期数据
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

            bool isPreviousDataNull = _matrix[_currentMatrixIndex] == null;

            var isAngleNearPreviousData = !isPreviousDataNull && Math.Abs(data.Angle - _matrix[_currentMatrixIndex].Angle) < 0.175780;

            return !isAngleNearPreviousData;
        }

        public AzimuthCell[] AzimuthCellsInAngleArea(AngleArea area) //返回角度在begin和end之间的周期数据集合
        {
            //没有保存过周期数据，返回一个0长度数组
            return _matrix.Length == 0 ? new AzimuthCell[0] : _matrix.Where(cell => cell != null).Where(cell => area.IsAngleInArea(cell.Angle)).ToArray();
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

            if (Math.Abs(s2.index - s1.index) > 2)   //大于2说明三个扇区跨越360度
            {
                begin = (s1.index > s2.index) ? s1.BeginAngle : s2.BeginAngle;
                end = (s1.index < s2.index) ? s1.EndAngle : s2.EndAngle;
            }
            else
            {
                begin = (s1.index < s2.index) ? s1.BeginAngle : s2.BeginAngle;
                end = (s1.index > s2.index) ? s1.EndAngle : s2.EndAngle;
            }

            return new AngleArea(begin, end);
        }

        public void Clear()
        {
            _matrix = new AzimuthCell[AzimuthCellCount];
            _currentMatrixIndex = 0;
        }

        /// <summary>执行与释放或重置非托管资源相关的应用程序定义的任务。</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() => _matrix = null;

        public void NotifyNewCycleData(byte[] rawData)
        {
            AzimuthCell cell = new AzimuthCell(rawData);
            //cell.Angle = AntennaDataManager.ReverAngleDirection(cell.Angle);
            SaveAzimuthCell(cell);
        }
    }
}
