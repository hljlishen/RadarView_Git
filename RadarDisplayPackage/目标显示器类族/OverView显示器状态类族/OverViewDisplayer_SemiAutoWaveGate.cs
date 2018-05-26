using Utilities;

namespace RadarDisplayPackage
{
    class OverViewDisplayer_SemiAutoWaveGate : OverViewDisplayer_WaveGate
    {
        public OverViewDisplayer_SemiAutoWaveGate(OverViewDisplayer o) : base(o)
        {
            isSemiAutoWaveGate = true;
            waveGateBrush = displayer.Canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255, 255, 0));   //黄色
            waveGateBrush.Opacity = 0.5f;   //透明度
        }

        public override OverViewState GetState()
        {
            return OverViewState.SemiAutoWaveGate;
        }
    }
}
