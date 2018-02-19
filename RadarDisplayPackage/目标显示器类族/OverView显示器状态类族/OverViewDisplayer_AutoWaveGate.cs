using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    class OverViewDisplayer_AutoWaveGate : OverViewDisplayer_WaveGate
    {
        public OverViewDisplayer_AutoWaveGate(OverViewDisplayer o) : base(o)
        {
            isSemiAutoWaveGate = false;
            waveGateBrush = displayer.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(245, 222, 179));   //淡黄色
            waveGateBrush.Opacity = 0.5f;   //透明度
        }

        public override OverViewState GetState()
        {
            return OverViewState.AutoWaveGate;
        }
    }
}
