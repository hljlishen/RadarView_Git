namespace RadarDisplayPackage
{
    public class OverViewDisplayerAntennaControlCommand : OverViewDisplayerCommand
    {
        public OverViewDisplayerAntennaControlCommand( OverViewDisplayer ovd) : base(ovd)
        {

        }

        public override void Execute()
        {
            ovd.SwitchState(OverViewState.AntennaControl);
        }
    }
}
