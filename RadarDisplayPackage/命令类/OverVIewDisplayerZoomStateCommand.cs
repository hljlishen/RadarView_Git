namespace RadarDisplayPackage
{
    public class OverViewDisplayerZoomStateCommand : OverViewDisplayerCommand
    {
        public OverViewDisplayerZoomStateCommand(OverViewDisplayer ovd) : base(ovd)
        {

        }

        public override void Execute() => ovd.SwitchState(OverViewState.Zoom);
    }
}
