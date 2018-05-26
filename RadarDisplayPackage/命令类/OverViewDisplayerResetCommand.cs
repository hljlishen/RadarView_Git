namespace RadarDisplayPackage
{
    public class OverViewDisplayerResetCommand : OverViewDisplayerCommand
    {
        public OverViewDisplayerResetCommand(OverViewDisplayer ovd) : base(ovd)
        {

        }

        public override void Execute() => ovd.ResetView();
    }
}
