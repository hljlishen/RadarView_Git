using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public class TargetManagerSwitchModeCommand : TargetManagerCommand
    {
        TargetManagerMode mode;
        public TargetManagerSwitchModeCommand(TargetManagerMode mode)
        {
            this.mode = mode;
        }

        public override void Execute() => targetController.SwitchMode(mode);
    }
}
