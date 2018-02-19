using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public interface IWaveGateController
    {
        void AddWaveGate(WaveGate wg);  //请求添加新波门

        void DelWaveGate(WaveGate wg);  //请求删除波门

        void DeleteActiveWaveGate();    //删除所有活动（被选中）波门
    }
}
