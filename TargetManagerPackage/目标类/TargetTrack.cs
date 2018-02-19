using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class TargetTrack : Target,IDisposable
    {
        private static int[] id = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
        public int trackID = 10;         //批号
        internal PolarCoordinate predictLocation; //预测坐标
        public List<PolarCoordinate> locations; //历史坐标，最新的在最后
        public double speed;        //速度
        public int score;           //航迹评分

        public TargetTrack() : base()
        {
            locations = new List<PolarCoordinate>();
        }
        public TargetTrack(PolarCoordinate c)
        {
            currentCoordinate = c;
            locations = new List<PolarCoordinate>();
        }

        public void Update(PolarCoordinate c)
        {
            locations.Add(currentCoordinate.Copy());   //保存历史航迹
            currentCoordinate = c;
        }

        public static TargetTrack CreateTargetTrack(PolarCoordinate current, PolarCoordinate pre)
        {
            int trackid = -1;
            for(int i = 0; i < 16; i++)
            {
                if(id[i] != 1)  
                {
                    trackid = i;
                    id[i] = 1;
                    break;
                }
            }

            if (trackid == -1)
            {
                return null;
            }
            else
            {
                if(trackid == 1)
                {

                }
                TargetTrack t = new TargetTrack(current);
                t.locations.Add(pre);   //上周期自由点的位置添加为历史位置
                t.trackID = trackid + 1;
                return t;
            }
        }

        public static void ReleaseAllTrackIDs()
        {

        }

        public void Dispose()
        {
            id[trackID - 1] = 0;    //释放ID号
            locations?.Clear();
        }
    }
}
