using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
    internal class FixedThresholdFilter : FilterTemplate
    {
        protected override bool Pass(DistanceCell cell)
        {
            int sum = 0;

            for(int i=1; i < DistanceCell.AMCount; i ++)    
            {
                sum += cell.speedAM[i];
            }

            if (sum == 0)   //1-7号速度回波幅度都是零则丢弃该距离单元
                return false;
            else
                return true;
        }
    }
}
