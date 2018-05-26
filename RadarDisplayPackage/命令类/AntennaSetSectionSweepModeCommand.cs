using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class AntennaSetSectionSweepModeCommand : AntennaCommand
    {
        private readonly AngleArea _angleArea;

        public AntennaSetSectionSweepModeCommand(float beginAngle, float endAngle)
        {
            _angleArea = new AngleArea(beginAngle,endAngle);
        }

        public AntennaSetSectionSweepModeCommand(AngleArea area)
        {
            _angleArea = area;
        }

        public override void Execute() => antenna.SetSectionSweepMode(_angleArea);
    }
}
