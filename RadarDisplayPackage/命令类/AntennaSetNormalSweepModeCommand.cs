using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public class AntennaSetNormalSweepModeCommand : AntennaCommand
    {
        RotateDirection direction;
        public AntennaSetNormalSweepModeCommand( RotateDirection direction) : base()
        {
            this.direction = direction;
        }

        public override void Execute() => antenna.SetRotateDirection(direction);
    }
}
