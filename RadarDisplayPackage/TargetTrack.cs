using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RadarDisplayPackage
{
    public enum TrackType
    {
        无人机 = 1,
        行人 = 2,
        车辆 = 3
    }
    public class TargetTrack
    {
        //public int trackID; //批号
        //public PolarCoordinate currentLocation; //当前坐标
        //public PolarCoordinate predictLocation; //预测坐标
        //public List<PolarCoordinate> locations; //历史坐标，最新的在最后
        //public double speed;        //速度
        //public double courseAngle;  //航向
        //public double az;
        //public double el;
        //public double height;
        //public double distance;
        ////public TrackType type;
        //public int score; //航迹评分
    }

    abstract class Coordinate
    {
        public abstract double DistanceTo(Coordinate c);
        public abstract bool EqualsTo(Coordinate c);
    }

    class PolarCoordinate : Coordinate
    {
        //public double az;
        //public double el;
        //public double height;

        public override double DistanceTo(Coordinate c)
        {
            return 0.0;
        }

        public override bool EqualsTo(Coordinate c)
        {
            return false;
        }
    }
}
